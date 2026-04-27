// <copyright file="ICommentRepository.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Interfaces;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TicketsPlease.Domain.Entities;

/// <summary>
/// Definiert die Repository-Methoden fÃ¼r Kommentare (F5).
/// </summary>
public interface ICommentRepository
{
  /// <summary>
  /// Ruft alle Kommentare fÃ¼r ein Ticket ab.
  /// </summary>
  /// <param name="ticketId">Die ID des Tickets.</param>
  /// <param name="ct">Das CancellationToken.</param>
  /// <returns>Eine Liste von <see cref="Comment"/>.</returns>
  public Task<List<Comment>> GetByTicketIdAsync(Guid ticketId, CancellationToken ct = default);

  /// <summary>
  /// FÃ¼gt einen neuen Kommentar hinzu.
  /// </summary>
  /// <param name="comment">Der Kommentar.</param>
  /// <param name="ct">Das CancellationToken.</param>
  /// <returns>Ein Task fÃ¼r die asynchrone Operation.</returns>
  public Task AddAsync(Comment comment, CancellationToken ct = default);

  /// <summary>
  /// Speichert die Ã„nderungen in der Datenbank.
  /// </summary>
  /// <param name="ct">Das CancellationToken.</param>
  /// <returns>Die Anzahl der betroffenen DatensÃ¤tze.</returns>
  public Task<int> SaveChangesAsync(CancellationToken ct = default);
}
