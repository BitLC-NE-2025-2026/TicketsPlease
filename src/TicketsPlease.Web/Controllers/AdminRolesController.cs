// <copyright file="AdminRolesController.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Web.Controllers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketsPlease.Domain.Entities;
using TicketsPlease.Infrastructure.Persistence;

/// <summary>
/// Controller für die Rollenverwaltung im Administrationsbereich.
/// Bietet vollständige RBAC-Funktionalität inkl. Permission-Management.
/// </summary>
[Authorize(Roles = "Admin")]
internal class AdminRolesController : Controller
{
  private readonly RoleManager<Role> roleManager;
  private readonly UserManager<User> userManager;
  private readonly AppDbContext dbContext;

  /// <summary>
  /// Initializes a new instance of the <see cref="AdminRolesController"/> class.
  /// </summary>
  /// <param name="roleManager">Die Rollenverwaltung.</param>
  /// <param name="userManager">Die Benutzerverwaltung.</param>
  /// <param name="dbContext">Der Datenbankkontext.</param>
  public AdminRolesController(
      RoleManager<Role> roleManager,
      UserManager<User> userManager,
      AppDbContext dbContext)
  {
    this.roleManager = roleManager;
    this.userManager = userManager;
    this.dbContext = dbContext;
  }

  /// <summary>
  /// Listet alle Rollen mit zugehörigen Berechtigungen und Benutzeranzahl auf.
  /// </summary>
  /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
  [HttpGet]
  public async Task<IActionResult> Index()
  {
    var roles = await this.roleManager.Roles.ToListAsync().ConfigureAwait(false);
    var viewModels = new List<RoleListViewModel>();

    foreach (var role in roles)
    {
      var claims = await this.roleManager.GetClaimsAsync(role).ConfigureAwait(false);
      var usersInRole = await this.userManager.GetUsersInRoleAsync(role.Name!).ConfigureAwait(false);
      var permissions = claims
          .Where(c => c.Type == "Permission")
          .Select(c => c.Value)
          .ToList();

      viewModels.Add(new RoleListViewModel
      {
        Id = role.Id,
        Name = role.Name ?? "Unknown",
        Description = role.Description,
        UserCount = usersInRole.Count,
        Permissions = permissions,
        IsSystemRole = IsSystemRole(role.Name),
      });
    }

    return this.View(viewModels);
  }

  /// <summary>
  /// Zeigt das Formular zum Erstellen einer neuen Rolle.
  /// </summary>
  /// <returns>The create view.</returns>
  [HttpGet]
  public IActionResult Create()
  {
    var model = new EditRoleViewModel
    {
      AllPermissions = PermissionRegistry.AllPermissions,
    };
    return this.View(model);
  }

  /// <summary>
  /// Erstellt eine neue Rolle mit den ausgewählten Berechtigungen.
  /// </summary>
  /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Create(EditRoleViewModel model)
  {
    if (!this.ModelState.IsValid)
    {
      model.AllPermissions = PermissionRegistry.AllPermissions;
      return this.View(model);
    }

    var role = new Role
    {
      Name = model.Name,
      Description = model.Description ?? string.Empty,
    };

    var result = await this.roleManager.CreateAsync(role).ConfigureAwait(false);
    if (!result.Succeeded)
    {
      foreach (var error in result.Errors)
      {
        this.ModelState.AddModelError(string.Empty, error.Description);
      }

      model.AllPermissions = PermissionRegistry.AllPermissions;
      return this.View(model);
    }

    // Add selected permissions as claims
    foreach (var permission in model.SelectedPermissions)
    {
      await this.roleManager.AddClaimAsync(role, new Claim("Permission", permission)).ConfigureAwait(false);
    }

    this.TempData["Notification"] = $"Role \"{model.Name}\" created successfully.";
    return this.RedirectToAction(nameof(this.Index));
  }

  /// <summary>
  /// Zeigt das Bearbeitungsformular für eine bestehende Rolle.
  /// </summary>
  /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
  [HttpGet]
  public async Task<IActionResult> Edit(Guid id)
  {
    var role = await this.roleManager.FindByIdAsync(id.ToString()).ConfigureAwait(false);
    if (role == null)
    {
      return this.NotFound();
    }

    var claims = await this.roleManager.GetClaimsAsync(role).ConfigureAwait(false);
    var currentPermissions = claims
        .Where(c => c.Type == "Permission")
        .Select(c => c.Value)
        .ToList();

    var usersInRole = await this.userManager.GetUsersInRoleAsync(role.Name!).ConfigureAwait(false);

    var model = new EditRoleViewModel
    {
      Id = role.Id,
      Name = role.Name ?? string.Empty,
      Description = role.Description,
      SelectedPermissions = currentPermissions,
      AllPermissions = PermissionRegistry.AllPermissions,
      IsSystemRole = IsSystemRole(role.Name),
      UsersInRole = usersInRole.Select(u => new RoleUserViewModel
      {
        Id = u.Id,
        UserName = u.UserName ?? "Unknown",
        Email = u.Email ?? "Unknown",
      }).ToList(),
    };

    return this.View(model);
  }

