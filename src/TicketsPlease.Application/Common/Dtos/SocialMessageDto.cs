// <copyright file="SocialMessageDto.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Dtos;

using System;

/// <summary>
/// Data Transfer Object für Social Feed Nachrichten.
/// </summary>
public class SocialMessageDto
{
  public Guid Id { get; set; }
  public string ContentMarkdown { get; set; } = string.Empty;
  public string? AttachmentUrl { get; set; }
  public DateTime CreatedAt { get; set; }
  
  public Guid AuthorId { get; set; }
  public string AuthorAvatarUrl { get; set; } = string.Empty;
  public string AuthorUserName { get; set; } = string.Empty;
  public string AuthorFirstName { get; set; } = string.Empty;
  public string AuthorLastName { get; set; } = string.Empty;
  public string AuthorCompany { get; set; } = string.Empty;
  public string AuthorPosition { get; set; } = string.Empty;
  public string AuthorTeam { get; set; } = string.Empty;
  
  public bool CanDelete { get; set; }
}
