// <copyright file="PermissionRequirement.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Web.Services;

using Microsoft.AspNetCore.Authorization;

/// <summary>
/// Authorization-Requirement für eine bestimmte Berechtigung.
/// </summary>
internal class PermissionRequirement : IAuthorizationRequirement
{
  /// <summary>
  /// Gets die erforderliche Berechtigung.
  /// </summary>
  public string Permission { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="PermissionRequirement"/> class.
  /// </summary>
  /// <param name="permission">Die erforderliche Berechtigung.</param>
  public PermissionRequirement(string permission)
  {
    this.Permission = permission;
  }
}
