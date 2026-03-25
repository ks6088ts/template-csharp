using TemplateCsharp.Core.Configuration;

namespace TemplateCsharp.Core.Tests.Configuration;

public class AppConfigurationTests
{
    [Fact]
    public void GetEnvironmentVariable_ReturnsDefaultWhenNotSet()
    {
        var result = AppConfiguration.GetEnvironmentVariable("NONEXISTENT_VAR_12345", "default");
        Assert.Equal("default", result);
    }

    [Fact]
    public void GetEnvironmentVariable_ReturnsEmptyStringDefaultWhenNotSet()
    {
        var result = AppConfiguration.GetEnvironmentVariable("NONEXISTENT_VAR_12345");
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void GetEnvironmentVariable_ReturnsSetValue()
    {
        const string key = "TEST_VAR_APPCONFIGURATION";
        const string expected = "test-value";
        Environment.SetEnvironmentVariable(key, expected);
        try
        {
            var result = AppConfiguration.GetEnvironmentVariable(key);
            Assert.Equal(expected, result);
        }
        finally
        {
            Environment.SetEnvironmentVariable(key, null);
        }
    }
}