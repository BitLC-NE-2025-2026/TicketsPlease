// <copyright file="StyleguideController.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Web.Controllers;

using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Controller fÃ¼r den UI-Styleguide.
/// Dient als Referenz fÃ¼r Entwickler, um einheitliche UI-Komponenten zu verwenden.
/// </summary>
internal sealed class StyleguideController : Controller
{
  /// <summary>
  /// Zeigt die Ãœbersicht aller UI-Komponenten und Design-Tokens an.
  /// </summary>
  /// <returns>Die Styleguide-View.</returns>
  [HttpGet]
  public IActionResult Index()
  {
    return this.View();
  }
}
