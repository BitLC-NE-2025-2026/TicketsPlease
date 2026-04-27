// <copyright file="SlaComplianceDto.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Dtos;

/// <summary>
/// Datentransferobjekt fÃ¼r den SLA-Status.
/// </summary>
/// <param name="ProjectName">Name des Projekts.</param>
/// <param name="TotalTickets">Gesamtanzahl Tickets.</param>
/// <param name="BreachedTickets">Anzahl verletzter Tickets.</param>
/// <param name="ComplianceRate">Einhaltungsrate.</param>
public record SlaComplianceDto(string ProjectName, int TotalTickets, int BreachedTickets, double ComplianceRate);
