// <copyright file="RoleUserViewModel.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Web.Controllers;

using System;

/// <summary>
/// ViewModel fÃ¼r einen Benutzer innerhalb einer Rolle.
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
