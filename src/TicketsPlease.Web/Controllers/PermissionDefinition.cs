// <copyright file="PermissionDefinition.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Web.Controllers;

/// <summary>
/// Definition einer einzelnen Berechtigung.
/// </summary>
/// <param name="Key">Der eindeutige Schlüssel (Claim-Wert).</param>
/// <param name="DisplayName">Der Anzeigename.</param>
/// <param name="Description">Eine Beschreibung der Berechtigung.</param>
internal record PermissionDefinition(string Key, string DisplayName, string Description);
