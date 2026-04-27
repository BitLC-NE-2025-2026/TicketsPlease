// <copyright file="ITimeTrackingService.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Interfaces;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicketsPlease.Application.Common.Dtos;

/// <summary>
/// Definiert die GeschÃ¤ftslogik fÃ¼r die Zeiterfassung an Tickets (F2.1.4).
/// </summary>
public interface ITimeTrackingService
{
  /// <summary>
  /// Startet eine neue Zeiterfassung fÃ¼r einen Benutzer an einem Ticket.
  /// </summary>
  /// <param name="ticketId">Die ID des Tickets.</param>
  /// <param name="userId">Die ID des Benutzers.</param>
  /// <returns>Ein Task.</returns>
  public Task StartTimeTrackingAsync(Guid ticketId, Guid userId);

  /// <summary>
  /// Stoppt die aktuelle Zeiterfassung eines Benutzers an einem Ticket.
  /// </summary>
  /// <param name="ticketId">Die ID des Tickets.</param>
  /// <param name="userId">Die ID des Benutzers.</param>
  /// <returns>Ein Task.</returns>
  public Task StopTimeTrackingAsync(Guid ticketId, Guid userId);

  /// <summary>
  /// Ruft alle ZeiterfassungseintrÃ¤ge fÃ¼r ein Ticket ab.
  /// </summary>
  /// <param name="ticketId">Die ID des Tickets.</param>
  /// <returns>Eine Liste von <see cref="TimeLogDto"/>.</returns>
  public Task<IEnumerable<TimeLogDto>> GetTimeLogsAsync(Guid ticketId);

  /// <summary>
  /// PrÃ¼ft, ob fÃ¼r den Benutzer aktuell eine Zeiterfassung an diesem Ticket lÃ¤uft.
  /// </summary>
  /// <param name="ticketId">Die ID des Tickets.</param>
  /// <param name="userId">Die ID des Benutzers.</param>
  /// <returns>True, wenn eine Erfassung lÃ¤uft.</returns>
  public Task<bool> IsTimerRunningAsync(Guid ticketId, Guid userId);
}
