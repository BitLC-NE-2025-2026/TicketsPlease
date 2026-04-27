// <copyright file="SlaPolicy.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Domain.Entities;

using System;
using TicketsPlease.Domain.Common;

/// <summary>
/// ReprÃ¤sentiert eine Service-Level-Agreement (SLA) Richtlinie fÃ¼r bestimmte Ticket-PrioritÃ¤ten.
/// </summary>
public class SlaPolicy : BaseEntity
{
  /// <summary>
  /// Gets or sets die ID der zugehÃ¶rigen Ticket-PrioritÃ¤t.
  /// </summary>
  public Guid PriorityId { get; set; }

  /// <summary>
  /// Gets or sets das Navigation-Property fÃ¼r die verknÃ¼pfte Ticket-PrioritÃ¤t.
  /// </summary>
  public TicketPriority? Priority { get; set; }

  /// <summary>
  /// Gets or sets die definierte Antwortzeit in Stunden.
  /// </summary>
  public int ResponseTimeHours { get; set; }

  /// <summary>
  /// Gets or sets die definierte LÃ¶sungszeit in Stunden.
  /// </summary>
  public int ResolutionTimeHours { get; set; }
}
