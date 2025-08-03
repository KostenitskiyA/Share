using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Share.Extensions;

public static class ConfigurationExtensions
{
    public static IConfigurationSection GetSection<T>(
        this IConfiguration configuration)
        where T : class
    {
        var section = configuration.GetSection(typeof(T).Name);

        if (!section.Exists() || !section.GetChildren().Any())
            throw new InvalidOperationException($"{typeof(T).Name} section is missing or empty");

        return section;
    }

    public static T GetOptions<T>(
        this IConfigurationSection configurationSection)
        where T : class
    {
        var options = configurationSection.Get<T>();

        if (options is null)
            throw new InvalidOperationException($"{typeof(T).Name} options not found");

        return options;
    }

    public static T ConfigureOptions<T>(
        this IServiceCollection services,
        IConfigurationSection section,
        string? name = null)
        where T : class
    {
        var options = section.GetOptions<T>();

        if (string.IsNullOrEmpty(name))
            services.Configure<T>(section);
        else
            services.Configure<T>(name, section);

        return options;
    }

    public static T ConfigureOptions<T>(
        this IServiceCollection services,
        IConfiguration configuration,
        string? name = null)
        where T : class
    {
        var section = configuration.GetSection<T>();
        var options = services.ConfigureOptions<T>(section, name);

        return options;
    }
}
