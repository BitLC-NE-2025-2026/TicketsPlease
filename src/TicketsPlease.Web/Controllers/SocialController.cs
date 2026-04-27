// <copyright file="SocialController.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Web.Controllers;

using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using TicketsPlease.Application.Common.Dtos;
using TicketsPlease.Domain.Entities;
using TicketsPlease.Infrastructure.Persistence;
using TicketsPlease.Web.Hubs;

/// <summary>
/// Controller fÃ¼r das tenant-Ã¼bergreifende Social Feed System.
/// </summary>
[Authorize]
internal class SocialController : Controller
{
  private readonly AppDbContext context;
  private readonly UserManager<User> userManager;
  private readonly IHubContext<NotificationHub> hubContext;
  private readonly IWebHostEnvironment env;

  /// <summary>
  /// Initializes a new instance of the <see cref="SocialController"/> class.
  /// </summary>
  /// <param name="context">Der Datenbankkontext.</param>
  /// <param name="userManager">Die Benutzerverwaltung.</param>
  /// <param name="hubContext">Der SignalR Hub Kontext.</param>
  /// <param name="env">Das Web Host Environment.</param>
  public SocialController(
      AppDbContext context,
      UserManager<User> userManager,
      IHubContext<NotificationHub> hubContext,
      IWebHostEnvironment env)
  {
    this.context = context;
    this.userManager = userManager;
    this.hubContext = hubContext;
    this.env = env;
  }

  /// <summary>
  /// Zeigt die Social Feed Index-Ansicht an.
  /// </summary>
  /// <returns>Die Index-View.</returns>
  public IActionResult Index()
  {
    return this.View();
  }

  /// <summary>
  /// LÃ¤dt die letzten Feed-Nachrichten.
  /// </summary>
  /// <returns>Eine Liste von SocialMessageDto.</returns>
  [HttpGet]
  public async Task<IActionResult> GetFeed()
  {
    var messages = await this.context.SocialMessages
        .Include(m => m.Author)
            .ThenInclude(u => u.Profile)
        .OrderByDescending(m => m.CreatedAt)
        .Take(50)
        .ToListAsync().ConfigureAwait(false);

    var isAdminOrPo = this.User.IsInRole("Admin") || this.User.IsInRole("ProductOwner");

    var organizationIds = messages.Select(m => m.Author?.TenantId).Where(id => id.HasValue).Distinct().Cast<Guid>().ToList();
    var orgs = await this.context.Organizations.Where(o => organizationIds.Contains(o.Id)).ToDictionaryAsync(o => o.Id, o => o.Name).ConfigureAwait(false);

    var dtos = messages.Select(m => new SocialMessageDto
    {
      Id = m.Id,
      ContentMarkdown = m.Content,
      AttachmentUrl = m.AttachmentUrl,
      CreatedAt = m.CreatedAt,
      AuthorId = m.AuthorId,
      AuthorAvatarUrl = m.Author?.Profile?.AvatarUrl?.ToString() ?? string.Empty,
      AuthorUserName = m.Author?.UserName ?? "Unknown",
      AuthorFirstName = m.Author?.Profile?.FirstName ?? string.Empty,
      AuthorLastName = m.Author?.Profile?.LastName ?? string.Empty,
      AuthorCompany = m.Author?.TenantId != null && orgs.ContainsKey(m.Author.TenantId) ? orgs[m.Author.TenantId] : (m.Author?.TenantId.ToString() ?? string.Empty),
      AuthorPosition = m.Author?.Profile?.Position ?? string.Empty,
      AuthorTeam = string.Empty,
      CanDelete = isAdminOrPo || m.AuthorId == Guid.Parse(this.userManager.GetUserId(this.User) ?? Guid.Empty.ToString()),
    }).ToList();

    return this.Ok(dtos);
  }

