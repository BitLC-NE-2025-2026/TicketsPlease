// <copyright file="StyleguideController.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Web.Controllers;

using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Controller fÃƒÂ¼r den UI-Styleguide.
/// Dient als Referenz fÃƒÂ¼r Entwickler, um einheitliche UI-Komponenten zu verwenden.
/// </summary>
internal class StyleguideController : Controller
{
  /// <summary>
  /// Zeigt die ÃƒÅ“bersicht aller UI-Komponenten und Design-Tokens an.
  /// </summary>
  /// <returns>Die Styleguide-View.</returns>
  [HttpGet]
  public IActionResult Index()
  {
    return this.View();
  }
}
