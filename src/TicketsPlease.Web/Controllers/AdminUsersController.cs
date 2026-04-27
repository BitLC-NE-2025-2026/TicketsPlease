// <copyright file="AdminUsersController.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Web.Controllers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketsPlease.Domain.Entities;
using TicketsPlease.Infrastructure.Persistence;

/// <summary>
/// Controller fÃ¼r die Benutzerverwaltung im Administrationsbereich (F2).
/// </summary>
[Authorize(Roles = "Admin")]
internal class AdminUsersController : Controller
{
  private readonly UserManager<User> userManager;
  private readonly RoleManager<Role> roleManager;
  private readonly TicketsPlease.Application.Common.Interfaces.IUserRepository userRepository;
  private readonly AppDbContext dbContext;

  /// <summary>
  /// Initializes a new instance of the <see cref="AdminUsersController"/> class.
  /// </summary>
  /// <param name="userManager">Die Benutzerverwaltung.</param>
  /// <param name="roleManager">Die Rollenverwaltung.</param>
  /// <param name="userRepository">Das Benutzer-Repository.</param>
  /// <param name="dbContext">Der Datenbankkontext.</param>
  public AdminUsersController(
      UserManager<User> userManager,
      RoleManager<Role> roleManager,
      TicketsPlease.Application.Common.Interfaces.IUserRepository userRepository,
      AppDbContext dbContext)
  {
    this.userManager = userManager;
    this.roleManager = roleManager;
    this.userRepository = userRepository;
    this.dbContext = dbContext;
  }

  /// <summary>
  /// Listet alle Benutzer im System auf.
  /// </summary>
  /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
  [HttpGet]
  public async Task<IActionResult> Index()
  {
    // Ignore query filters to include soft-deleted users
    var users = await this.userManager.Users.IgnoreQueryFilters().ToListAsync().ConfigureAwait(false);
    var userViewModels = new List<UserListViewModel>();

    foreach (var user in users)
    {
      var roles = await this.userManager.GetRolesAsync(user).ConfigureAwait(false);
      userViewModels.Add(new UserListViewModel
      {
        Id = user.Id,
        UserName = user.UserName ?? "Unknown",
        Email = user.Email ?? "Unknown",
        Roles = roles.ToList(),
        IsActive = user.IsActive,
        IsDeleted = user.IsDeleted,
        IsLockedOut = user.LockoutEnd != null && user.LockoutEnd > DateTimeOffset.UtcNow,
        TenantId = user.TenantId,
      });
    }

    return this.View(userViewModels);
  }

  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> ToggleDelete(Guid id)
  {
    var user = await this.userManager.Users.IgnoreQueryFilters().FirstOrDefaultAsync(u => u.Id == id).ConfigureAwait(false);
    if (user != null)
    {
      user.IsDeleted = !user.IsDeleted;
      user.DeletedAt = user.IsDeleted ? DateTime.UtcNow : null;
      await this.userManager.UpdateAsync(user).ConfigureAwait(false);
    }

    return this.RedirectToAction(nameof(this.Index));
  }

  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> ToggleLock(Guid id)
  {
    var user = await this.userManager.Users.IgnoreQueryFilters().FirstOrDefaultAsync(u => u.Id == id).ConfigureAwait(false);
    if (user != null)
    {
      bool isCurrentlyLocked = user.LockoutEnd != null && user.LockoutEnd > DateTimeOffset.UtcNow;
      user.LockoutEnd = isCurrentlyLocked ? null : DateTimeOffset.UtcNow.AddYears(100);
      await this.userManager.UpdateAsync(user).ConfigureAwait(false);
    }

    return this.RedirectToAction(nameof(this.Index));
  }

  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> ToggleActive(Guid id)
  {
    var user = await this.userManager.Users.IgnoreQueryFilters().FirstOrDefaultAsync(u => u.Id == id).ConfigureAwait(false);
    if (user != null)
    {
      user.IsActive = !user.IsActive;
      await this.userManager.UpdateAsync(user).ConfigureAwait(false);
    }

    return this.RedirectToAction(nameof(this.Index));
  }

