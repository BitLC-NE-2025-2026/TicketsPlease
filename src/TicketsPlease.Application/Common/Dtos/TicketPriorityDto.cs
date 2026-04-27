// <copyright file="TicketPriorityDto.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Dtos;

using System;

/// <summary>
/// DatenÃ¼bertragungsobjekt fÃ¼r Ticket-PrioritÃ¤ten.
/// </summary>
/// <param name="Id">Die ID der PrioritÃ¤t.</param>
/// <param name="Name">Der Anzeigename.</param>
/// <param name="ColorHex">Der Hex-Farbcode fÃ¼r die UI.</param>
public record TicketPriorityDto(
    Guid Id,
    string Name,
    string ColorHex);
