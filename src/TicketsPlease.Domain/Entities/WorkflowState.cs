// <copyright file="WorkflowState.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Domain.Entities;

using TicketsPlease.Domain.Common;

/// <summary>
/// ReprÃ¤sentiert einen Zustand innerhalb des Ticket-Workflows (z.B. Offen, In Bearbeitung, Geschlossen).
/// </summary>
public class WorkflowState : BaseEntity
{
  /// <summary>
  /// Gets or sets den Namen des Zustands.
  /// </summary>
  public string Name { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets die Reihenfolgenummer fÃ¼r die logische Anzeige im Kanban-Board.
  /// </summary>
  public int OrderIndex { get; set; }

  /// <summary>
  /// Gets or sets den Hexadezimal-Farbcode des Zustands fÃ¼r die UI-Darstellung.
  /// </summary>
  public string ColorHex { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets die ID des zugehÃ¶rigen Workflows.
  /// </summary>
  public Guid WorkflowId { get; set; }

  /// <summary>
  /// Gets or sets das Navigation-Property zum Workflow.
  /// </summary>
  public Workflow? Workflow { get; set; }

  /// <summary>
  /// Gets or sets a value indicating whether dieser Zustand der Endzustand (Terminal State) ist.
  /// </summary>
  public bool IsTerminalState { get; set; }
}