  /// <summary>
  /// Zeigt das Formular zum Bearbeiten eines Benutzers an.
  /// </summary>
  /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
  [HttpGet]
  public async Task<IActionResult> Edit(Guid id)
  {
    var user = await this.userManager.FindByIdAsync(id.ToString()).ConfigureAwait(false);
    if (user == null)
    {
      return this.NotFound();
    }

    var userRoles = await this.userManager.GetRolesAsync(user).ConfigureAwait(false);
    var allRoles = await this.roleManager.Roles.Select(r => r.Name!).ToListAsync().ConfigureAwait(false);
    var profile = await this.userRepository.GetOrCreateProfileAsync(user.Id).ConfigureAwait(false);

    var allTenants = await this.dbContext.Organizations.ToDictionaryAsync(o => o.Id, o => o.Name).ConfigureAwait(false);
    var allTeams = await this.dbContext.Teams.ToDictionaryAsync(t => t.Id, t => t.Name).ConfigureAwait(false);
    var userTeamIds = await this.dbContext.TeamMembers.Where(tm => tm.UserId == user.Id).Select(tm => tm.TeamId).ToListAsync().ConfigureAwait(false);

    var model = new EditUserViewModel
    {
      Id = user.Id,
      UserName = user.UserName ?? string.Empty,
      Email = user.Email ?? string.Empty,
      UserRoles = userRoles.ToList(),
      AllRoles = allRoles,
      Position = profile.Position,
      TechStack = profile.TechStack,
      Street = profile.Street,
      HouseNumber = profile.HouseNumber,
      City = profile.City,
      Country = profile.Country,
      TenantId = user.TenantId,
      SelectedTeamIds = userTeamIds,
      AvailableTenants = allTenants,
      AvailableTeams = allTeams,
    };

    return this.View(model);
  }

  /// <summary>
  /// Speichert die Ã„nderungen an einem Benutzer.
  /// </summary>
  /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Edit(EditUserViewModel model)
  {
    if (!this.ModelState.IsValid)
    {
      return this.View(model);
    }

    var user = await this.userManager.FindByIdAsync(model.Id.ToString()).ConfigureAwait(false);
    if (user == null)
    {
      return this.NotFound();
    }

    user.Email = model.Email;
    user.UserName = model.UserName;

    var profile = await this.userRepository.GetOrCreateProfileAsync(user.Id).ConfigureAwait(false);
    profile.Position = model.Position;
    profile.TechStack = model.TechStack;
    profile.Street = model.Street;
    profile.HouseNumber = model.HouseNumber;
    profile.City = model.City;
    profile.Country = model.Country;
    await this.userRepository.UpdateProfileAsync(profile).ConfigureAwait(false);

    var result = await this.userManager.UpdateAsync(user).ConfigureAwait(false);
    if (!result.Succeeded)
    {
      foreach (var error in result.Errors)
      {
        this.ModelState.AddModelError(string.Empty, error.Description);
      }

      return this.View(model);
    }

    var userRoles = await this.userManager.GetRolesAsync(user).ConfigureAwait(false);
    var rolesToRemove = userRoles.Except(model.SelectedRoles).ToList();
    var rolesToAdd = model.SelectedRoles.Except(userRoles).ToList();

    if (rolesToRemove.Count > 0)
    {
      await this.userManager.RemoveFromRolesAsync(user, rolesToRemove).ConfigureAwait(false);
    }

    if (rolesToAdd.Count > 0)
    {
      await this.userManager.AddToRolesAsync(user, rolesToAdd).ConfigureAwait(false);
    }

    // Update Tenant
    user.TenantId = model.TenantId;
    await this.userManager.UpdateAsync(user).ConfigureAwait(false);

    // Update Teams
    var currentTeamMemberships = await this.dbContext.TeamMembers.Where(tm => tm.UserId == user.Id).ToListAsync().ConfigureAwait(false);
    this.dbContext.TeamMembers.RemoveRange(currentTeamMemberships.Where(tm => !model.SelectedTeamIds.Contains(tm.TeamId)));

    var existingTeamIds = currentTeamMemberships.Select(tm => tm.TeamId).ToList();
    foreach (var teamId in model.SelectedTeamIds)
    {
      if (!existingTeamIds.Contains(teamId))
      {
        this.dbContext.TeamMembers.Add(new TeamMember { Id = Guid.NewGuid(), TeamId = teamId, UserId = user.Id, IsTeamLead = false });
      }
    }

    await this.dbContext.SaveChangesAsync().ConfigureAwait(false);

    return this.RedirectToAction(nameof(this.Index));
  }
}
