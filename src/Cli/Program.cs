using Microsoft.Extensions.Logging;

using TemplateCsharp.Core.Configuration;
using TemplateCsharp.Core.Logging;

var verbose = args.Contains("--verbose") || args.Contains("-v");

using var loggerFactory = AppLogger.Create(verbose, builder => builder.AddConsole());
var logger = loggerFactory.CreateLogger("Cli");
var config = new AppConfiguration();

logger.LogInformation("Hello, World!");

if (verbose)
{
    logger.LogDebug("Verbose mode enabled");
    logger.LogDebug(
        "Environment: {Environment}",
        config.GetEnvironmentVariable("ENVIRONMENT", "development")
    );
}