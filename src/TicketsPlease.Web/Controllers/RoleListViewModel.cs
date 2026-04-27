// <copyright file="RoleListViewModel.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Web.Controllers;

using System;
using System.Collections.Generic;

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
