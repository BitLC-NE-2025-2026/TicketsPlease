// <copyright file="StakeholderDashboardDto.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Dtos;

using System.Collections.ObjectModel;

/// <summary>
/// Zusammenfassendes DTO fÃ¼r das Stakeholder Dashboard.
/// </summary>
/// <param name="SlaCompliance">SLA Einhaltung.</param>
/// <param name="TeamThroughput">Team Durchsatz.</param>
/// <param name="ProjectHealth">Projekt Gesundheit.</param>
/// <param name="TotalActiveUsers">Gesamt aktive Benutzer.</param>
public record StakeholderDashboardDto(
    Collection<SlaComplianceDto> SlaCompliance,
    Collection<TeamThroughputDto> TeamThroughput,
    Collection<ProjectHealthDto> ProjectHealth,
    int TotalActiveUsers);
