// <copyright file="WorkflowTransition.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Domain.Entities;

using System;
using TicketsPlease.Domain.Common;

/// <summary>
/// ReprÃ¤sentiert einen erlaubten ZustandsÃ¼bergang in einem Ticket-Workflow.
/// </summary>
public class WorkflowTransition : BaseEntity
{
  /// <summary>
  /// Gets or sets die ID des Ausgangszustands.
  /// </summary>
  public Guid FromStateId { get; set; }

  /// <summary>
  /// Gets or sets das Navigation-Property fÃ¼r den Ausgangszustand.
  /// </summary>
  public WorkflowState? FromState { get; set; }

  /// <summary>
  /// Gets or sets die ID des Zielzustands.
  /// </summary>
  public Guid ToStateId { get; set; }

  /// <summary>
  /// Gets or sets das Navigation-Property fÃ¼r den Zielzustand.
  /// </summary>
  public WorkflowState? ToState { get; set; }

  /// <summary>
  /// Gets or sets die (optionale) Rollen-ID, um den Ãœbergang auf eine bestimmte Rolle zu beschrÃ¤nken.
  /// </summary>
  public Guid? AllowedRoleId { get; set; }

  /// <summary>
  /// Gets or sets das Navigation-Property fÃ¼r die erlaubte Rolle.
  /// </summary>
  public Role? AllowedRole { get; set; }
}
