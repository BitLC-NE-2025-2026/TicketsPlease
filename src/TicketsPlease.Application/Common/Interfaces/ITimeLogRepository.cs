// <copyright file="ITimeLogRepository.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Interfaces;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TicketsPlease.Domain.Entities;

/// <summary>
/// Definiert die Datenzugriffsschicht fÃ¼r <see cref="TimeLog"/> EntitÃ¤ten.
/// </summary>
public interface ITimeLogRepository
{
  /// <summary>
  /// Ruft alle ZeiterfassungseintrÃ¤ge ab.
  /// </summary>
  /// <param name="ct">Das Abbruchsignal.</param>
  /// <returns>Eine Liste von TimeLogs.</returns>
  public Task<List<TimeLog>> GetAllAsync(CancellationToken ct = default);

  /// <summary>
  /// Ruft alle ZeiterfassungseintrÃ¤ge fÃ¼r einen Tenant ab.
  /// </summary>
  /// <param name="tenantId">Die ID des Mandanten.</param>
  /// <param name="ct">Das Abbruchsignal.</param>
  /// <returns>Eine Liste von TimeLogs.</returns>
  public Task<List<TimeLog>> GetByTenantAsync(Guid tenantId, CancellationToken ct = default);

  /// <summary>
  /// Ruft alle ZeiterfassungseintrÃ¤ge fÃ¼r einen bestimmten Benutzer ab.
  /// </summary>
  /// <param name="userId">Die ID des Benutzers.</param>
  /// <param name="ct">Das Abbruchsignal.</param>
  /// <returns>Eine Liste von TimeLogs.</returns>
  public Task<List<TimeLog>> GetByUserIdAsync(Guid userId, CancellationToken ct = default);
}
