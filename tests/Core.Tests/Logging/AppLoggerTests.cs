using TemplateCsharp.Core.Logging;

namespace TemplateCsharp.Core.Tests.Logging;

public class AppLoggerTests
{
    [Fact]
    public void Create_ReturnsNonNullLoggerFactory()
    {
        using var factory = AppLogger.Create(false, _ => { });
        Assert.NotNull(factory);
    }

    [Fact]
    public void Create_VerboseFalse_ReturnsLoggerFactory()
    {
        using var factory = AppLogger.Create(false, _ => { });
        var logger = factory.CreateLogger("Test");
        Assert.NotNull(logger);
    }

    [Fact]
    public void Create_VerboseTrue_ReturnsLoggerFactory()
    {
        using var factory = AppLogger.Create(true, _ => { });
        var logger = factory.CreateLogger("Test");
        Assert.NotNull(logger);
    }

    [Fact]
    public void Create_InvokesConfigureProviders()
    {
        var providerConfigured = false;
        using var factory = AppLogger.Create(false, _ => providerConfigured = true);
        Assert.True(providerConfigured);
    }
}