// <copyright file="UserAddress.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Domain.Entities;

using System;
using TicketsPlease.Domain.Common;

/// <summary>
/// ReprÃ¤sentiert die postalische Adresse eines Benutzers.
/// </summary>
public class UserAddress : BaseAuditableEntity
{
  /// <summary>
  /// Gets or sets die ID des zugehÃ¶rigen Benutzers.
  /// </summary>
  public Guid UserId { get; set; }

  /// <summary>
  /// Gets or sets das Navigation-Property fÃ¼r den zugehÃ¶rigen Benutzer.
  /// </summary>
  public User? User { get; set; }

  /// <summary>
  /// Gets or sets den Namen der StraÃŸe / Hausnummer.
  /// </summary>
  public string Street { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets den Ortsnamen / die Stadt.
  /// </summary>
  public string City { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets die Postleitzahl.
  /// </summary>
  public string ZipCode { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets das Land.
  /// </summary>
  public string Country { get; set; } = string.Empty;
}
