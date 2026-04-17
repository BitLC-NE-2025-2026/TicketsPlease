// <copyright file="AuditLogService.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Services;

using System;
using System.Threading.Tasks;
using TicketsPlease.Application.Common.Interfaces;
using TicketsPlease.Domain.Entities;

/// <summary>
/// Implementierung des Audit-Log-Dienstes zur Erfassung von Governance-Aktionen.
/// </summary>
/// <param name="organizationRepository">Das Organisations-Repository.</param>
public class AuditLogService(IOrganizationRepository organizationRepository) : IAuditLogService
{
  /// <inheritdoc/>
  public async Task LogActionAsync(Guid organizationId, Guid actorUserId, string actionType, string description)
  {
    var log = new AuditLog(organizationId, actorUserId, actionType, description);
    await organizationRepository.AddAuditLogAsync(log).ConfigureAwait(false);
    await organizationRepository.SaveChangesAsync().ConfigureAwait(false);
  }
}
