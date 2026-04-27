// <copyright file="CreateTicketTemplateDto.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Dtos;

using System;

/// <summary>
/// DTO zum Erstellen einer neuen Ticket-Vorlage.
/// </summary>
/// <param name="Name">Name der Vorlage.</param>
/// <param name="DescriptionMarkdownTemplate">Der Markdown-Body.</param>
/// <param name="DefaultPriorityId">Optionale PrioritÃ¤t.</param>
public record CreateTicketTemplateDto(
    string Name,
    string DescriptionMarkdownTemplate,
    Guid? DefaultPriorityId);
