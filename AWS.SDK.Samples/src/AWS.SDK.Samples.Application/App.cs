using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AWS.SDK.Samples.Application
{
    public class App
    {
        public static IHost Create(Action<HostBuilderContext, IServiceCollection> configureDelegate)
        {
            return Host.CreateDefaultBuilder()
                .UseEnvironment("Development")
                .ConfigureServices((context, services) =>
                {
                    services.AddAWSServices(context.Configuration);

                    configureDelegate(context, services);
                })
                .Build();
        }
    }
}