// <copyright file="TicketCustomValue.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Domain.Entities;

using System;
using TicketsPlease.Domain.Common;

/// <summary>
/// ReprÃ¤sentiert den konkreten Wert eines benutzerdefinierten Feldes (Custom Field) fÃ¼r ein spezifisches Ticket.
/// </summary>
public class TicketCustomValue : BaseEntity
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
  /// Gets or sets die ID der benutzerdefinierten Felddefinition.
  /// </summary>
  public Guid FieldDefinitionId { get; set; }

  /// <summary>
  /// Gets or sets das Navigation-Property fÃ¼r die Felddefinition.
  /// </summary>
  public CustomFieldDefinition? FieldDefinition { get; set; }

  /// <summary>
  /// Gets or sets den als String gespeicherten Wert des benutzerdefinierten Feldes.
  /// </summary>
  public string Value { get; set; } = string.Empty;
}
