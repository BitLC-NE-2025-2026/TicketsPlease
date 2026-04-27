// <copyright file="FileAssetDto.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Dtos;

using System;

/// <summary>
/// DatenÃ¼bertragungsobjekt fÃ¼r DateianhÃ¤nge.
/// </summary>
/// <param name="Id">Die ID der Datei.</param>
/// <param name="FileName">Der ursprÃ¼ngliche Dateiname.</param>
/// <param name="ContentType">Der MIME-Typ.</param>
/// <param name="SizeBytes">Die GrÃ¶ÃŸe in Bytes.</param>
/// <param name="UploadedAt">Der Zeitpunkt des Uploads.</param>
/// <param name="UploadedUserName">Der Name des Urhebers.</param>
public record FileAssetDto(
    Guid Id,
    string FileName,
    string ContentType,
    long SizeBytes,
    DateTime UploadedAt,
    string UploadedUserName);
