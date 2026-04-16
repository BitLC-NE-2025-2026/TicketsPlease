// <copyright file="TeamThroughputDto.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Dtos;

/// <summary>
/// Datentransferobjekt für den Durchsatz eines Teams.
/// </summary>
/// <param name="TeamName">Name des Teams.</param>
/// <param name="TicketsCompleted">Abgeschlossene Tickets.</param>
/// <param name="AveragePointsPerWeek">Durchschnittliche Punkte pro Woche.</param>
public record TeamThroughputDto(string TeamName, int TicketsCompleted, double AveragePointsPerWeek);
