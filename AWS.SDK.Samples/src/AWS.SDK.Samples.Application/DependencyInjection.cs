using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.SimpleEmail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AWS.SDK.Samples.Application
{
    internal static class DependencyInjection
    {
        public static IServiceCollection AddAWSServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDefaultAWSOptions(new AWSOptions
            {
                Region = RegionEndpoint.GetBySystemName(configuration["AWS:Region"]),
                Credentials = new BasicAWSCredentials(configuration["AWS:AccessKey"], configuration["AWS:SecretKey"])
            });

            services.AddAWSService<IAmazonS3>();
            services.AddAWSService<IAmazonCognitoIdentityProvider>();
            services.AddAWSService<IAmazonSimpleEmailService>();

            return services;
        }
    }
}