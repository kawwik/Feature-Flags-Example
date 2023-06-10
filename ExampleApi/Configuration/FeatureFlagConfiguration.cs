using System.Reflection;
using Microsoft.Extensions.FileProviders;
using Microsoft.FeatureManagement;

namespace WebApplication2.Configuration;

public static class FeatureFlagConfiguration
{
    public static void ConfigureFeatureFlags(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddFeatureManagement();

        var configFolderPath = GetAbsolutePath(relativePath: "configs");
        var fileProvider = new PhysicalFileProvider(configFolderPath)
        {
            UsePollingFileWatcher = true,
            UseActivePolling = true
        };
        
        configuration.AddJsonFile(
            fileProvider,
            "feature-flags.json",
            optional: false,
            reloadOnChange: true);
    }

    private static string GetAbsolutePath(string relativePath)
    {
        var executableLocation = Assembly.GetEntryAssembly()!.Location;
        var executablePath = Path.GetDirectoryName(executableLocation)!;
        return Path.Combine(executablePath, relativePath);
    }
}