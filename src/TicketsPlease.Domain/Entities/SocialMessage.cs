// <copyright file="SocialMessage.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Domain.Entities;

using System;
using System.ComponentModel.DataAnnotations;
using TicketsPlease.Domain.Common;

/// <summary>
/// ReprÃ¤sentiert eine Nachricht im tenant-Ã¼bergreifenden Social/Feed-System.
/// </summary>
public class SocialMessage : BaseAuditableEntity
{
  /// <summary>
  /// Gets or sets den Haupttext der Nachricht (Markdown formatiert).
  /// </summary>
  [Required]
  public string Content { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets eine optionale Medien-URL oder Dateianhang-URL.
  /// </summary>
  public string? AttachmentUrl { get; set; }

  /// <summary>
  /// Gets or sets den Verfasser der Nachricht.
  /// </summary>
  [Required]
  public Guid AuthorId { get; set; }

  /// <summary>
  /// Gets or sets die Navigation-Property des Verfassers.
  /// </summary>
  public virtual User? Author { get; set; }
}
