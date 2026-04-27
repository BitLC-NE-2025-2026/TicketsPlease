// <copyright file="Team.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Domain.Entities;

using System;
using System.Collections.Generic;
using TicketsPlease.Domain.Common;

/// <summary>
/// ReprÃ¤sentiert ein Support- oder Bearbeitungsteam zur Gruppierung von Tickets und Agenten.
/// </summary>
public class Team : BaseEntity
{
  /// <summary>
  /// Gets or sets den Namen des Teams.
  /// </summary>
  public string Name { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets die Beschreibung der ZustÃ¤ndigkeiten des Teams.
  /// </summary>
  public string Description { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets den spezifischen Farbcode des Teams fÃ¼r Dashboards und UI.
  /// </summary>
  public string ColorCode { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets den Zeitpunkt (UTC), an dem das Team erstellt wurde.
  /// </summary>
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

  /// <summary>
  /// Gets or sets die ID des Benutzers, der das Team erstellt hat.
  /// </summary>
  public Guid CreatedByUserId { get; set; }

  /// <summary>
  /// Gets or sets das Navigation-Property fÃ¼r den Benutzer, der das Team angelegt hat.
  /// </summary>
  public User? CreatedByUser { get; set; }

  /// <summary>
  /// Gets die Liste der Teammitglieder.
  /// </summary>
  public virtual ICollection<TeamMember> Members { get; } = new List<TeamMember>();
}
