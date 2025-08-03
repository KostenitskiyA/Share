using System.Diagnostics;
using Castle.DynamicProxy;
using Microsoft.Extensions.Options;
using Share.Options;

namespace Share.Interceptors;

public class TracingInterceptor(IOptionsMonitor<OpenTelemetryConfiguration> options) : IInterceptor
{
    private readonly ActivitySource ActivitySource = new(options.CurrentValue.ServiceName);

    public void Intercept(IInvocation invocation)
    {
        using var activity = ActivitySource.StartActivity(invocation.Method.Name);

        try {
            invocation.Proceed();
        }
        catch (Exception ex) {
            activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
            activity?.AddException(ex);
            throw;
        }
    }
}
