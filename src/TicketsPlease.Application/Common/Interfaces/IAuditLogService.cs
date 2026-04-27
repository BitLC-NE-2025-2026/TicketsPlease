// <copyright file="IAuditLogService.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Interfaces;

using System;
using System.Threading.Tasks;

/// <summary>
/// Interface fÃ¼r den Audit-Log-Dienst zur Erfassung von Governance-Aktionen.
/// </summary>
public interface IAuditLogService
{
  /// <summary>
  /// Erfasst eine Governance-Aktion im Audit-Log.
  /// </summary>
  /// <param name="organizationId">ID der Organisation.</param>
  /// <param name="actorUserId">ID des ausfÃ¼hrenden Benutzers.</param>
  /// <param name="actionType">Typ der Aktion.</param>
  /// <param name="description">Beschreibung der Ã„nderungen.</param>
  /// <returns>Task.</returns>
  public Task LogActionAsync(Guid organizationId, Guid actorUserId, string actionType, string description);
}
