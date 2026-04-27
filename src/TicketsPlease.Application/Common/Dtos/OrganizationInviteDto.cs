// <copyright file="OrganizationInviteDto.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Dtos;

using System;

/// <summary>
/// Datentransferobjekt fÃ¼r eine Organisationseinladung.
/// </summary>
public record OrganizationInviteDto(
    Guid Token,
    Guid OrganizationId,
    string OrganizationName,
    DateTime ExpiresAt,
    string? TargetedEmail);
