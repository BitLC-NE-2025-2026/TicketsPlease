// <copyright file="BaseAuditableEntity.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Domain.Common;

using System;

/// <summary>
/// Erweitert <see cref="BaseEntity"/> um Felder fÃ¼r die automatische ÃœberprÃ¼fung (Auditing).
/// </summary>
public abstract class BaseAuditableEntity : BaseEntity
{
  /// <summary>
  /// Gets or sets den Zeitpunkt der Erstellung.
  /// </summary>
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

  /// <summary>
  /// Gets or sets die ID oder den Namen des Benutzers, der die EntitÃ¤t erstellt hat.
  /// </summary>
  public string? CreatedBy { get; set; }

  /// <summary>
  /// Gets or sets den Zeitpunkt der letzten Ã„nderung.
  /// </summary>
  public DateTime? UpdatedAt { get; set; }

  /// <summary>
  /// Gets or sets die ID oder den Namen des Benutzers, der die letzte Ã„nderung vorgenommen hat.
  /// </summary>
  public string? UpdatedBy { get; set; }
}
