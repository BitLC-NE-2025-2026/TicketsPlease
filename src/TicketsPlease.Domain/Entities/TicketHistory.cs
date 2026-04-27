// <copyright file="TicketHistory.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Domain.Entities;

using System;
using TicketsPlease.Domain.Common;

/// <summary>
/// ReprÃ¤sentiert einen Audit-Log-Eintrag fÃ¼r eine Ã„nderung an einem Ticket.
/// </summary>
public class TicketHistory : BaseEntity
{
  /// <summary>
  /// Gets or sets die ID des geÃ¤nderten Tickets.
  /// </summary>
  public Guid TicketId { get; set; }

  /// <summary>
  /// Gets or sets das Navigation-Property fÃ¼r das geÃ¤nderte Ticket.
  /// </summary>
  public Ticket? Ticket { get; set; }

  /// <summary>
  /// Gets or sets die ID des Benutzers, der die Ã„nderung vorgenommen hat.
  /// </summary>
  public Guid ActorUserId { get; set; }

  /// <summary>
  /// Gets or sets das Navigation-Property fÃ¼r den agierenden Benutzer.
  /// </summary>
  public User? ActorUser { get; set; }

  /// <summary>
  /// Gets or sets den Namen des geÃ¤nderten Feldes (z.B. Status, PrioritÃ¤t, Zuweisung).
  /// </summary>
  public string FieldName { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets den alten Wert vor der Ã„nderung als String.
  /// </summary>
  public string? OldValue { get; set; }

  /// <summary>
  /// Gets or sets den neuen Wert nach der Ã„nderung als String.
  /// </summary>
  public string? NewValue { get; set; }

  /// <summary>
  /// Gets or sets den Zeitpunkt (UTC) der Ã„nderung.
  /// </summary>
  public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
}
