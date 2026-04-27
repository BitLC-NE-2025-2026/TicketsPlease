// <copyright file="TicketUpvote.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Domain.Entities;

using System;
using TicketsPlease.Domain.Common;

/// <summary>
/// ReprÃ¤sentiert einen Upvote (Zustimmung/UnterstÃ¼tzung) eines Benutzers fÃ¼r ein Ticket (z.B. Feature Request).
/// </summary>
public class TicketUpvote : BaseEntity
{
  /// <summary>
  /// Gets or sets die ID des upgevoteten Tickets.
  /// </summary>
  public Guid TicketId { get; set; }

  /// <summary>
  /// Gets or sets das Navigation-Property fÃ¼r das Ticket.
  /// </summary>
  public Ticket? Ticket { get; set; }

  /// <summary>
  /// Gets or sets die ID des Benutzers, der den Upvote abgegeben hat.
  /// </summary>
  public Guid UserId { get; set; }

  /// <summary>
  /// Gets or sets das Navigation-Property fÃ¼r den abstimmenden Benutzer.
  /// </summary>
  public User? User { get; set; }

  /// <summary>
  /// Gets or sets den Zeitpunkt (UTC) des Upvotes.
  /// </summary>
  public DateTime VotedAt { get; set; } = DateTime.UtcNow;
}
