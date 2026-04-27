// <copyright file="MessageReadReceipt.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Domain.Entities;

using System;
using TicketsPlease.Domain.Common;

/// <summary>
/// ReprÃ¤sentiert eine LesebestÃ¤tigung (Read Receipt) fÃ¼r eine Nachricht.
/// </summary>
public class MessageReadReceipt : BaseEntity
{
  /// <summary>
  /// Gets or sets die ID der gelesenen Nachricht.
  /// </summary>
  public Guid MessageId { get; set; }

  /// <summary>
  /// Gets or sets das Navigation-Property fÃ¼r die gelesene Nachricht.
  /// </summary>
  public Message? Message { get; set; }

  /// <summary>
  /// Gets or sets die ID des Benutzers, der die Nachricht gelesen hat.
  /// </summary>
  public Guid UserId { get; set; }

  /// <summary>
  /// Gets or sets das Navigation-Property fÃ¼r den Benutzer, der die Nachricht gelesen hat.
  /// </summary>
  public User? User { get; set; }

  /// <summary>
  /// Gets or sets den Zeitpunkt (UTC), an dem die Nachricht als gelesen markiert wurde.
  /// </summary>
  public DateTime ReadAt { get; set; } = DateTime.UtcNow;
}
