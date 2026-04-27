// <copyright file="ICorporateSkinProvider.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Interfaces;

/// <summary>
/// Definiert die Schnittstelle fÃ¼r den Corporate Skin Provider.
/// ErmÃ¶glicht das Abrufen von branding-spezifischen Informationen wie Farben und Logos.
/// </summary>
public interface ICorporateSkinProvider
{
  /// <summary>
  /// Ruft die PrimÃ¤rfarbe fÃ¼r das Branding ab (hexadezimal oder CSS-Variable).
  /// </summary>
  /// <returns>Die PrimÃ¤rfarbe als String.</returns>
  public string GetPrimaryColor();

  /// <summary>
  /// Ruft die SekundÃ¤rfarbe fÃ¼r das Branding ab.
  /// </summary>
  /// <returns>Die SekundÃ¤rfarbe als String.</returns>
  public string GetSecondaryColor();

  /// <summary>
  /// Ruft den Namen oder Pfad des Firmenlogos ab.
  /// </summary>
  /// <returns>Den Dateinamen des Firmenlogos.</returns>
  public string GetLogoName();
}
