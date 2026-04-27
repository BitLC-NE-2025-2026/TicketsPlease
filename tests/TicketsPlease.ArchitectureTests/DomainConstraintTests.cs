// <copyright file="DomainConstraintTests.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.ArchitectureTests;

using FluentAssertions;
using NetArchTest.Rules;
using TicketsPlease.Domain.Common;
using TicketsPlease.Domain.Entities;

/// <summary>
/// EnthÃ¤lt Architektur-Tests zur Sicherstellung der DatenintegritÃ¤t und Einhaltung von Domain-Vorgaben.
/// Nutzt NetArchTest zur statischen Analyse der Assembly-Struktur.
/// </summary>
#pragma warning disable CA1707 // Identifiers should not contain underscores
public class DomainConstraintTests
{
  /// <summary>
  /// PrÃ¼ft, ob alle EntitÃ¤ten im Domain-Layer von der Klasse <see cref="BaseEntity"/> erben.
  /// Dies stellt sicher, dass grundlegende Felder wie Id und RowVersion Ã¼berall vorhanden sind.
  /// </summary>
  [Fact]
  public void Entities_Should_Inherit_From_BaseEntity()
  {
    var result = Types.InAssembly(typeof(User).Assembly)
        .That()
        .ResideInNamespace("TicketsPlease.Domain.Entities")
        .And()
        .AreClasses()
        .And()
        .DoNotHaveName("User")
        .And()
        .DoNotHaveName("Role")
        .Should()
        .Inherit(typeof(BaseEntity))
        .GetResult();

    var failureMessage = "alle EntitÃ¤ten mÃ¼ssen von BaseEntity erben. Fehlend: " +
                         (result.FailingTypeNames != null ? string.Join(", ", result.FailingTypeNames) : "keine");
    result.IsSuccessful.Should().BeTrue(failureMessage);
  }

  /// <summary>
  /// PrÃ¼ft, ob alle EntitÃ¤ten im Domain-Namespace liegen.
  /// </summary>
  [Fact]
  public void Entities_Should_Reside_In_Entities_Namespace()
  {
    var result = Types.InAssembly(typeof(User).Assembly)
        .That()
        .Inherit(typeof(BaseEntity))
        .And()
        .AreNotAbstract()
        .Should()
        .ResideInNamespace("TicketsPlease.Domain.Entities")
        .GetResult();

    result.IsSuccessful.Should().BeTrue("EntitÃ¤ten sollten in einem spezifischen Namespace gruppiert sein.");
  }
}
#pragma warning restore CA1707 // Identifiers should not contain underscores
