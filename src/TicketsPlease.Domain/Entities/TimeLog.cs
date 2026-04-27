// <copyright file="TimeLog.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Domain.Entities;

using System;
using TicketsPlease.Domain.Common;

/// <summary>
/// ReprÃ¤sentiert einen Zeiterfassungseintrag fÃ¼r die Arbeit an einem Ticket.
/// </summary>
public class TimeLog : BaseEntity
{
  /// <summary>
  /// Gets or sets die ID des zugehÃ¶rigen Tickets.
  /// </summary>
  public Guid TicketId { get; set; }

  /// <summary>
  /// Gets or sets das Navigation-Property fÃ¼r das zugehÃ¶rige Ticket.
  /// </summary>
  public Ticket? Ticket { get; set; }

  /// <summary>
  /// Gets or sets die ID des Benutzers, der die Zeit erfasst hat.
  /// </summary>
  public Guid UserId { get; set; }

  /// <summary>
  /// Gets or sets das Navigation-Property fÃ¼r den Benutzer, der die Zeit erfasst hat.
  /// </summary>
  public User? User { get; set; }

  /// <summary>
  /// Gets or sets den Startzeitpunkt (UTC) der Zeiterfassung.
  /// </summary>
  public DateTime StartedAt { get; set; }

  /// <summary>
  /// Gets or sets den (optionalen) Endzeitpunkt (UTC) der Zeiterfassung.
  /// </summary>
  public DateTime? StoppedAt { get; set; }

  /// <summary>
  /// Gets or sets die manuell oder automatisch berechnete gebuchte Zeit in Stunden.
  /// </summary>
  public decimal HoursLogged { get; set; }

  /// <summary>
  /// Gets or sets eine (optionale) Beschreibung oder Bemerkung zur gebuchten Zeit.
  /// </summary>
  public string? Description { get; set; }
}
