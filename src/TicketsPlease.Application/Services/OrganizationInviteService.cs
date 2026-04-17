// <copyright file="OrganizationInviteService.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Services;

using System;
using System.Linq;
using System.Threading.Tasks;
using TicketsPlease.Application.Common.Dtos;
using TicketsPlease.Application.Common.Interfaces;
using TicketsPlease.Domain.Entities;

/// <summary>
/// Implementierung des Dienstes zur Verwaltung von Organisationseinladungen.
/// </summary>
/// <param name="repository">Das Organisations-Repository.</param>
public class OrganizationInviteService(IOrganizationRepository repository) : IOrganizationInviteService
{
  /// <inheritdoc />
  public async Task<OrganizationInviteDto> CreateInviteAsync(Guid organizationId, string? targetedEmail = null, int expiryDays = 7)
  {
    var org = await repository.GetByIdAsync(organizationId).ConfigureAwait(false);
    if (org == null)
    {
      throw new ArgumentException("Organisation nicht gefunden.", nameof(organizationId));
    }

    var invite = new OrganizationInvite
    {
      Token = Guid.NewGuid(),
      OrganizationId = organizationId,
      TargetedEmail = targetedEmail,
      ExpiresAt = DateTime.UtcNow.AddDays(expiryDays),
      IsUsed = false,
    };

    await repository.AddInviteAsync(invite).ConfigureAwait(false);
    await repository.SaveChangesAsync().ConfigureAwait(false);

    return new OrganizationInviteDto(invite.Token, invite.OrganizationId, org.Name, invite.ExpiresAt, invite.TargetedEmail);
  }

  /// <inheritdoc />
  public async Task<OrganizationInviteDto?> ValidateTokenAsync(Guid token)
  {
    var invite = await repository.GetInviteByTokenAsync(token).ConfigureAwait(false);

    if (invite == null || invite.Organization == null)
    {
      return null;
    }

    return new OrganizationInviteDto(invite.Token, invite.OrganizationId, invite.Organization.Name, invite.ExpiresAt, invite.TargetedEmail);
  }

  /// <inheritdoc />
  public async Task MarkAsUsedAsync(Guid token, Guid userId)
  {
    var invite = await repository.GetInviteByTokenAsync(token).ConfigureAwait(false);

    if (invite != null)
    {
      invite.IsUsed = true;
      invite.UsedByUserId = userId;
      await repository.SaveChangesAsync().ConfigureAwait(false);
    }
  }
}
