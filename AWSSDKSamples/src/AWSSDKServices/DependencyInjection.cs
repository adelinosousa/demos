using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.S3;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AWSSDKServices
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAWSServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDefaultAWSOptions(new AWSOptions
            {
                Region = RegionEndpoint.GetBySystemName(configuration["AWS:Region"]),
                Credentials = new BasicAWSCredentials(configuration["AWS:AccessKey"], configuration["AWS:SecretKey"])
            });

            services.AddAWSService<IAmazonS3>();

            return services;
        }
    }
}