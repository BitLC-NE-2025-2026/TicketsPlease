// <copyright file="SocialMessageLike.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Domain.Entities;

using System;
using System.ComponentModel.DataAnnotations;
using TicketsPlease.Domain.Common;

/// <summary>
/// Repräsentiert ein Like eines Benutzers für eine Social-Feed-Nachricht.
/// </summary>
public class SocialMessageLike : BaseEntity
{
  /// <summary>
  /// Gets or sets die ID der gelikten Nachricht.
  /// </summary>
  [Required]
  public Guid SocialMessageId { get; set; }

  /// <summary>
  /// Gets or sets die Navigation-Property für die Nachricht.
  /// </summary>
  public virtual SocialMessage? SocialMessage { get; set; }

  /// <summary>
  /// Gets or sets die ID des Benutzers, der das Like vergeben hat.
  /// </summary>
  [Required]
  public Guid UserId { get; set; }

  /// <summary>
  /// Gets or sets die Navigation-Property für den Benutzer.
  /// </summary>
  public virtual User? User { get; set; }

  /// <summary>
  /// Gets or sets den Zeitpunkt des Likes.
  /// </summary>
  public DateTime LikedAt { get; set; } = DateTime.UtcNow;
}
