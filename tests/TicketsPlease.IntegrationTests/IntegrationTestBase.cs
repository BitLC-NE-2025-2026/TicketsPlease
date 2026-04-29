// <copyright file="IntegrationTestBase.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.IntegrationTests;

using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TicketsPlease.Domain.Entities;
using TicketsPlease.Infrastructure.Persistence;

/// <summary>
/// Base class for all integration tests.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1515:Consider making public types internal", Justification = "Required as base class for IntegrationTests")]
public abstract class IntegrationTestBase : IDisposable
{
  /// <summary>
  /// The fixed Tenant ID used for all basic integration tests.
  /// </summary>
  public static readonly Guid TestTenantId = Guid.Parse("00000000-0000-0000-0000-000000001000");

  /// <summary>
  /// The fixed User ID used for basic integration tests.
  /// </summary>
  public static readonly Guid TestUserId = Guid.Parse("00000000-0000-0000-0000-000000002000");

  /// <summary>
  /// Standard Priority ID seeded in all tests.
  /// </summary>
  public static readonly Guid MediumPriorityId = Guid.Parse("00000000-0000-0000-0000-000000000002");

  /// <summary>
  /// Standard To-Do State ID seeded in all tests.
  /// </summary>
  public static readonly Guid TodoStateId = Guid.Parse("00000000-0000-0000-0000-000000000003");

  /// <summary>
  /// Standard Done State ID seeded in all tests.
  /// </summary>
  public static readonly Guid DoneStateId = Guid.Parse("00000000-0000-0000-0000-000000000004");

  private bool disposedValue;

  /// <summary>
  /// Initializes a new instance of the <see cref="IntegrationTestBase"/> class.
  /// </summary>
  protected IntegrationTestBase()
  {
    this.Factory = new TestWebApplicationFactory();

    // Initialize database
    using var scope = this.Factory.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
  }

  /// <summary>
  /// Gets the WebApplicationFactory for the system under test.
  /// </summary>
  protected TestWebApplicationFactory Factory { get; }

  /// <inheritdoc/>
  public void Dispose()
  {
    this.Dispose(disposing: true);
    GC.SuppressFinalize(this);
  }

  /// <summary>
  /// Seeds minimal required data for tests.
  /// </summary>
  /// <param name="db">The database context.</param>
  /// <returns>A task.</returns>
  protected static async Task SeedMinimalAsync(AppDbContext db)
  {
    ArgumentNullException.ThrowIfNull(db);

    if (!await db.Projects.IgnoreQueryFilters().AnyAsync().ConfigureAwait(false))
    {
      await db.Organizations.AddAsync(new Organization { Id = TestTenantId, Name = "Test Org", TenantId = TestTenantId }).ConfigureAwait(false);

      var workflow = new Workflow { Id = Guid.NewGuid(), Name = "Standard Workflow", TenantId = TestTenantId };
      await db.Workflows.AddAsync(workflow).ConfigureAwait(false);

      var project = new Project("Test Projekt", DateTime.UtcNow);
      project.AssignWorkflow(workflow.Id);
      project.SetTenantId(TestTenantId);
      await db.Projects.AddAsync(project).ConfigureAwait(false);

      var role = new Role { Id = Guid.Parse("00000000-0000-0000-0000-000000000001"), Name = "Admin" };
      await db.Roles.AddAsync(role).ConfigureAwait(false);

      var doneStateId = DoneStateId;
      var todoStateId = TodoStateId;

      await db.TicketPriorities.AddAsync(new TicketPriority { Id = MediumPriorityId, Name = "Medium", TenantId = TestTenantId }).ConfigureAwait(false);
      await db.WorkflowStates.AddAsync(new WorkflowState { Id = todoStateId, Name = "Todo", WorkflowId = workflow.Id, TenantId = TestTenantId }).ConfigureAwait(false);
      await db.WorkflowStates.AddAsync(new WorkflowState { Id = doneStateId, Name = "Done", WorkflowId = workflow.Id, TenantId = TestTenantId, IsTerminalState = true }).ConfigureAwait(false);

      await db.WorkflowTransitions.AddAsync(new WorkflowTransition
      {
        Id = Guid.NewGuid(),
        FromStateId = todoStateId,
        ToStateId = doneStateId,
        TenantId = TestTenantId,
      }).ConfigureAwait(false);

      await db.Users.AddAsync(new User
      {
        Id = TestUserId,
        UserName = "testadmin",
        Email = "admin@test.com",
        TenantId = TestTenantId,
        RoleId = role.Id,
        NormalizedEmail = "ADMIN@TEST.COM",
        NormalizedUserName = "TESTADMIN",
        EmailConfirmed = true,
        SecurityStamp = Guid.NewGuid().ToString(),
        Profile = new UserProfile { UserId = TestUserId, FirstName = "Test", LastName = "Admin", TenantId = TestTenantId },
      }).ConfigureAwait(false);

      await db.SaveChangesAsync().ConfigureAwait(false);
    }
  }

  /// <summary>
  /// Sets a mock HttpContext with the specified user and tenant information in the given service provider.
  /// This bypasses global query filters and provides identity for services.
  /// </summary>
  /// <param name="services">The service provider (usually from a scope).</param>
  /// <param name="userId">The user ID.</param>
  /// <param name="tenantId">The tenant ID.</param>
  /// <param name="role">The user role.</param>
  protected void SetContext(IServiceProvider services, Guid userId, Guid tenantId, string role = "Admin")
  {
    ArgumentNullException.ThrowIfNull(services);
    var httpContextAccessor = services.GetRequiredService<IHttpContextAccessor>();

    var claims = new[]
    {
      new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
      new Claim(ClaimTypes.Role, role),
      new Claim("TenantId", tenantId.ToString()),
    };

    var identity = new ClaimsIdentity(claims, "Test");
    var principal = new ClaimsPrincipal(identity);

    httpContextAccessor.HttpContext = new DefaultHttpContext
    {
      User = principal,
    };
  }

  /// <summary>
  /// Disposes resources.
  /// </summary>
  /// <param name="disposing">True if disposing.</param>
  protected virtual void Dispose(bool disposing)
  {
    if (!this.disposedValue)
    {
      if (disposing)
      {
        this.Factory.Dispose();
      }

      this.disposedValue = true;
    }
  }
}
