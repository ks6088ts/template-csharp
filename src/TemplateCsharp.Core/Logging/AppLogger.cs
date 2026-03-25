using Microsoft.Extensions.Logging;

namespace TemplateCsharp.Core.Logging;

/// <summary>
/// Factory helpers for creating pre-configured <see cref="ILoggerFactory"/> instances.
/// </summary>
public static class AppLogger
{
    /// <summary>
    /// Creates an <see cref="ILoggerFactory"/> using the supplied provider configuration.
    /// </summary>
    /// <param name="verbose">
    /// When <see langword="true"/>, sets the minimum log level to
    /// <see cref="LogLevel.Debug"/>; otherwise <see cref="LogLevel.Information"/>.
    /// </param>
    /// <param name="configureProviders">
    /// A delegate that registers one or more logging providers
    /// (e.g. <c>builder => builder.AddConsole()</c>).
    /// </param>
    /// <returns>A configured <see cref="ILoggerFactory"/>.</returns>
    public static ILoggerFactory Create(bool verbose, Action<ILoggingBuilder> configureProviders) =>
        LoggerFactory.Create(builder =>
        {
            builder.SetMinimumLevel(verbose ? LogLevel.Debug : LogLevel.Information);
            configureProviders(builder);
        });
}