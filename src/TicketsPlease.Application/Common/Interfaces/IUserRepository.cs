// <copyright file="IUserRepository.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Interfaces;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicketsPlease.Domain.Entities;

/// <summary>
/// Interface fÃ¼r erweiterte Benutzer-Datenbankoperationen.
/// </summary>
public interface IUserRepository
{
  /// <summary>
  /// Ruft einen Benutzer mit seinem Profil, seiner Rolle und seiner Organisation (Tenant) ab.
  /// </summary>
  /// <param name="userId">Die ID des Benutzers.</param>
  /// <returns>Der Benutzer mit geladenen Details oder null.</returns>
  public Task<User?> GetUserWithDetailsAsync(Guid userId);

  /// <summary>
  /// Ruft die Teams ab, in denen der Benutzer Mitglied ist.
  /// </summary>
  /// <param name="userId">Die ID des Benutzers.</param>
  /// <returns>Eine Liste von Teams.</returns>
  public Task<List<Team>> GetUserTeamsAsync(Guid userId);

  /// <summary>
  /// Ruft das Profil eines Benutzers ab oder erstellt es, falls es nicht existiert.
  /// </summary>
  /// <param name="userId">Die ID des Benutzers.</param>
  /// <returns>Das UserProfile.</returns>
  public Task<UserProfile> GetOrCreateProfileAsync(Guid userId);

  /// <summary>
  /// Speichert ProfilÃ¤nderungen.
  /// </summary>
  /// <param name="profile">Das zu speichernde Profil.</param>
  /// <returns>Ein Task.</returns>
  public Task UpdateProfileAsync(UserProfile profile);

  /// <summary>
  /// ZÃ¤hlt die aktiven Benutzer eines Tenants.
  /// </summary>
  /// <param name="tenantId">Die Mandanten-ID.</param>
  /// <returns>Die Anzahl.</returns>
  public Task<int> GetActiveUserCountAsync(Guid tenantId);
}
