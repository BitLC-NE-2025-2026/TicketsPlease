// <copyright file="MessageService.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TicketsPlease.Application.Common.Dtos;
using TicketsPlease.Application.Common.Interfaces;
using TicketsPlease.Domain.Entities;

/// <summary>
/// Implementiert die GeschÃ¤ftslogik fÃ¼r die Nachrichtenverwaltung.
/// </summary>
/// <param name="messageRepository">Das injizierte Repository.</param>
/// <param name="fileStorageService">Der Dienst zur Dateispeicherung.</param>
/// <param name="fileAssetRepository">Das Repository fÃ¼r Datei-Metadaten.</param>
/// <param name="notificationService">Der Benachrichtigungsdienst.</param>
/// <param name="userRepository">Das Repository fÃ¼r Benutzer.</param>
public class MessageService(
  IMessageRepository messageRepository,
  IFileStorageService fileStorageService,
  IFileAssetRepository fileAssetRepository,
  INotificationService notificationService,
  IUserRepository userRepository) : IMessageService
{
  /// <inheritdoc />
  public async Task<List<MessageDto>> GetUserMessagesAsync(Guid userId, CancellationToken ct = default)
  {
    var messages = await messageRepository.GetUserMessagesAsync(userId, ct).ConfigureAwait(false);
    return messages.Select(m => MapToDto(m)).ToList();
  }

  /// <inheritdoc />
  public async Task<List<MessageDto>> GetLatestUserMessagesAsync(Guid userId, int limit, CancellationToken ct = default)
  {
    var messages = await messageRepository.GetLatestUserMessagesAsync(userId, limit, ct).ConfigureAwait(false);
    return messages.Select(m => MapToDto(m)).ToList();
  }

  /// <inheritdoc />
  public async Task<MessageDto> SendMessageAsync(Guid senderId, CreateMessageDto dto, CancellationToken ct = default)
  {
    ArgumentNullException.ThrowIfNull(dto);

    var sender = await userRepository.GetUserWithDetailsAsync(senderId).ConfigureAwait(false);
    var tenantId = sender?.TenantId ?? Guid.Empty;

    var message = new Message
    {
      Id = Guid.NewGuid(),
      SenderUserId = senderId,
      ReceiverUserId = dto.ReceiverUserId,
      TeamId = dto.TeamId,
      TicketId = dto.TicketId,
      BodyMarkdown = dto.BodyMarkdown,
      SentAt = DateTime.UtcNow,
      TenantId = tenantId,
    };

    await messageRepository.AddAsync(message, ct).ConfigureAwait(false);
    await messageRepository.SaveChangesAsync(ct).ConfigureAwait(false);

    if (dto.Attachment != null)
    {
      await this.UploadAttachmentAsync(message.Id, dto.Attachment).ConfigureAwait(false);
    }

    // Fetch again to ensure navigation properties are loaded
    var savedMessage = await messageRepository.GetByIdAsync(message.Id, ct).ConfigureAwait(false);
    var mappedResult = MapToDto(savedMessage!);

    // Real-time notification for direct messages
    if (dto.ReceiverUserId.HasValue)
    {
      await notificationService.NotifyNewMessageAsync(dto.ReceiverUserId.Value, mappedResult).ConfigureAwait(false);
    }

    return mappedResult;
  }

  /// <inheritdoc />
  public async Task<List<MessageDto>> GetTeamMessagesAsync(Guid teamId, CancellationToken ct = default)
  {
    var messages = await messageRepository.GetTeamMessagesAsync(teamId, ct).ConfigureAwait(false);
    return messages.Select(m => MapToDto(m)).ToList();
  }

  /// <inheritdoc />
  public async Task<List<MessageDto>> GetGlobalMessagesAsync(CancellationToken ct = default)
  {
    var messages = await messageRepository.GetGlobalMessagesAsync(ct).ConfigureAwait(false);
    return messages.Select(m => MapToDto(m)).ToList();
  }

  /// <inheritdoc />
  public async Task UploadAttachmentAsync(Guid messageId, Microsoft.AspNetCore.Http.IFormFile file)
  {
    ArgumentNullException.ThrowIfNull(file);

    var message = await messageRepository.GetByIdAsync(messageId).ConfigureAwait(false);
    if (message == null)
    {
      throw new KeyNotFoundException("Nachricht nicht gefunden.");
    }

    using var stream = file.OpenReadStream();
    var blobPath = await fileStorageService.SaveFileAsync(stream, file.FileName).ConfigureAwait(false);

    var asset = new FileAsset
    {
      Id = Guid.NewGuid(),
      FileName = file.FileName,
      ContentType = file.ContentType,
      SizeBytes = file.Length,
      BlobPath = blobPath,
      UploadedByUserId = message.SenderUserId,
      MessageId = messageId,
      UploadedAt = DateTime.UtcNow,
    };

    await fileAssetRepository.AddAsync(asset).ConfigureAwait(false);
    await fileAssetRepository.SaveChangesAsync().ConfigureAwait(false);
  }

  /// <inheritdoc />
  public async Task<List<MessageDto>> GetConversationAsync(Guid userId, Guid otherUserId, CancellationToken ct = default)
  {
    var messages = await messageRepository.GetConversationAsync(userId, otherUserId, ct).ConfigureAwait(false);
    return messages.Select(m => MapToDto(m)).ToList();
  }

  private static MessageDto MapToDto(Message m)
  {
    if (m == null)
    {
      return new MessageDto(Guid.Empty, Guid.Empty, "System", null, null, null, null, "Error: Message could not be loaded", DateTime.UtcNow, Enumerable.Empty<FileAssetDto>());
    }

    var attachments = (m.Attachments ?? new List<FileAsset>())
        .Where(a => a != null)
        .Select(a => new FileAssetDto(
            a.Id,
            a.FileName ?? "Unknown",
            a.ContentType ?? "application/octet-stream",
            a.SizeBytes,
            a.UploadedAt,
            a.UploadedByUser?.UserName ?? "Unknown"))
        .ToList();

    return new MessageDto(
        m.Id,
        m.SenderUserId,
        m.SenderUser?.UserName ?? "Unknown",
        m.SenderUser?.Profile?.AvatarUrl,
        m.ReceiverUserId,
        m.ReceiverUser?.UserName,
        m.ReceiverUser?.Profile?.AvatarUrl,
        m.BodyMarkdown,
        m.SentAt,
        attachments);
  }
}
