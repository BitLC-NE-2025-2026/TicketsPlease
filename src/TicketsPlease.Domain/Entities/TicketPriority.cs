// <copyright file="TicketPriority.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Domain.Entities;

using TicketsPlease.Domain.Common;

/// <summary>
/// ReprÃ¤sentiert die PrioritÃ¤t eines Tickets, z.B. Hoch, Mittel, Niedrig.
/// </summary>
public class TicketPriority : BaseEntity
{
  /// <summary>
  /// Gets or sets den Namen der PrioritÃ¤t.
  /// </summary>
  public string Name { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets die Gewichtung der PrioritÃ¤t zur Sortierung (hÃ¶here Zahl = wichtiger).
  /// </summary>
  public int LevelWeight { get; set; }

  /// <summary>
  /// Gets or sets den Hexadezimal-Farbcode der PrioritÃ¤t fÃ¼r die UI-Darstellung.
  /// </summary>
  public string ColorHex { get; set; } = string.Empty;
}
