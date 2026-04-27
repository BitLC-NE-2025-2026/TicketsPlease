// <copyright file="TicketLinkDto.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Dtos;

using System;
using TicketsPlease.Domain.Enums;

/// <summary>
/// DatenÃ¼bertragungsobjekt fÃ¼r eine Ticket-VerknÃ¼pfung (F7).
/// </summary>
/// <param name="Id">Die ID der VerknÃ¼pfung.</param>
/// <param name="SourceTicketId">Die ID des Quell-Tickets.</param>
/// <param name="SourceTicketTitle">Der Titel des Quell-Tickets.</param>
/// <param name="TargetTicketId">Die ID des Ziel-Tickets.</param>
/// <param name="TargetTicketTitle">Der Titel des Ziel-Tickets.</param>
/// <param name="LinkType">Der Typ der VerknÃ¼pfung.</param>
/// <param name="IsClosed">Gibt an, ob das verknÃ¼pfte Ticket geschlossen ist.</param>
public record TicketLinkDto(
    Guid Id,
    Guid SourceTicketId,
    string SourceTicketTitle,
    Guid TargetTicketId,
    string TargetTicketTitle,
    TicketLinkType LinkType,
    bool IsClosed);
