﻿using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Share.Extensions;

public static class InterceptorServiceExtensions
{
    private static readonly ProxyGenerator ProxyGenerator = new();

    public static IServiceCollection AddTransient<TInterface, TImplementation, TInterceptor>(
        this IServiceCollection services)
        where TImplementation : class, TInterface
        where TInterface : class
        where TInterceptor : class, IInterceptor
    {
        services.AddTransient<TInterceptor>();
        services.AddTransient<TImplementation>();

        services.AddTransient<TInterface>(provider =>
        {
            var target = provider.GetRequiredService<TImplementation>();
            var interceptor = provider.GetRequiredService<TInterceptor>();
            return ProxyGenerator.CreateInterfaceProxyWithTarget<TInterface>(target, interceptor);
        });

        return services;
    }

    public static IServiceCollection AddScoped<TInterface, TImplementation, TInterceptor>(
        this IServiceCollection services)
        where TImplementation : class, TInterface
        where TInterface : class
        where TInterceptor : class, IInterceptor
    {
        services.AddScoped<TInterceptor>();
        services.AddScoped<TImplementation>();

        services.AddScoped<TInterface>(provider =>
        {
            var target = provider.GetRequiredService<TImplementation>();
            var interceptor = provider.GetRequiredService<TInterceptor>();
            return ProxyGenerator.CreateInterfaceProxyWithTarget<TInterface>(target, interceptor);
        });

        return services;
    }

    public static IServiceCollection AddSingleton<TInterface, TImplementation, TInterceptor>(
        this IServiceCollection services)
        where TImplementation : class, TInterface
        where TInterface : class
        where TInterceptor : class, IInterceptor
    {
        services.AddSingleton<TInterceptor>();
        services.AddSingleton<TImplementation>();

        services.AddSingleton<TInterface>(provider =>
        {
            var target = provider.GetRequiredService<TImplementation>();
            var interceptor = provider.GetRequiredService<TInterceptor>();
            return ProxyGenerator.CreateInterfaceProxyWithTarget<TInterface>(target, interceptor);
        });

        return services;
    }

    public static IServiceCollection AddHostedService<TInterface, TImplementation, TInterceptor>(
        this IServiceCollection services)
        where TInterface : class, IHostedService
        where TImplementation : class, TInterface
        where TInterceptor : class, IInterceptor
    {
        services.AddSingleton<TInterceptor>();
        services.AddSingleton<TImplementation>();

        services.AddSingleton<TInterface>(provider =>
        {
            var target = provider.GetRequiredService<TImplementation>();
            var interceptor = provider.GetRequiredService<TInterceptor>();
            return ProxyGenerator.CreateInterfaceProxyWithTarget<TInterface>(target, interceptor);
        });

        services.AddSingleton<IHostedService>(provider => provider.GetRequiredService<TInterface>());

        return services;
    }
}
