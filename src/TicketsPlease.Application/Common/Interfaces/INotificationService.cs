// <copyright file="INotificationService.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Interfaces;

using System;
using System.Threading.Tasks;
using TicketsPlease.Application.Common.Dtos;

/// <summary>
/// Domain-neutraler Dienst fÃ¼r Echtzeit-Benachrichtigungen.
/// </summary>
public interface INotificationService
{
  /// <summary>
  /// Sendet eine Benachrichtigung an einen spezifischen Benutzer.
  /// </summary>
  /// <param name="userId">Die ID des Benutzers.</param>
  /// <param name="title">Der Titel der Benachrichtigung.</param>
  /// <param name="message">Die Nachricht.</param>
  /// <param name="link">Optionaler Link.</param>
  /// <returns>Ein Task.</returns>
  public Task SendNotificationToUserAsync(Guid userId, string title, string message, string? link = null);

  /// <summary>
  /// Sendet eine Benachrichtigung an alle verbundenen Benutzer.
  /// </summary>
  /// <param name="title">Der Titel der Benachrichtigung.</param>
  /// <param name="message">Die Nachricht.</param>
  /// <returns>Ein Task.</returns>
  public Task SendNotificationToAllAsync(string title, string message);

  /// <summary>
  /// Informiert Teilnehmer eines Tickets Ã¼ber eine Aktualisierung.
  /// </summary>
  /// <param name="ticketId">Die ID des Tickets.</param>
  /// <param name="message">Die Update-Nachricht.</param>
  /// <returns>Ein Task.</returns>
  public Task NotifyTicketUpdateAsync(Guid ticketId, string message);

  /// <summary>
  /// Informiert Teilnehmer eines Tickets Ã¼ber einen neuen Kommentar.
  /// </summary>
  /// <param name="ticketId">Die ID des Tickets.</param>
  /// <param name="comment">Der neue Kommentar.</param>
  /// <returns>Ein Task.</returns>
  public Task NotifyNewCommentAsync(Guid ticketId, CommentDto comment);

  /// <summary>
  /// Informiert Teilnehmer eines Tickets Ã¼ber eine neue Privatnachricht.
  /// </summary>
  /// <param name="receiverUserId">Die ID des EmpfÃ¤ngers.</param>
  /// <param name="message">Das Nachrichten-DTO.</param>
  /// <returns>Ein Task.</returns>
  public Task NotifyNewMessageAsync(Guid receiverUserId, MessageDto message);

  /// <summary>
  /// Ruft Benachrichtigungen fÃ¼r einen Benutzer ab.
  /// </summary>
  /// <param name="userId">Die Benutzer-ID.</param>
  /// <param name="limit">Maximalanzahl.</param>
  /// <param name="offset">Offset.</param>
  /// <returns>Liste von DTOs.</returns>
  public Task<List<NotificationDto>> GetNotificationsForUserAsync(Guid userId, int limit = 20, int offset = 0);

  /// <summary>
  /// Markiert eine Benachrichtigung als gelesen.
  /// </summary>
  /// <param name="notificationId">Die ID der Benachrichtigung.</param>
  /// <returns>Ein Task.</returns>
  public Task MarkAsReadAsync(Guid notificationId);

  /// <summary>
  /// Markiert alle Benachrichtigungen eines Benutzers als gelesen.
  /// </summary>
  /// <param name="userId">Die Benutzer-ID.</param>
  /// <returns>Ein Task.</returns>
  public Task MarkAllAsReadAsync(Guid userId);
}
