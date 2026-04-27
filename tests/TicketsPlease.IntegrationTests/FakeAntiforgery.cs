// <copyright file="FakeAntiforgery.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.IntegrationTests;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

/// <summary>
/// Fake Antiforgery implementation for tests.
/// </summary>
internal sealed class FakeAntiforgery : Microsoft.AspNetCore.Antiforgery.IAntiforgery
{
  public Microsoft.AspNetCore.Antiforgery.AntiforgeryTokenSet GetAndStoreTokens(HttpContext httpContext) => new("test", "test", "test", "test");

  public Microsoft.AspNetCore.Antiforgery.AntiforgeryTokenSet GetTokens(HttpContext httpContext) => new("test", "test", "test", "test");

  public Task<bool> IsRequestValidAsync(HttpContext httpContext) => Task.FromResult(true);

  public void SetCookieTokenAndHeader(HttpContext httpContext)
  {
  }

  public Task ValidateRequestAsync(HttpContext httpContext) => Task.CompletedTask;
}
