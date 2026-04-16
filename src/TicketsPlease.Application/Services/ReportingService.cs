// <copyright file="ReportingService.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Services;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TicketsPlease.Application.Common.Dtos;
using TicketsPlease.Application.Common.Interfaces;

/// <summary>
/// Implementierung des ReportingService für Stakeholder-Auswertungen.
/// </summary>
/// <param name="projectRepository">Das Repository für Projekte.</param>
/// <param name="ticketRepository">Das Repository für Tickets.</param>
/// <param name="teamRepository">Das Repository für Teams.</param>
/// <param name="userRepository">Das Repository für Benutzer.</param>
public class ReportingService(
  IProjectRepository projectRepository,
  ITicketRepository ticketRepository,
  ITeamRepository teamRepository,
  IUserRepository userRepository) : IReportingService
{
  /// <inheritdoc />
  public async Task<StakeholderDashboardDto> GetStakeholderDashboardAsync(Guid tenantId)
  {
    var projects = (await projectRepository.GetAllAsync(tenantId).ConfigureAwait(false)).ToList();
    var allTickets = await ticketRepository.GetByTenantAsync(tenantId).ConfigureAwait(false);
    var teams = await teamRepository.GetTeamsByTenantAsync(tenantId).ConfigureAwait(false);
    var userCount = await userRepository.GetActiveUserCountAsync(tenantId).ConfigureAwait(false);

    var slaCompliance = projects.Select(p =>
    {
      var projectTickets = allTickets.Where(t => t.ProjectId == p.Id).ToList();
      var total = projectTickets.Count;
      var breached = projectTickets.Count(t => t.ClosedAt.HasValue && (t.ClosedAt.Value - t.CreatedAt).TotalHours > 24); // Beispiel-SLA: 24h
      var rate = total > 0 ? (double)(total - breached) / total * 100 : 100;
      return new SlaComplianceDto(p.Title, total, breached, Math.Round(rate, 2));
    }).ToList();

    // 2. Team Durchsatz
    var doneTickets = allTickets.Where(t => t.Status == "Done").ToList();

    var teamThroughput = teams.Select(team =>
    {
      var memberIds = team.Members.Select(m => m.UserId).ToList();
      var completedCount = doneTickets.Count(t => t.AssignedUserId.HasValue && memberIds.Contains(t.AssignedUserId.Value));
      return new TeamThroughputDto(team.Name, completedCount, Math.Round((double)completedCount / 4, 2)); // Dummy "per Week" (Last 4 weeks)
    }).ToList();

    // 3. Projekt-Gesundheit
    var projectHealth = projects.Select(p =>
    {
      var projectTickets = allTickets.Where(t => t.ProjectId == p.Id).ToList();
      var open = projectTickets.Count(t => t.Status != "Done");
      var urgent = projectTickets.Count(t => t.Priority != null && t.Priority.Name == "Blocker");
      string status = "Healthy";
      if (urgent > 2)
      {
          status = "At Risk";
      }
      else if (open > 10)
      {
          status = "Warning";
      }

      return new ProjectHealthDto(p.Title, open, urgent, status);
    }).ToList();

    return new StakeholderDashboardDto(
        new Collection<SlaComplianceDto>(slaCompliance),
        new Collection<TeamThroughputDto>(teamThroughput),
        new Collection<ProjectHealthDto>(projectHealth),
        userCount);
  }
}
