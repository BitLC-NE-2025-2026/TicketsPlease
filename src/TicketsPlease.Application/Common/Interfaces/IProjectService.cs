// <copyright file="IProjectService.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Interfaces;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicketsPlease.Application.Common.Dtos;

/// <summary>
/// Definiert die GeschÃ¤ftslogik fÃ¼r die Projektverwaltung.
/// </summary>
public interface IProjectService
{
  /// <summary>
  /// Ruft alle Projekte des aktuellen Mandanten ab.
  /// </summary>
  /// <returns>Eine Liste von <see cref="ProjectDto"/>.</returns>
  public Task<IEnumerable<ProjectDto>> GetProjectsAsync();

  /// <summary>
  /// Ruft ein spezifisches Projekt ab.
  /// </summary>
  /// <param name="id">Die ID des Projekts.</param>
  /// <returns>Ein <see cref="ProjectDto"/> oder null, wenn nicht gefunden.</returns>
  public Task<ProjectDto?> GetProjectAsync(Guid id);

  /// <summary>
  /// Erstellt ein neues Projekt fÃ¼r den aktuellen Mandanten.
  /// </summary>
  /// <param name="dto">Die Projektdaten.</param>
  /// <returns>Ein Task fÃ¼r die asynchrone Operation.</returns>
  public Task CreateProjectAsync(CreateProjectDto dto);

  /// <summary>
  /// Aktualisiert ein bestehendes Projekt.
  /// </summary>
  /// <param name="dto">Die aktualisierten Projektdaten.</param>
  /// <returns>Ein Task fÃ¼r die asynchrone Operation.</returns>
  public Task UpdateProjectAsync(UpdateProjectDto dto);

  /// <summary>
  /// LÃ¶scht ein Projekt.
  /// </summary>
  /// <param name="id">Die ID des zu lÃ¶schenden Projekts.</param>
  /// <returns>Ein Task fÃ¼r die asynchrone Operation.</returns>
  public Task DeleteProjectAsync(Guid id);
}
