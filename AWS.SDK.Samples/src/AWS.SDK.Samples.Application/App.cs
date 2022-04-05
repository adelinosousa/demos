using Microsoft.Extensions.Hosting;

namespace AWS.SDK.Samples.Application
{
    public class App
    {
        public static IHost Create()
        {
            return Host.CreateDefaultBuilder()
                .UseEnvironment("Development")
                .ConfigureServices((context, services) =>
                {
                    services.AddAWSServices(context.Configuration);
                })
                .Build();
        }
    }
}