namespace TemplateCsharp.Core.Configuration;

/// <summary>
/// Provides access to application configuration values from environment variables.
/// </summary>
public class AppConfiguration
{
    /// <summary>
    /// Gets the value of an environment variable, returning a default value if not set.
    /// </summary>
    /// <param name="key">The name of the environment variable.</param>
    /// <param name="defaultValue">The value to return when the variable is not set.</param>
    /// <returns>The environment variable value, or <paramref name="defaultValue"/>.</returns>
    public string GetEnvironmentVariable(string key, string defaultValue = "") =>
        Environment.GetEnvironmentVariable(key) ?? defaultValue;
}