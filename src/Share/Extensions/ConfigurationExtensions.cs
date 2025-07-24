using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Share.Extensions;

public static class ConfigurationExtensions
{
    public static T ConfigureOptions<T>(
        this IServiceCollection services,
        IConfiguration configuration)
        where T : class
        => services.ConfigureOptions<T>(typeof(T).Name, configuration);

    public static T ConfigureOptions<T>(
        this IServiceCollection services,
        string optionsName,
        IConfiguration configuration)
        where T : class
    {
        var section = configuration.GetSection(typeof(T).Name);

        if (!section.Exists() || !section.GetChildren().Any())
            throw new InvalidOperationException($"{typeof(T).Name} section is missing or empty");

        var options = section.Get<T>();
        if (options is null)
            throw new InvalidOperationException($"{typeof(T).Name} options not found");

        services.Configure<T>(optionsName, section);

        return options;
    }
}
