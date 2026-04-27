// <copyright file="AuditLog.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Domain.Entities;

using System;
using TicketsPlease.Domain.Common;

/// <summary>
/// ReprÃ¤sentiert einen Log-Eintrag fÃ¼r Governance-Aktionen (PO-Level).
/// </summary>
public class AuditLog : BaseEntity
{
  /// <summary>
  /// Initializes a new instance of the <see cref="AuditLog"/> class.
  /// </summary>
  public AuditLog()
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="AuditLog"/> class.
  /// </summary>
  /// <param name="organizationId">ID der Organisation.</param>
  /// <param name="actorUserId">ID des Akteurs.</param>
  /// <param name="actionType">Aktionstyp.</param>
  /// <param name="description">Beschreibung.</param>
  public AuditLog(Guid organizationId, Guid actorUserId, string actionType, string description)
  {
    this.OrganizationId = organizationId;
    this.ActorUserId = actorUserId;
    this.ActionType = actionType;
    this.Description = description;
    this.Timestamp = DateTime.UtcNow;
  }

  /// <summary>
  /// Gets or sets die ID der Organisation, zu der dieser Log gehÃ¶rt.
  /// </summary>
  public Guid OrganizationId { get; set; }

  /// <summary>
  /// Gets or sets die ID des Benutzers, der die Aktion durchgefÃ¼hrt hat.
  /// </summary>
  public Guid ActorUserId { get; set; }

  /// <summary>
  /// Gets or sets den Namen des Akteurs (denormalisiert fÃ¼r schnelles Lesen).
  /// </summary>
  public string ActorName { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets den Typ der Aktion (z.B. "SLA_SETTINGS_UPDATE", "INVITE_GENERATED").
  /// </summary>
  public string ActionType { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets eine detaillierte Beschreibung der Aktion.
  /// </summary>
  public string Description { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets den Zeitpunkt der Aktion.
  /// </summary>
  public DateTime Timestamp { get; set; } = DateTime.UtcNow;

  /// <summary>
  /// Gets or sets das Navigation-Property fÃ¼r die Organisation.
  /// </summary>
  public virtual Organization? Organization { get; set; }
}
