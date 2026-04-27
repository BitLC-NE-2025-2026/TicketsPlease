// <copyright file="BaseEntity.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Domain.Common;

using System;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Stellt die Basisklasse fÃ¼r alle DomÃ¤nen-EntitÃ¤ten dar.
/// EnthÃ¤lt grundlegende Eigenschaften wie eine eindeutige ID und ein Feld fÃ¼r die NebenlÃ¤ufigkeitskontrolle.
/// </summary>
public abstract class BaseEntity : IBaseEntity
{
  /// <summary>
  /// Eine Liste von Domain-Events, die von dieser EntitÃ¤t ausgelÃ¶st wurden.
  /// </summary>
  private readonly List<IDomainEvent> domainEvents = new();

  /// <summary>
  /// Gets or sets die eindeutige IdentitÃ¤t der EntitÃ¤t.
  /// </summary>
  [Key]
  public Guid Id { get; set; } = Guid.NewGuid();

  /// <summary>
  /// Gets or sets die Mandanten-ID, zu der diese EntitÃ¤t gehÃ¶rt (Multi-Tenancy).
  /// </summary>
  public Guid TenantId { get; set; }

  /// <summary>
  /// Gets or sets a value indicating whether die EntitÃ¤t gelÃ¶scht wurde (Soft-Delete).
  /// </summary>
  public bool IsDeleted { get; set; }

  /// <summary>
  /// Gets or sets den Zeitpunkt des Soft-Deletes.
  /// </summary>
  public DateTime? DeletedAt { get; set; }

  /// <summary>
  /// Gets or sets das Feld zur optimistischen NebenlÃ¤ufigkeitskontrolle (Concurrency Control).
  /// Dieses Feld wird automatisch von SQL Server aktualisiert (Timestamp/RowVersion).
  /// </summary>
#pragma warning disable CA1819 // Properties should not return arrays
  [Timestamp]
  public byte[] RowVersion { get; set; } = Array.Empty<byte>();
#pragma warning restore CA1819 // Properties should not return arrays

  /// <summary>
  /// Gets die Liste der Domain-Events (Read-Only).
  /// </summary>
  public IReadOnlyCollection<IDomainEvent> DomainEvents => this.domainEvents.AsReadOnly();

  /// <summary>
  /// FÃ¼gt ein Domain-Event hinzu.
  /// </summary>
  /// <param name="domainEvent">Das hinzuzufÃ¼gende Event.</param>
  public void AddDomainEvent(IDomainEvent domainEvent) => this.domainEvents.Add(domainEvent);

  /// <summary>
  /// Entfernt ein Domain-Event.
  /// </summary>
  /// <param name="domainEvent">Das zu entfernende Event.</param>
  public void RemoveDomainEvent(IDomainEvent domainEvent) => this.domainEvents.Remove(domainEvent);

  /// <summary>
  /// Leert die Liste der Domain-Events.
  /// </summary>
  public void ClearDomainEvents() => this.domainEvents.Clear();
}