  /// <summary>
  /// Speichert die Änderungen an einer Rolle und deren Berechtigungen.
  /// </summary>
  /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Edit(EditRoleViewModel model)
  {
    if (!this.ModelState.IsValid)
    {
      model.AllPermissions = PermissionRegistry.AllPermissions;
      return this.View(model);
    }

    var role = await this.roleManager.FindByIdAsync(model.Id.ToString()).ConfigureAwait(false);
    if (role == null)
    {
      return this.NotFound();
    }

    // Update basic properties (name only editable for non-system roles)
    if (!IsSystemRole(role.Name))
    {
      role.Name = model.Name;
    }

    role.Description = model.Description ?? string.Empty;
    var result = await this.roleManager.UpdateAsync(role).ConfigureAwait(false);
    if (!result.Succeeded)
    {
      foreach (var error in result.Errors)
      {
        this.ModelState.AddModelError(string.Empty, error.Description);
      }

      model.AllPermissions = PermissionRegistry.AllPermissions;
      return this.View(model);
    }

    // Sync permissions: remove old, add new
    var existingClaims = await this.roleManager.GetClaimsAsync(role).ConfigureAwait(false);
    var existingPermissions = existingClaims.Where(c => c.Type == "Permission").ToList();

    foreach (var claim in existingPermissions)
    {
      await this.roleManager.RemoveClaimAsync(role, claim).ConfigureAwait(false);
    }

    foreach (var permission in model.SelectedPermissions)
    {
      await this.roleManager.AddClaimAsync(role, new Claim("Permission", permission)).ConfigureAwait(false);
    }

    this.TempData["Notification"] = $"Role \"{role.Name}\" updated successfully.";
    return this.RedirectToAction(nameof(this.Index));
  }

  /// <summary>
  /// Löscht eine Rolle (nur nicht-Systemrollen).
  /// </summary>
  /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Delete(Guid id)
  {
    var role = await this.roleManager.FindByIdAsync(id.ToString()).ConfigureAwait(false);
    if (role == null)
    {
      return this.NotFound();
    }

    if (IsSystemRole(role.Name))
    {
      this.TempData["Notification"] = "System roles cannot be deleted.";
      return this.RedirectToAction(nameof(this.Index));
    }

    var usersInRole = await this.userManager.GetUsersInRoleAsync(role.Name!).ConfigureAwait(false);
    if (usersInRole.Count > 0)
    {
      this.TempData["Notification"] = $"Cannot delete role \"{role.Name}\" — {usersInRole.Count} users are still assigned.";
      return this.RedirectToAction(nameof(this.Index));
    }

    await this.roleManager.DeleteAsync(role).ConfigureAwait(false);
    this.TempData["Notification"] = $"Role \"{role.Name}\" deleted.";
    return this.RedirectToAction(nameof(this.Index));
  }

  /// <summary>
  /// Dupliziert eine bestehende Rolle mit all ihren Berechtigungen.
  /// </summary>
  /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Duplicate(Guid id)
  {
    var sourceRole = await this.roleManager.FindByIdAsync(id.ToString()).ConfigureAwait(false);
    if (sourceRole == null)
    {
      return this.NotFound();
    }

    var newRole = new Role
    {
      Name = $"{sourceRole.Name} (Copy)",
      Description = sourceRole.Description,
    };

    var result = await this.roleManager.CreateAsync(newRole).ConfigureAwait(false);
    if (result.Succeeded)
    {
      var claims = await this.roleManager.GetClaimsAsync(sourceRole).ConfigureAwait(false);
      foreach (var claim in claims.Where(c => c.Type == "Permission"))
      {
        await this.roleManager.AddClaimAsync(newRole, new Claim("Permission", claim.Value)).ConfigureAwait(false);
      }

      this.TempData["Notification"] = $"Role \"{sourceRole.Name}\" duplicated as \"{newRole.Name}\".";
    }

    return this.RedirectToAction(nameof(this.Index));
  }

  private static bool IsSystemRole(string? roleName)
  {
    return roleName is "Admin" or "Developer" or "Tester" or "ProductOwner" or "Stakeholder";
  }
}
