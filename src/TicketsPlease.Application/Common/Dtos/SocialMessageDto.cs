// <copyright file="SocialMessageDto.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Dtos;

using System;

/// <summary>
/// Data Transfer Object fÃ¼r Social Feed Nachrichten.
/// </summary>
public class SocialMessageDto
{
  /// <summary>
  /// Gets or sets the unique identifier of the message.
  /// </summary>
  public Guid Id { get; set; }

  /// <summary>
  /// Gets or sets the content of the message in Markdown format.
  /// </summary>
  public string ContentMarkdown { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets the URL of an attachment associated with the message.
  /// </summary>
  public string? AttachmentUrl { get; set; }

  /// <summary>
  /// Gets or sets the date and time when the message was created.
  /// </summary>
  public DateTime CreatedAt { get; set; }

  /// <summary>
  /// Gets or sets the unique identifier of the author.
  /// </summary>
  public Guid AuthorId { get; set; }

  /// <summary>
  /// Gets or sets the URL of the author's avatar.
  /// </summary>
  public string AuthorAvatarUrl { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets the username of the author.
  /// </summary>
  public string AuthorUserName { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets the first name of the author.
  /// </summary>
  public string AuthorFirstName { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets the last name of the author.
  /// </summary>
  public string AuthorLastName { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets the company of the author.
  /// </summary>
  public string AuthorCompany { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets the position of the author.
  /// </summary>
  public string AuthorPosition { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets the team of the author.
  /// </summary>
  public string AuthorTeam { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets a value indicating whether the current user can delete the message.
  /// </summary>
  public bool CanDelete { get; set; }
}
