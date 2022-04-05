using Amazon.S3;
using AWSS3Sample;
using AWSSDKServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using IHost host = Host.CreateDefaultBuilder(args)
    .UseEnvironment("Development")
    .ConfigureServices((context, services) => 
    {
        services.AddAWSServices(context.Configuration);
    })
    .Build();

var config = host.Services.GetRequiredService<IConfiguration>();
var s3Client = host.Services.GetRequiredService<IAmazonS3>();

var bucket = new Bucket(s3Client, config["S3:Bucket:Name"]);

Console.WriteLine($"S3 Bucket '{config["S3:Bucket:Name"]}' exists");

if (await bucket.Create())
{
    Console.WriteLine($"'{config["S3:Bucket:Name"]}' bucket created");
}

if (await bucket.Exists())
{
    Console.WriteLine($"'{config["S3:Bucket:Name"]}' bucket exists");
}

Console.WriteLine($"'{config["S3:Bucket:Name"]}' bucket location is '{await bucket.Location()}'");

if (await bucket.Delete())
{
    Console.WriteLine($"'{config["S3:Bucket:Name"]}' bucket deleted");
}

await host.RunAsync();


