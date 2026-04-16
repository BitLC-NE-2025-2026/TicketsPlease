// <copyright file="ProjectHealthDto.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Dtos;

/// <summary>
/// Datentransferobjekt für den Projekt-Gesundheitsstatus.
/// </summary>
/// <param name="ProjectName">Name des Projekts.</param>
/// <param name="OpenTickets">Offene Tickets.</param>
/// <param name="UrgentTickets">Dringende Tickets.</param>
/// <param name="HealthStatus">Gesundheitsstatus.</param>
public record ProjectHealthDto(string ProjectName, int OpenTickets, int UrgentTickets, string HealthStatus);
