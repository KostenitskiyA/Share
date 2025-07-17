using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Share.Extensions;

public static class ConfigurationExtensions
{
    public static T ConfigureOptions<T>(this IServiceCollection services, IConfiguration configuration) where T : class
    {
        var sectionName = typeof(T).Name;
        var section = configuration.GetSection(sectionName);

        if (!section.GetChildren().Any())
            throw new Exception($"{sectionName} section is missing or empty");

        var options = section.Get<T>();

        if (options is null)
            throw new Exception($"{sectionName} options not found");

        services.Configure<T>(section);

        return options;
    }
}