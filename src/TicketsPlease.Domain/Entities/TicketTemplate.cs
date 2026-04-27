// <copyright file="TicketTemplate.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Domain.Entities;

using System;
using TicketsPlease.Domain.Common;

/// <summary>
/// ReprÃ¤sentiert eine Vorlage zur schnelleren Erstellung von standardisierten Tickets.
/// </summary>
public class TicketTemplate : BaseEntity
{
  /// <summary>
  /// Gets or sets den Namen der Ticket-Vorlage.
  /// </summary>
  public string Name { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets die Markdown-Vorlage fÃ¼r die Beschreibung des Tickets.
  /// </summary>
  public string DescriptionMarkdownTemplate { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets die (optionale) Standard-PrioritÃ¤ts-ID fÃ¼r Tickets, die aus dieser Vorlage erstellt werden.
  /// </summary>
  public Guid? DefaultPriorityId { get; set; }

  /// <summary>
  /// Gets or sets das Navigation-Property fÃ¼r die Standard-PrioritÃ¤t.
  /// </summary>
  public TicketPriority? DefaultPriority { get; set; }

  /// <summary>
  /// Gets or sets die ID des Benutzers, der die Vorlage erstellt hat.
  /// </summary>
  public Guid CreatorId { get; set; }

  /// <summary>
  /// Gets or sets das Navigation-Property fÃ¼r den Ersteller der Vorlage.
  /// </summary>
  public User? Creator { get; set; }
}
