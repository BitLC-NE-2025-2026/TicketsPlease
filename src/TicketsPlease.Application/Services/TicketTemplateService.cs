// <copyright file="TicketTemplateService.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Services;

using TicketsPlease.Application.Common.Dtos;
using TicketsPlease.Application.Common.Interfaces;
using TicketsPlease.Domain.Entities;

/// <summary>
/// Implementiert die GeschÃ¤ftslogik fÃ¼r Ticket-Vorlagen.
/// </summary>
/// <param name="repository">Das injizierte Repository.</param>
public class TicketTemplateService(ITicketTemplateRepository repository) : ITicketTemplateService
{
  /// <inheritdoc/>
  public async Task<List<TicketTemplateDto>> GetAllTemplatesAsync(CancellationToken ct = default)
  {
    var templates = await repository.GetAllAsync(ct).ConfigureAwait(false);
    return templates.Select(t => new TicketTemplateDto(
        t.Id,
        t.Name,
        t.DescriptionMarkdownTemplate,
        t.DefaultPriorityId,
        t.DefaultPriority?.Name)).ToList();
  }

  /// <inheritdoc/>
  public async Task<TicketTemplateDto> CreateTemplateAsync(Guid creatorId, CreateTicketTemplateDto dto, CancellationToken ct = default)
  {
    ArgumentNullException.ThrowIfNull(dto);

    var template = new TicketTemplate
    {
      Id = Guid.NewGuid(),
      Name = dto.Name,
      DescriptionMarkdownTemplate = dto.DescriptionMarkdownTemplate,
      DefaultPriorityId = dto.DefaultPriorityId,
      CreatorId = creatorId,
    };

    await repository.AddAsync(template, ct).ConfigureAwait(false);
    await repository.SaveChangesAsync(ct).ConfigureAwait(false);

    // Fetch again to get priority name if any
    var saved = await repository.GetByIdAsync(template.Id, ct).ConfigureAwait(false);

    return new TicketTemplateDto(
        saved!.Id,
        saved.Name,
        saved.DescriptionMarkdownTemplate,
        saved.DefaultPriorityId,
        saved.DefaultPriority?.Name);
  }

  /// <inheritdoc/>
  public async Task DeleteTemplateAsync(Guid id, CancellationToken ct = default)
  {
    var template = await repository.GetByIdAsync(id, ct).ConfigureAwait(false);
    if (template != null)
    {
      await repository.DeleteAsync(template, ct).ConfigureAwait(false);
      await repository.SaveChangesAsync(ct).ConfigureAwait(false);
    }
  }
}
