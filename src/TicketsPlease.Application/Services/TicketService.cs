// <copyright file="TicketService.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using TicketsPlease.Application.Common.Dtos;
using TicketsPlease.Application.Common.Interfaces;
using TicketsPlease.Domain.Entities;

/// <summary>
/// Implementierung des Ticket-Services zur Steuerung des Kanban-Boards.
/// </summary>
/// <param name="ticketRepository">Das Repository für Tickets.</param>
/// <param name="userManager">Die Benutzerverwaltung.</param>
/// <param name="roleManager">Die Rollenverwaltung.</param>
/// <param name="httpContextAccessor">Zugriff auf den HTTP-Kontext.</param>
/// <param name="fileAssetRepository">Das Repository für Datei-Metadaten.</param>
/// <param name="fileStorageService">Der Dienst zur Dateispeicherung.</param>
/// <param name="timeTrackingService">Der Dienst für Zeiterfassung.</param>
/// <param name="subTicketService">Der Dienst für Untertickets.</param>
/// <param name="notificationService">Der Dienst für Benachrichtigungen.</param>
public class TicketService(
    ITicketRepository ticketRepository,
    UserManager<User> userManager,
    RoleManager<Role> roleManager,
    IHttpContextAccessor httpContextAccessor,
    IFileAssetRepository fileAssetRepository,
    IFileStorageService fileStorageService,
    ITimeTrackingService timeTrackingService,
    ISubTicketService subTicketService,
    INotificationService notificationService) : ITicketService
{
  private const string TicketNotFoundMessage = "Ticket nicht gefunden.";
  private const string UnknownLiteral = "Unbekannt";

  /// <inheritdoc/>
  public async Task<IEnumerable<TicketDto>> GetActiveTicketsAsync()
  {
    var tickets = await ticketRepository.GetAllActiveAsync().ConfigureAwait(false);
    var dtos = new List<TicketDto>();
    foreach (var ticket in tickets)
    {
      dtos.Add(await this.MapToDtoAsync(ticket).ConfigureAwait(false));
    }

    return dtos;
  }

  /// <inheritdoc/>
  public async Task<IEnumerable<TicketDto>> GetFilteredTicketsAsync(
      Guid? projectId = null,
      Guid? assignedUserId = null,
      Guid? creatorId = null,
      string? status = null,
      Guid? priorityId = null,
      DateTime? fromDate = null,
      DateTime? toDate = null,
      string? searchString = null,
      Guid? tagId = null)
  {
    var tickets = await ticketRepository.GetFilteredAsync(
        projectId,
        assignedUserId,
        creatorId,
        status,
        priorityId,
        fromDate,
        toDate,
        searchString,
        tagId).ConfigureAwait(false);

    var dtos = new List<TicketDto>();
    foreach (var ticket in tickets)
    {
      dtos.Add(await this.MapToDtoAsync(ticket).ConfigureAwait(false));
    }

    return dtos;
  }

  /// <inheritdoc/>
  public async Task<TicketDto?> GetTicketAsync(Guid id)
  {
    var ticket = await ticketRepository.GetByIdAsync(id).ConfigureAwait(false);
    return ticket != null ? await this.MapToDtoAsync(ticket).ConfigureAwait(false) : null;
  }

  /// <inheritdoc/>
  public async Task CreateTicketAsync(CreateTicketDto dto)
  {
    ArgumentNullException.ThrowIfNull(dto);

    var user = await this.GetCurrentUserAsync().ConfigureAwait(false) ?? throw new UnauthorizedAccessException();

    var defaultState = await ticketRepository.GetDefaultWorkflowStateAsync().ConfigureAwait(false);
    var defaultStateId = defaultState?.Id ?? Guid.Empty;
    var defaultStateName = defaultState?.Name ?? "Todo";

    var ticket = new Ticket(dto.Title, TicketsPlease.Domain.Enums.TicketType.Task, dto.ProjectId, user.Id, defaultStateId, defaultStateName, "initial");
    ticket.UpdateDescription(dto.Description, dto.Description);
    ticket.AssignUser(dto.AssignedUserId);
    ticket.SetPriority(dto.PriorityId);
    ticket.SetEstimatePoints(dto.EstimatePoints);
    ticket.SetDifficulty(dto.ChilliesDifficulty);
    ticket.SetTenantId(user.TenantId);

    // Auto-SLA Assignment (Stage 3)
    ticket.SetSLA(TimeSpan.FromHours(4), TimeSpan.FromHours(48));
    await ticketRepository.AddHistoryAsync(new TicketHistory { TicketId = ticket.Id, FieldName = "SLA", OldValue = "None", NewValue = "Assigned (4h/48h)", ActorUserId = user.Id, ChangedAt = DateTime.UtcNow }).ConfigureAwait(false);

    if (dto.TagIds is { Count: > 0 })
    {
      ticket.SyncTags(dto.TagIds);
    }

    await ticketRepository.AddAsync(ticket).ConfigureAwait(false);
    _ = await ticketRepository.SaveChangesAsync().ConfigureAwait(false);
  }

  /// <inheritdoc/>
  public async Task UpdateTicketAsync(UpdateTicketDto dto)
  {
    ArgumentNullException.ThrowIfNull(dto);

    var ticket = await ticketRepository.GetByIdAsync(dto.Id).ConfigureAwait(false) ?? throw new KeyNotFoundException(TicketNotFoundMessage);

    // --- Concurrency Check (Optimistic Locking) ---
    if (dto.RowVersion != null)
    {
      ticketRepository.SetOriginalRowVersion(ticket, dto.RowVersion);
    }

    var user = await this.GetCurrentUserAsync().ConfigureAwait(false) ?? throw new UnauthorizedAccessException();
    if (ticket.Title != dto.Title)
    {
      await ticketRepository.AddHistoryAsync(new TicketHistory { TicketId = ticket.Id, FieldName = "Title", OldValue = ticket.Title, NewValue = dto.Title, ActorUserId = user.Id, ChangedAt = DateTime.UtcNow }).ConfigureAwait(false);
      ticket.UpdateTitle(dto.Title);
    }

    if (ticket.Description != dto.Description)
    {
      await ticketRepository.AddHistoryAsync(new TicketHistory { TicketId = ticket.Id, FieldName = "Description", OldValue = "---", NewValue = "Updated", ActorUserId = user.Id, ChangedAt = DateTime.UtcNow }).ConfigureAwait(false);
      ticket.UpdateDescription(dto.Description, dto.Description);
    }

    if (ticket.AssignedUserId != dto.AssignedUserId)
    {
      await ticketRepository.AddHistoryAsync(new TicketHistory { TicketId = ticket.Id, FieldName = "Assignee", OldValue = ticket.AssignedUserId?.ToString() ?? "None", NewValue = dto.AssignedUserId?.ToString() ?? "None", ActorUserId = user.Id, ChangedAt = DateTime.UtcNow }).ConfigureAwait(false);
      ticket.AssignUser(dto.AssignedUserId);
    }

    if (ticket.PriorityId != dto.PriorityId)
    {
      await ticketRepository.AddHistoryAsync(new TicketHistory { TicketId = ticket.Id, FieldName = "Priority", OldValue = ticket.Priority?.Name ?? ticket.PriorityId.ToString(), NewValue = dto.PriorityId.ToString(), ActorUserId = user.Id, ChangedAt = DateTime.UtcNow }).ConfigureAwait(false);
      ticket.SetPriority(dto.PriorityId);
    }

    if (ticket.EstimatePoints != dto.EstimatePoints)
    {
      await ticketRepository.AddHistoryAsync(new TicketHistory { TicketId = ticket.Id, FieldName = "Estimate", OldValue = ticket.EstimatePoints?.ToString(System.Globalization.CultureInfo.InvariantCulture) ?? "None", NewValue = dto.EstimatePoints?.ToString(System.Globalization.CultureInfo.InvariantCulture) ?? "None", ActorUserId = user.Id, ChangedAt = DateTime.UtcNow }).ConfigureAwait(false);
      ticket.SetEstimatePoints(dto.EstimatePoints);
    }

    if (ticket.ChilliesDifficulty != dto.ChilliesDifficulty)
    {
      await ticketRepository.AddHistoryAsync(new TicketHistory { TicketId = ticket.Id, FieldName = "Difficulty", OldValue = ticket.ChilliesDifficulty.ToString(System.Globalization.CultureInfo.InvariantCulture), NewValue = dto.ChilliesDifficulty.ToString(System.Globalization.CultureInfo.InvariantCulture), ActorUserId = user.Id, ChangedAt = DateTime.UtcNow }).ConfigureAwait(false);
      ticket.SetDifficulty(dto.ChilliesDifficulty);
    }

    if (dto.TagIds != null)
    {
      ticket.SyncTags(dto.TagIds);
    }

    _ = await ticketRepository.SaveChangesAsync().ConfigureAwait(false);
    await notificationService.NotifyTicketUpdateAsync(ticket.Id, "Ticket updated").ConfigureAwait(false);
  }

  /// <inheritdoc/>
  public async Task MoveTicketAsync(Guid id, string newStatus)
  {
    var ticket = await ticketRepository.GetByIdAsync(id).ConfigureAwait(false);
    var user = await this.GetCurrentUserAsync().ConfigureAwait(false);
    if (ticket == null || user == null)
    {
      throw new KeyNotFoundException(TicketNotFoundMessage);
    }

    var targetState = await ticketRepository.GetWorkflowStateByNameAsync(newStatus).ConfigureAwait(false) ?? throw new ArgumentException($"Ungültiger Status: {newStatus}");

    // Übergangsregel prüfen (F8)
    var transition = await ticketRepository.GetTransitionAsync(ticket.WorkflowStateId, targetState.Id).ConfigureAwait(false) ?? throw new InvalidOperationException($"Der Übergang von '{ticket.Status}' nach '{newStatus}' ist nicht erlaubt.");

    // Rollenprüfung falls eingeschränkt
    if (transition.AllowedRoleId.HasValue)
    {
      var roles = await userManager.GetRolesAsync(user).ConfigureAwait(false);

      // Wir gehen davon aus, dass wir die Rollen-Namen prüfen oder die ID vergleichen müssen.
      // Da wir in der Transition die RoleId haben, prüfen wir ob der User diese Rolle hat.
      // Besser: roleManager nutzen oder rollen-Strings vergleichen.
      // Einfachere Lösung für MVVM: User-Rollen gegen Namen prüfen wenn Role-ID bekannt ist.
      // Da wir statische IDs haben, können wir es hardcoden oder sauber auflösen.

      // Suche Rolle Name für ID
      var role = await roleManager.FindByIdAsync(transition.AllowedRoleId.Value.ToString()).ConfigureAwait(false);
      if (role != null && !roles.Contains(role.Name!))
      {
        throw new UnauthorizedAccessException($"Dieser Übergang ist nur für die Rolle '{role.Name}' erlaubt.");
      }
    }

    if (newStatus == "Done" || targetState.IsTerminalState)
    {
      await ticketRepository.AddHistoryAsync(new TicketHistory { TicketId = ticket.Id, FieldName = "Status", OldValue = ticket.Status, NewValue = newStatus, ActorUserId = user.Id, ChangedAt = DateTime.UtcNow }).ConfigureAwait(false);
      await this.CloseTicketAsync(id).ConfigureAwait(false);
    }
    else
    {
      await ticketRepository.AddHistoryAsync(new TicketHistory { TicketId = ticket.Id, FieldName = "Status", OldValue = ticket.Status, NewValue = newStatus, ActorUserId = user.Id, ChangedAt = DateTime.UtcNow }).ConfigureAwait(false);
      ticket.MoveToState(targetState.Id, newStatus);
      _ = await ticketRepository.SaveChangesAsync().ConfigureAwait(false);
      await notificationService.NotifyTicketUpdateAsync(ticket.Id, $"Status updated to {newStatus}").ConfigureAwait(false);
    }
  }

  /// <inheritdoc/>
  public async Task CloseTicketAsync(Guid id)
  {
    var ticket = await ticketRepository.GetByIdAsync(id).ConfigureAwait(false);
    var user = await this.GetCurrentUserAsync().ConfigureAwait(false);
    if (ticket == null || user == null)
    {
      throw new KeyNotFoundException();
    }

    if (!ticket.CanBeClosed())
    {
      throw new InvalidOperationException("Das Ticket kann nicht geschlossen werden, da es noch offene Abhängigkeiten (Vorgänger) hat.");
    }

    var roles = await userManager.GetRolesAsync(user).ConfigureAwait(false);
    var doneState = await ticketRepository.GetWorkflowStateByNameAsync("Done").ConfigureAwait(false);
    ticket.Close(user.Id, roles.Contains("Admin"), doneState?.Id);
    _ = await ticketRepository.SaveChangesAsync().ConfigureAwait(false);
    await notificationService.NotifyTicketUpdateAsync(ticket.Id, "Ticket closed").ConfigureAwait(false);
  }

  /// <inheritdoc/>
  public async Task AddDependencyAsync(Guid ticketId, Guid blockerId)
  {
    if (ticketId == blockerId)
    {
      throw new InvalidOperationException("Ein Ticket kann nicht von sich selbst abhängig sein.");
    }

    _ = await ticketRepository.GetByIdAsync(ticketId).ConfigureAwait(false) ?? throw new KeyNotFoundException($"Ticket {ticketId} nicht gefunden.");
    var blocker = await ticketRepository.GetByIdAsync(blockerId).ConfigureAwait(false) ?? throw new KeyNotFoundException($"Blocker-Ticket {blockerId} nicht gefunden.");

    // Zirkuläre Abhängigkeit prüfen: Kann das aktuelle Ticket (ticketId) den potenziellen Blocker (blockerId) bereits blockieren?
    if (await this.IsBlockedByRecursiveAsync(blockerId, ticketId).ConfigureAwait(false))
    {
      throw new InvalidOperationException("Zirkuläre Abhängigkeit erkannt: Das ausgewählte Ticket ist bereits direkt oder indirekt von diesem Ticket abhängig.");
    }

    // Check for existing link
    if (blocker.Blocking.Any(l => l.TargetTicketId == ticketId && l.LinkType == TicketsPlease.Domain.Enums.TicketLinkType.Blocks))
    {
      return;
    }

    blocker.AddLink(ticketId, TicketsPlease.Domain.Enums.TicketLinkType.Blocks);

    // Ensure TenantId is set (Ticket.AddLink only creates the object)
    var newLink = blocker.Blocking.Last();
    newLink.TenantId = blocker.TenantId;

    _ = await ticketRepository.SaveChangesAsync().ConfigureAwait(false);
  }

  private async Task<bool> IsBlockedByRecursiveAsync(Guid currentTicketId, Guid targetBlockerId, HashSet<Guid>? visited = null)
  {
    visited ??= new HashSet<Guid>();
    if (visited.Contains(currentTicketId))
    {
      return false;
    }

    visited.Add(currentTicketId);

    var ticket = await ticketRepository.GetByIdAsync(currentTicketId).ConfigureAwait(false);
    if (ticket == null)
    {
      return false;
    }

    // Prüfen ob einer der Blocker des aktuellen Tickets das Ziel-Ticket ist
    foreach (var link in ticket.BlockedBy.Where(l => l.LinkType == TicketsPlease.Domain.Enums.TicketLinkType.Blocks))
    {
      if (link.SourceTicketId == targetBlockerId)
      {
        return true;
      }

      if (await this.IsBlockedByRecursiveAsync(link.SourceTicketId, targetBlockerId, visited).ConfigureAwait(false))
      {
        return true;
      }
    }

    return false;
  }

  /// <inheritdoc/>
  public async Task RemoveDependencyAsync(Guid sourceId, Guid targetId)
  {
    var ticket = await ticketRepository.GetByIdAsync(sourceId).ConfigureAwait(false) ?? throw new KeyNotFoundException(TicketNotFoundMessage);

    var link = ticket.BlockedBy.Union(ticket.Blocking).FirstOrDefault(l => l.Id == targetId);
    if (link != null)
    {
      await ticketRepository.RemoveLinkAsync(link.Id).ConfigureAwait(false);
      _ = await ticketRepository.SaveChangesAsync().ConfigureAwait(false);
    }
  }

  /// <inheritdoc/>
  public async Task UploadAttachmentAsync(Guid ticketId, IFormFile file)
  {
    ArgumentNullException.ThrowIfNull(file);

    var user = await this.GetCurrentUserAsync().ConfigureAwait(false) ?? throw new UnauthorizedAccessException();

    _ = await ticketRepository.GetByIdAsync(ticketId).ConfigureAwait(false) ?? throw new KeyNotFoundException(TicketNotFoundMessage);

    using var stream = file.OpenReadStream();
    var blobPath = await fileStorageService.SaveFileAsync(stream, file.FileName).ConfigureAwait(false);

    var asset = new FileAsset
    {
      Id = Guid.NewGuid(),
      FileName = file.FileName,
      ContentType = file.ContentType,
      SizeBytes = file.Length,
      BlobPath = blobPath,
      UploadedByUserId = user.Id,
      TicketId = ticketId,
      UploadedAt = DateTime.UtcNow,
    };

    await fileAssetRepository.AddAsync(asset).ConfigureAwait(false);
    await fileAssetRepository.SaveChangesAsync().ConfigureAwait(false);
  }

  /// <inheritdoc/>
  public async Task<IEnumerable<TagDto>> GetAllTagsAsync()
  {
    var tags = await ticketRepository.GetAllTagsAsync().ConfigureAwait(false);
    return tags.Select(t => new TagDto(t.Id, t.Name, t.ColorHex));
  }

  /// <inheritdoc/>
  public async Task UpvoteAsync(Guid id)
  {
    var user = await this.GetCurrentUserAsync().ConfigureAwait(false);
    if (user != null && !await ticketRepository.UserHasUpvotedAsync(id, user.Id).ConfigureAwait(false))
    {
      await ticketRepository.AddUpvoteAsync(new TicketUpvote { TicketId = id, UserId = user.Id }).ConfigureAwait(false);
      _ = await ticketRepository.SaveChangesAsync().ConfigureAwait(false);
    }
  }

  /// <inheritdoc/>
  public async Task DownvoteAsync(Guid id)
  {
    var user = await this.GetCurrentUserAsync().ConfigureAwait(false);
    if (user != null)
    {
      await ticketRepository.RemoveUpvoteAsync(id, user.Id).ConfigureAwait(false);
      _ = await ticketRepository.SaveChangesAsync().ConfigureAwait(false);
    }
  }

  private async Task<TicketDto> MapToDtoAsync(Ticket t)
  {
    var comments = t.Comments?.OrderByDescending(c => c.CreatedAt).Select(c => new CommentDto(
      c.Id,
      c.Content,
      c.AuthorId,
      c.Author?.UserName ?? UnknownLiteral,
      c.CreatedAt)).ToList() ?? [];

    var blockedBy = t.BlockedBy?.Select(l => new TicketLinkDto(
          l.Id,
          l.SourceTicketId,
          l.SourceTicket?.Title ?? "???",
          l.TargetTicketId,
          l.TargetTicket?.Title ?? "???",
          l.LinkType,
          l.SourceTicket?.Status == "Closed" || l.SourceTicket?.Status == "Done")).ToList() ?? [];

    var blocking = t.Blocking?.Select(l => new TicketLinkDto(
          l.Id,
          l.SourceTicketId,
          l.SourceTicket?.Title ?? "???",
          l.TargetTicketId,
          l.TargetTicket?.Title ?? "???",
          l.LinkType,
          l.TargetTicket?.Status == "Closed" || l.TargetTicket?.Status == "Done")).ToList() ?? [];

    var attachments = t.Attachments?.Select(a => new FileAssetDto(
          a.Id,
          a.FileName,
          a.ContentType ?? "application/octet-stream",
          a.SizeBytes,
          a.UploadedAt,
          a.UploadedByUser?.UserName ?? UnknownLiteral)).ToList() ?? [];

    var tags = t.Tags?.Select(tt => new TagDto(tt.TagId, tt.Tag?.Name ?? UnknownLiteral, tt.Tag?.ColorHex ?? "#ccc")).ToList() ?? [];

    var timeLogs = (await timeTrackingService.GetTimeLogsAsync(t.Id).ConfigureAwait(false)).ToList();
    var subTickets = (await subTicketService.GetSubTicketsAsync(t.Id).ConfigureAwait(false)).ToList();
    var user = await this.GetCurrentUserAsync().ConfigureAwait(false);
    bool isTimerRunning = user != null && await timeTrackingService.IsTimerRunningAsync(t.Id, user.Id).ConfigureAwait(false);

    var history = t.History?.OrderByDescending(h => h.ChangedAt).Select(h => new TicketHistoryDto(
          h.Id,
          h.FieldName,
          h.OldValue ?? string.Empty,
          h.NewValue ?? string.Empty,
          h.ChangedAt,
          h.ActorUser?.UserName ?? UnknownLiteral)).ToList() ?? [];

    int upvoteCount = t.Upvotes?.Count ?? 0;
    bool hasUpvoted = user != null && (t.Upvotes?.Any(u => u.UserId == user.Id) ?? false);

    // Concurrency Token direkt aus der Entität (automatisch gefüllt durch EF)
    byte[] rowVersion = t.RowVersion ?? [];

    return new TicketDto(
        t.Id,
        t.Title,
        t.Description,
        t.Status,
        t.ProjectId,
        t.Project?.Title ?? UnknownLiteral,
        t.AssignedUserId,
        t.AssignedUser?.UserName ?? "Nicht zugewiesen",
        t.Type,
        new TicketPriorityDto(t.PriorityId, t.Priority?.Name ?? "Normal", t.Priority?.ColorHex ?? "#ccc"),
        t.CreatedAt,
        t.EstimatePoints,
        t.ChilliesDifficulty,
        tags,
        timeLogs,
        subTickets,
        isTimerRunning,
        comments,
        blockedBy,
        blocking,
        attachments,
        history,
        upvoteCount,
        hasUpvoted,
        rowVersion,
        t.ResponseDeadline,
        t.ResolutionDeadline,
        t.LastRespondedAt);
  }

  private async Task<User?> GetCurrentUserAsync()
  {
    return await userManager.GetUserAsync(httpContextAccessor.HttpContext!.User).ConfigureAwait(false);
  }
}
