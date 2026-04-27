namespace TicketsPlease.UnitTests.Infrastructure.Services;

using FluentAssertions;
using TicketsPlease.Infrastructure.Services;

internal class DefaultCorporateSkinProviderTests
{
  [Fact]
  public void GetColors_ShouldReturnDefaults()
  {
    var provider = new DefaultCorporateSkinProvider();
    provider.GetPrimaryColor().Should().Be("#3b82f6");
    provider.GetSecondaryColor().Should().Be("#1e40af");
    provider.GetLogoName().Should().Be("TicketsPlease");
  }
}
