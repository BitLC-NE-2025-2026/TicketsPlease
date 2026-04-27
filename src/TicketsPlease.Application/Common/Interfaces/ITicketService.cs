// <copyright file="ITicketService.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Interfaces;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TicketsPlease.Application.Common.Dtos;

/// <summary>
/// Definiert die GeschÃ¤ftslogik fÃ¼r das Ticket-Management (Kanban).
/// </summary>
public interface ITicketService
{
  /// <summary>
  /// Ruft alle aktiven Tickets fÃ¼r den aktuellen Mandanten ab.
  /// </summary>
  /// <returns>Eine Liste von <see cref="TicketDto"/>.</returns>
  public Task<IEnumerable<TicketDto>> GetActiveTicketsAsync();

  /// <summary>
  /// Ruft gefilterte Tickets ab (F6).
  /// </summary>
  /// <param name="projectId">Optionale Projekt-ID.</param>
  /// <param name="assignedUserId">Optionale Zuweisungs-ID.</param>
  /// <param name="creatorId">Optionale Ersteller-ID.</param>
  /// <param name="status">Optionaler Status (z.B. Todo, Doing).</param>
  /// <param name="priorityId">Optionale PrioritÃ¤ts-ID.</param>
  /// <param name="fromDate">Optionales Startdatum.</param>
  /// <param name="toDate">Optionales Enddatum.</param>
  /// <param name="searchString">Optionaler Suchstring fÃ¼r Titel/Beschreibung.</param>
  /// <param name="tagId">Optionale Tag-ID.</param>
  /// <returns>Eine Liste von <see cref="TicketDto"/>.</returns>
  public Task<IEnumerable<TicketDto>> GetFilteredTicketsAsync(
      Guid? projectId = null,
      Guid? assignedUserId = null,
      Guid? creatorId = null,
      string? status = null,
      Guid? priorityId = null,
      DateTime? fromDate = null,
      DateTime? toDate = null,
      string? searchString = null,
      Guid? tagId = null);

  /// <summary>
  /// Ruft ein spezifisches Ticket ab.
  /// </summary>
  /// <param name="id">Die ID des Tickets.</param>
  /// <returns>Ein <see cref="TicketDto"/> oder null.</returns>
  public Task<TicketDto?> GetTicketAsync(Guid id);

  /// <summary>
  /// Erstellt ein neues Ticket.
  /// </summary>
  /// <param name="dto">Die Ticketdaten.</param>
  /// <returns>Ein Task fÃ¼r die asynchrone Operation.</returns>
  public Task CreateTicketAsync(CreateTicketDto dto);

  /// <summary>
  /// Aktualisiert ein bestehendes Ticket.
  /// </summary>
  /// <param name="dto">Die aktualisierten Daten.</param>
  /// <returns>Ein Task fÃ¼r die asynchrone Operation.</returns>
  public Task UpdateTicketAsync(UpdateTicketDto dto);

  /// <summary>
  /// Verschiebt ein Ticket in einen neuen Status.
  /// </summary>
  /// <param name="id">Die ID des Tickets.</param>
  /// <param name="newStatus">Der Zielstatus.</param>
  /// <returns>Ein Task fÃ¼r die asynchrone Operation.</returns>
  public Task MoveTicketAsync(Guid id, string newStatus);

  /// <summary>
  /// SchlieÃŸt ein Ticket endgÃ¼ltig (F3.4).
  /// BerÃ¼cksichtigt AbhÃ¤ngigkeiten (Blocker) (F7).
  /// </summary>
  /// <param name="id">Die ID des zu schlieÃŸenden Tickets.</param>
  /// <returns>Ein Task fÃ¼r die asynchrone Operation.</returns>
  public Task CloseTicketAsync(Guid id);

  /// <summary>
  /// FÃ¼gt eine AbhÃ¤ngigkeit hinzu (F7).
  /// </summary>
  /// <param name="ticketId">Das blockierte Ticket (Nachfolger).</param>
  /// <param name="blockerId">Das blockierende Ticket (VorgÃ¤nger).</param>
  /// <returns>Ein Task fÃ¼r die asynchrone Operation.</returns>
  public Task AddDependencyAsync(Guid ticketId, Guid blockerId);

  /// <summary>
  /// Entfernt eine AbhÃ¤ngigkeit zwischen zwei Tickets.
  /// </summary>
  /// <param name="sourceId">Das Master-Ticket.</param>
  /// <param name="targetId">Das abhÃ¤ngige Ticket.</param>
  /// <returns>Ein Task.</returns>
  public Task RemoveDependencyAsync(Guid sourceId, Guid targetId);

  /// <summary>
  /// LÃ¤dt eine Datei als Anhang fÃ¼r ein Ticket hoch.
  /// </summary>
  /// <param name="ticketId">Die Ticket-ID.</param>
  /// <param name="file">Die Datei.</param>
  /// <returns>Ein Task.</returns>
  public Task UploadAttachmentAsync(Guid ticketId, IFormFile file);

  /// <summary>
  /// Ruft alle verfÃ¼gbaren Tags ab.
  /// </summary>
  /// <returns>Eine Liste von Tag-DTOs.</returns>
  public Task<IEnumerable<TagDto>> GetAllTagsAsync();

  /// <summary>
  /// Gibt einen Upvote fÃ¼r ein Ticket ab (Phase 2 Enterprise).
  /// </summary>
  /// <param name="id">Die ID des Tickets.</param>
  /// <returns>Ein Task.</returns>
  public Task UpvoteAsync(Guid id);

  /// <summary>
  /// Entfernt einen Upvote von einem Ticket.
  /// </summary>
  /// <param name="id">Die ID des Tickets.</param>
  /// <returns>Ein Task.</returns>
  public Task DownvoteAsync(Guid id);
}
