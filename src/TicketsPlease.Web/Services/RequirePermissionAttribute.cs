// <copyright file="RequirePermissionAttribute.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Web.Services;

using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

/// <summary>
/// Sperrt eine Seite oder Funktion für Benutzer ohne die angegebene Permission.
/// Leitet zur AccessDenied-Seite mit kontextbezogener Fehlermeldung weiter.
/// Admins werden automatisch durchgelassen.
/// </summary>
/// <remarks>
/// Verwendung:
///   [RequirePermission(PermissionRegistry.PageTickets)]
///   public IActionResult Index() { ... }
///
///   [RequirePermission(PermissionRegistry.TicketsDelete, "You need ticket deletion permission.")]
///   public IActionResult Delete() { ... }.
/// </remarks>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
internal sealed class RequirePermissionAttribute : Attribute, IAuthorizationFilter
{
  /// <summary>
  /// Gets die erforderliche Berechtigung.
  /// </summary>
  public string Permission { get; }

  /// <summary>
  /// Gets die benutzerdefinierte Fehlermeldung.
  /// </summary>
  public string? ErrorMessage { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="RequirePermissionAttribute"/> class.
  /// </summary>
  /// <param name="permission">Die erforderliche Berechtigung (z.B. PermissionRegistry.PageTickets).</param>
  /// <param name="errorMessage">Optionale Fehlermeldung.</param>
  public RequirePermissionAttribute(string permission, string? errorMessage = null)
  {
    this.Permission = permission;
    this.ErrorMessage = errorMessage;
  }

  /// <inheritdoc/>
  public void OnAuthorization(AuthorizationFilterContext context)
  {
    ArgumentNullException.ThrowIfNull(context);

    var user = context.HttpContext.User;

    // Nicht authentifizierte Benutzer zur Login-Seite
    if (user.Identity?.IsAuthenticated != true)
    {
      context.Result = new ChallengeResult();
      return;
    }

    // Admins haben implizit alle Berechtigungen
    if (user.IsInRole("Admin"))
    {
      return;
    }

    // Prüfe ob der Benutzer die Permission besitzt
    if (!user.HasClaim("Permission", this.Permission))
    {
      var returnUrl = context.HttpContext.Request.Path + context.HttpContext.Request.QueryString;
      var accessDeniedUrl = $"/Account/AccessDenied?ReturnUrl={Uri.EscapeDataString(returnUrl)}&permission={Uri.EscapeDataString(this.Permission)}";

      context.Result = new RedirectResult(accessDeniedUrl);
    }
  }
}
