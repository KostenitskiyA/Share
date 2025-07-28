﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Share.Extensions;

public static class ConfigurationExtensions
{
    public static T ConfigureOptions<T>(
        this IServiceCollection services,
        IConfiguration configuration,
        string? name)
        where T : class
    {
        var section = configuration.GetSection(typeof(T).Name);

        if (!section.Exists() || !section.GetChildren().Any())
            throw new InvalidOperationException($"{typeof(T).Name} section is missing or empty");

        var options = section.Get<T>();
        if (options is null)
            throw new InvalidOperationException($"{typeof(T).Name} options not found");

        if (string.IsNullOrEmpty(name))
            services.Configure<T>(section);
        else
            services.Configure<T>(name, section);

        return options;
    }
}
