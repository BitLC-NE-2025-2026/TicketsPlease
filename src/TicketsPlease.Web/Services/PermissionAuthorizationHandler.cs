// <copyright file="PermissionAuthorizationHandler.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Web.Services;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

/// <summary>
/// Handler, der prÃ¼ft ob der aktuelle Benutzer die angeforderte Permission besitzt.
/// Admins erhalten automatisch alle Berechtigungen.
/// </summary>
internal class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
  /// <inheritdoc/>
  protected override Task HandleRequirementAsync(
      AuthorizationHandlerContext context,
      PermissionRequirement requirement)
  {
    ArgumentNullException.ThrowIfNull(context);
    ArgumentNullException.ThrowIfNull(requirement);

    // Admin-Rolle hat implizit alle Berechtigungen
    if (context.User.IsInRole("Admin"))
    {
      context.Succeed(requirement);
      return Task.CompletedTask;
    }

    // PrÃ¼fe ob der User die Permission als Claim besitzt
    if (context.User.HasClaim("Permission", requirement.Permission))
    {
      context.Succeed(requirement);
    }

    return Task.CompletedTask;
  }
}
