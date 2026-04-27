// <copyright file="TestWebApplicationFactory.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.IntegrationTests;

using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TicketsPlease.Infrastructure.Persistence;

/// <summary>
/// Custom WebApplicationFactory with SQLite in-memory for integration tests.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1515:Consider making public types internal", Justification = "Required for test infrastructure")]
public sealed class TestWebApplicationFactory : WebApplicationFactory<Program>
{
  private readonly SqliteConnection connection;

  /// <summary>
  /// Initializes a new instance of the <see cref="TestWebApplicationFactory"/> class.
  /// </summary>
  public TestWebApplicationFactory()
  {
    this.connection = new SqliteConnection("DataSource=:memory:");
    this.connection.Open();

    using var command = this.connection.CreateCommand();
    command.CommandText = "PRAGMA foreign_keys = ON;";
    command.ExecuteNonQuery();
  }

  /// <inheritdoc/>
  protected override void ConfigureWebHost(IWebHostBuilder builder)
  {
    ArgumentNullException.ThrowIfNull(builder);

    builder.UseEnvironment("Testing");
    builder.ConfigureServices(services =>
    {
      // 1. Remove background services to prevent crashes during startup
      var hostedServices = services.Where(d => d.ServiceType == typeof(IHostedService)).ToList();
      foreach (var d in hostedServices)
      {
        services.Remove(d);
      }

      // 2. Remove existing EF Core registrations
      var efServices = services.Where(d =>
          d.ServiceType.Namespace?.StartsWith("Microsoft.EntityFrameworkCore", StringComparison.Ordinal) == true ||
          d.ServiceType.FullName?.Contains("EntityFrameworkCore", StringComparison.Ordinal) == true).ToList();
      foreach (var d in efServices)
      {
        services.Remove(d);
      }

      var contextDescriptors = services.Where(d =>
          d.ServiceType == typeof(AppDbContext) ||
          d.ServiceType == typeof(DbContextOptions<AppDbContext>)).ToList();
      foreach (var d in contextDescriptors)
      {
        services.Remove(d);
      }

      // 3. Inject SQLite in-memory
      services.AddDbContext<AppDbContext>(options =>
      {
        options.UseSqlite(this.connection);
        options.EnableServiceProviderCaching(false);
      });

      // 4. Fake Authentication & Antiforgery
      services.AddAuthentication(TestAuthHandler.AuthenticationScheme)
              .AddScheme<Microsoft.AspNetCore.Authentication.AuthenticationSchemeOptions, TestAuthHandler>(
                  TestAuthHandler.AuthenticationScheme, _ => { });

      services.AddSingleton<Microsoft.AspNetCore.Antiforgery.IAntiforgery, FakeAntiforgery>();
    });
  }

  /// <inheritdoc/>
  protected override void Dispose(bool disposing)
  {
    base.Dispose(disposing);
    if (disposing)
    {
      this.connection.Close();
      this.connection.Dispose();
    }
  }
}
