// <copyright file="IBaseEntity.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Domain.Common;

using System;
using System.Collections.Generic;

/// <summary>
/// Definiert die Basiseigenschaften fÃ¼r alle DomÃ¤nen-EntitÃ¤ten.
/// ErmÃ¶glicht eine konsistente Behandlung von EntitÃ¤ten, auch wenn diese
/// von externen Klassen (wie IdentityUser) erben mÃ¼ssen.
/// </summary>
public interface IBaseEntity
{
  /// <summary>
  /// Gets die eindeutige IdentitÃ¤t der EntitÃ¤t.
  /// </summary>
  public Guid Id { get; }

  /// <summary>
  /// Gets die Mandanten-ID, zu der diese EntitÃ¤t gehÃ¶rt.
  /// </summary>
  public Guid TenantId { get; }

  /// <summary>
  /// Gets a value indicating whether die EntitÃ¤t gelÃ¶scht wurde.
  /// </summary>
  public bool IsDeleted { get; }

  /// <summary>
  /// Gets den Zeitpunkt des Soft-Deletes.
  /// </summary>
  public DateTime? DeletedAt { get; }

  /// <summary>
  /// Gets die Version fÃ¼r die NebenlÃ¤ufigkeitskontrolle.
  /// </summary>
#pragma warning disable CA1819 // Properties should not return arrays
  public byte[] RowVersion { get; }
#pragma warning restore CA1819 // Properties should not return arrays

  /// <summary>
  /// Gets die Liste der DomÃ¤nenereignisse.
  /// </summary>
  public IReadOnlyCollection<IDomainEvent> DomainEvents { get; }

  /// <summary>
  /// FÃ¼gt ein DomÃ¤nenereignis hinzu.
  /// </summary>
  /// <param name="domainEvent">Das Ereignis.</param>
  public void AddDomainEvent(IDomainEvent domainEvent);

  /// <summary>
  /// Entfernt ein DomÃ¤nenereignis.
  /// </summary>
  /// <param name="domainEvent">Das Ereignis.</param>
  public void RemoveDomainEvent(IDomainEvent domainEvent);

  /// <summary>
  /// Leert die DomÃ¤nenereignisse.
  /// </summary>
  public void ClearDomainEvents();
}
