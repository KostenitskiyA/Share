using System.Diagnostics;
using Castle.DynamicProxy;
using Microsoft.Extensions.Options;
using Share.Options;

namespace Share.Interceptors;

public class TracingInterceptor(IOptions<OpenTelemetryConfiguration> options) : IInterceptor
{
    private readonly ActivitySource ActivitySource = new(options.Value.ServiceName);

    public void Intercept(IInvocation invocation)
    {
        using var activity = ActivitySource.StartActivity(invocation.Method.Name);
        invocation.Proceed();
    }
}
