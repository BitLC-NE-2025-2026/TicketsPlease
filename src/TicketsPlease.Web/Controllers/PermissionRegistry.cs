// <copyright file="PermissionRegistry.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Web.Controllers;

using System.Collections.Generic;

/// <summary>
/// Zentrales Register aller verfÃƒÂ¼gbaren Berechtigungen.
/// Berechtigungen werden als Claims vom Typ "Permission" in IdentityRoleClaims gespeichert.
/// </summary>
public static class PermissionRegistry
{
  // --- Page Access Permissions ---
  public const string PageDashboard = "Pages.Dashboard";
  public const string PageProjects = "Pages.Projects";
  public const string PageTickets = "Pages.Tickets";
  public const string PageTeams = "Pages.Teams";
  public const string PageMessages = "Pages.Messages";
  public const string PageSocial = "Pages.Social";
  public const string PageInsights = "Pages.Insights";
  public const string PageAdmin = "Pages.Admin";
  public const string PageAdminUsers = "Pages.Admin.Users";
  public const string PageAdminRoles = "Pages.Admin.Roles";
  public const string PageAdminTemplates = "Pages.Admin.Templates";
  public const string PageAdminSettings = "Pages.Admin.Settings";
  public const string PageAdminWorkspaces = "Pages.Admin.Workspaces";

  // --- Ticket Permissions ---
  public const string TicketsCreate = "Tickets.Create";
  public const string TicketsEdit = "Tickets.Edit";
  public const string TicketsDelete = "Tickets.Delete";
  public const string TicketsAssign = "Tickets.Assign";
  public const string TicketsChangeStatus = "Tickets.ChangeStatus";
  public const string TicketsExport = "Tickets.Export";

  // --- Project Permissions ---
  public const string ProjectsCreate = "Projects.Create";
  public const string ProjectsEdit = "Projects.Edit";
  public const string ProjectsDelete = "Projects.Delete";

  // --- Team Permissions ---
  public const string TeamsCreate = "Teams.Create";
  public const string TeamsEdit = "Teams.Edit";
  public const string TeamsDelete = "Teams.Delete";
  public const string TeamsManageMembers = "Teams.ManageMembers";

  // --- User Permissions ---
  public const string UsersEdit = "Users.Edit";
  public const string UsersDeactivate = "Users.Deactivate";
  public const string UsersDelete = "Users.Delete";

  // --- Reporting Permissions ---
  public const string ReportsView = "Reports.View";
  public const string ReportsExport = "Reports.Export";

  /// <summary>
  /// Gets alle verfÃƒÂ¼gbaren Berechtigungen, gruppiert nach Modul.
  /// </summary>
  public static Dictionary<string, List<PermissionDefinition>> AllPermissions => new()
  {
    ["Pages"] = new List<PermissionDefinition>
    {
      new(PageDashboard, "Dashboard", "Access to the main dashboard"),
      new(PageProjects, "Projects", "Access to project pages"),
      new(PageTickets, "Tickets", "Access to ticket pages"),
      new(PageTeams, "Teams", "Access to team pages"),
      new(PageMessages, "Messages", "Access to messaging"),
      new(PageSocial, "Social Feed", "Access to the social feed"),
      new(PageInsights, "Insights / Reports", "Access to stakeholder insights"),
      new(PageAdmin, "Admin Panel", "Access to the administration panel"),
      new(PageAdminUsers, "User Management", "Access to user management"),
      new(PageAdminRoles, "Role Management", "Access to role management"),
      new(PageAdminTemplates, "Template Management", "Access to ticket templates"),
      new(PageAdminSettings, "System Settings", "Access to system settings"),
      new(PageAdminWorkspaces, "Workspace Management", "Access to workspace management"),
    },
    ["Tickets"] = new List<PermissionDefinition>
    {
      new(TicketsCreate, "Create Tickets", "Create new tickets"),
      new(TicketsEdit, "Edit Tickets", "Modify existing tickets"),
      new(TicketsDelete, "Delete Tickets", "Soft-delete tickets"),
      new(TicketsAssign, "Assign Tickets", "Assign tickets to users"),
      new(TicketsChangeStatus, "Change Status", "Move tickets through workflow"),
      new(TicketsExport, "Export Tickets", "Export ticket data"),
    },
    ["Projects"] = new List<PermissionDefinition>
    {
      new(ProjectsCreate, "Create Projects", "Create new projects"),
      new(ProjectsEdit, "Edit Projects", "Modify project settings"),
      new(ProjectsDelete, "Delete Projects", "Delete projects"),
    },
    ["Teams"] = new List<PermissionDefinition>
    {
      new(TeamsCreate, "Create Teams", "Create new teams"),
      new(TeamsEdit, "Edit Teams", "Modify team settings"),
      new(TeamsDelete, "Delete Teams", "Delete teams"),
      new(TeamsManageMembers, "Manage Members", "Add/remove team members"),
    },
    ["Users"] = new List<PermissionDefinition>
    {
      new(UsersEdit, "Edit Users", "Edit user profiles"),
      new(UsersDeactivate, "Deactivate Users", "Toggle user active status"),
      new(UsersDelete, "Delete Users", "Soft-delete users"),
    },
    ["Reports"] = new List<PermissionDefinition>
    {
      new(ReportsView, "View Reports", "View reporting dashboards"),
      new(ReportsExport, "Export Reports", "Export report data"),
    },
  };
}



