// <copyright file="RoleViewModels.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Web.Controllers;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// ViewModel für die Rollenübersicht im Administrationsbereich.
/// </summary>
internal class RoleListViewModel
{
  /// <summary>
  /// Gets or sets die Rollen-ID.
  /// </summary>
  public Guid Id { get; set; }

  /// <summary>
  /// Gets or sets den Rollennamen.
  /// </summary>
  public string Name { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets die Rollenbeschreibung.
  /// </summary>
  public string Description { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets die Anzahl der Benutzer in dieser Rolle.
  /// </summary>
  public int UserCount { get; set; }

  /// <summary>
  /// Gets or sets die zugewiesenen Berechtigungen.
  /// </summary>
  public List<string> Permissions { get; set; } = new();

  /// <summary>
  /// Gets or sets a value indicating whether dies eine Systemrolle ist.
  /// </summary>
  public bool IsSystemRole { get; set; }
}

/// <summary>
/// ViewModel zum Bearbeiten/Erstellen einer Rolle.
/// </summary>
internal class EditRoleViewModel
{
  /// <summary>
  /// Gets or sets die Rollen-ID.
  /// </summary>
  public Guid Id { get; set; }

  /// <summary>
  /// Gets or sets den Rollennamen.
  /// </summary>
  [Required(ErrorMessage = "Role name is required.")]
  [StringLength(50, ErrorMessage = "Role name cannot exceed 50 characters.")]
  public string Name { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets die Rollenbeschreibung.
  /// </summary>
  [StringLength(200, ErrorMessage = "Description cannot exceed 200 characters.")]
  public string? Description { get; set; }

  /// <summary>
  /// Gets or sets die ausgewählten Berechtigungen.
  /// </summary>
  public List<string> SelectedPermissions { get; set; } = new();

  /// <summary>
  /// Gets or sets alle verfügbaren Berechtigungen (gruppiert).
  /// </summary>
  public Dictionary<string, List<PermissionDefinition>> AllPermissions { get; set; } = new();

  /// <summary>
  /// Gets or sets a value indicating whether dies eine Systemrolle ist (Name nicht änderbar).
  /// </summary>
  public bool IsSystemRole { get; set; }

  /// <summary>
  /// Gets or sets die Benutzer, die dieser Rolle zugewiesen sind.
  /// </summary>
  public List<RoleUserViewModel> UsersInRole { get; set; } = new();
}

/// <summary>
/// ViewModel für einen Benutzer innerhalb einer Rolle.
/// </summary>
internal class RoleUserViewModel
{
  /// <summary>
  /// Gets or sets die Benutzer-ID.
  /// </summary>
  public Guid Id { get; set; }

  /// <summary>
  /// Gets or sets den Benutzernamen.
  /// </summary>
  public string UserName { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets die E-Mail-Adresse.
  /// </summary>
  public string Email { get; set; } = string.Empty;
}