  /// <summary>
  /// Postet eine neue Nachricht.
  /// </summary>
  /// <param name="request">Die eingehenden Daten (Content, AttachmentUrl).</param>
  /// <returns>Das erstellte Dto.</returns>
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> PostMessage([FromBody] PostMessageRequest request)
  {
    if (string.IsNullOrWhiteSpace(request.Content))
    {
      return this.BadRequest("Content is required.");
    }

    var userIdStr = this.userManager.GetUserId(this.User);
    if (string.IsNullOrEmpty(userIdStr) || !Guid.TryParse(userIdStr, out var userId))
    {
      return this.Unauthorized();
    }

    var message = new SocialMessage
    {
      Id = Guid.NewGuid(),
      Content = request.Content,
      AttachmentUrl = request.AttachmentUrl,
      AuthorId = userId,
      CreatedAt = DateTime.UtcNow,
      TenantId = Guid.Empty, // Explicitly global/cross-tenant
    };

    this.context.SocialMessages.Add(message);
    await this.context.SaveChangesAsync().ConfigureAwait(false);

    // Reload with includes
    var savedMessage = await this.context.SocialMessages
        .Include(m => m.Author)
            .ThenInclude(u => u.Profile)
        .FirstOrDefaultAsync(m => m.Id == message.Id).ConfigureAwait(false);

    var isAdminOrPo = this.User.IsInRole("Admin") || this.User.IsInRole("ProductOwner");

    var orgName = savedMessage.Author?.TenantId.ToString() ?? string.Empty;
    if (savedMessage.Author != null && savedMessage.Author.TenantId != Guid.Empty)
    {
      var org = await this.context.Organizations.FindAsync(savedMessage.Author.TenantId).ConfigureAwait(false);
      if (org != null)
      {
        orgName = org.Name;
      }
    }

    var dto = new SocialMessageDto
    {
      Id = savedMessage!.Id,
      ContentMarkdown = savedMessage.Content,
      AttachmentUrl = savedMessage.AttachmentUrl,
      CreatedAt = savedMessage.CreatedAt,
      AuthorId = savedMessage.AuthorId,
      AuthorAvatarUrl = savedMessage.Author?.Profile?.AvatarUrl?.ToString() ?? string.Empty,
      AuthorUserName = savedMessage.Author?.UserName ?? "Unknown",
      AuthorFirstName = savedMessage.Author?.Profile?.FirstName ?? string.Empty,
      AuthorLastName = savedMessage.Author?.Profile?.LastName ?? string.Empty,
      AuthorCompany = orgName,
      AuthorPosition = savedMessage.Author?.Profile?.Position ?? string.Empty,
      AuthorTeam = string.Empty,
      CanDelete = isAdminOrPo || savedMessage.AuthorId == userId,
    };

    // Parse Mentions
    var matches = Regex.Matches(request.Content, @"@([a-zA-Z0-9_\-\.]+)");
    foreach (Match match in matches)
    {
      var mentionedUsername = match.Groups[1].Value;
      var mentionedUser = await this.userManager.FindByNameAsync(mentionedUsername).ConfigureAwait(false);
      if (mentionedUser != null && mentionedUser.Id != userId)
      {
        var notification = new Notification
        {
          Id = Guid.NewGuid(),
          UserId = mentionedUser.Id,
          Title = "New Social Mention",
          Content = $"You were mentioned by @{dto.AuthorUserName} in a Social post.",
          TargetUrl = "/Social",
          CreatedAt = DateTime.UtcNow,
        };
        this.context.Notifications.Add(notification);
        await this.hubContext.Clients.User(mentionedUser.Id.ToString()).SendAsync("ReceiveNotification", notification.Title).ConfigureAwait(false);
      }
    }

    await this.context.SaveChangesAsync().ConfigureAwait(false);

    // Broadcast to SignalR -> ReceiveSocialMessage
    await this.hubContext.Clients.All.SendAsync("ReceiveSocialMessage", dto).ConfigureAwait(false);

    return this.Ok(dto);
  }

  /// <summary>
  /// LÃ¶scht eine Nachricht via Soft-Delete.
  /// </summary>
  /// <param name="id">Die ID der Nachricht.</param>
  /// <returns>Ein OK Resultat.</returns>
  [HttpPost("Social/DeleteMessage/{id}")]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> DeleteMessage(Guid id)
  {
    var message = await this.context.SocialMessages.FindAsync(id).ConfigureAwait(false);
    if (message == null)
    {
      return this.NotFound();
    }

    var currentUserId = Guid.Parse(this.userManager.GetUserId(this.User) ?? Guid.Empty.ToString());
    var isAdminOrPo = this.User.IsInRole("Admin") || this.User.IsInRole("ProductOwner");

    if (!isAdminOrPo && message.AuthorId != currentUserId)
    {
      return this.Forbid();
    }

    // ChangeTracker triggers soft delete
    this.context.SocialMessages.Remove(message);
    await this.context.SaveChangesAsync().ConfigureAwait(false);

    await this.hubContext.Clients.All.SendAsync("SocialMessageDeleted", id).ConfigureAwait(false);

    return this.Ok();
  }

  /// <summary>
  /// LÃ¤dt ein File hoch fÃ¼r Social Attachments.
  /// </summary>
  /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
  [HttpPost("Social/Upload")]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> UploadAttachment(IFormFile file)
  {
    if (file == null || file.Length == 0)
    {
      return this.BadRequest("File empty");
    }

    var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".mp4", ".webm", ".mp3", ".wav" };
    if (!allowedExtensions.Contains(ext))
    {
      return this.BadRequest("Invalid file type.");
    }

    var uploadsFolder = Path.Combine(this.env.WebRootPath, "uploads", "social");
    if (!Directory.Exists(uploadsFolder))
    {
      Directory.CreateDirectory(uploadsFolder);
    }

    var uniqueName = $"{Guid.NewGuid()}{ext}";
    var filePath = Path.Combine(uploadsFolder, uniqueName);

    using (var stream = new FileStream(filePath, FileMode.Create))
    {
      await file.CopyToAsync(stream).ConfigureAwait(false);
    }

    return this.Ok(new { url = $"/uploads/social/{uniqueName}" });
  }

  /// <summary>
  /// LÃ¶st ein Ticket-Preview auf, nur wenn der User im selben Tenant ist.
  /// </summary>
  /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
  [HttpGet("Social/TicketPreview/{id}")]
  public async Task<IActionResult> GetTicketPreview(Guid id)
  {
    var userIdStr = this.userManager.GetUserId(this.User);
    if (string.IsNullOrEmpty(userIdStr) || !Guid.TryParse(userIdStr, out var userId))
    {
      return this.Unauthorized();
    }

    var user = await this.userManager.FindByIdAsync(userIdStr).ConfigureAwait(false);
    if (user == null)
    {
      return this.Unauthorized();
    }

    var ticket = await this.context.Tickets.FirstOrDefaultAsync(t => t.Id == id).ConfigureAwait(false);
    if (ticket == null)
    {
      return this.NotFound(new { error = "Ticket nicht gefunden" });
    }

    if (ticket.TenantId != user.TenantId)
    {
      return this.Forbid();
    }

    // Bereite minimales Preview Dto vor
    return this.Ok(new
    {
      id = ticket.Id,
      title = ticket.Title,
      status = ticket.Status.ToString(),
      priority = ticket.Priority.ToString(),
    });
  }

  /// <summary>
  /// Request DTO.
  /// </summary>
  internal class PostMessageRequest
  {
    /// <summary>
    /// Gets or sets the message content.
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets an optional attachment URL.
    /// </summary>
    public string? AttachmentUrl { get; set; }
  }
}
