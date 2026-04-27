// <copyright file="CustomFieldDefinition.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Domain.Entities;

using TicketsPlease.Domain.Common;

/// <summary>
/// ReprÃ¤sentiert die Definition eines benutzerdefinierten Feldes fÃ¼r Tickets.
/// </summary>
public class CustomFieldDefinition : BaseEntity
{
  /// <summary>
  /// Gets or sets den Namen des benutzerdefinierten Feldes.
  /// </summary>
  public string Name { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets den Typ des Feldes (z.B. Text, Number, Date, List).
  /// </summary>
  public string FieldType { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets die (optionale) JSON-Konfiguration fÃ¼r das Feld, z.B. AuswahlmÃ¶glichkeiten fÃ¼r Listen.
  /// </summary>
  public string? ConfigurationJson { get; set; }
}
