using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AWS.SDK.Samples.Application;
using AWS.SDK.Samples.S3.Services;
using AWS.SDK.Samples.S3.Models;

var host = App.Create((context, services) =>
{
    services.AddSingleton<ICloudStorage, S3Service>();
});

var config = host.Services.GetRequiredService<IConfiguration>();
var s3Service = host.Services.GetRequiredService<ICloudStorage>();

var bucket = new Bucket(config["S3:Bucket:Name"]);

if (await s3Service.Create(bucket))
{
    Console.WriteLine($"'{config["S3:Bucket:Name"]}' bucket created");
}

if (await s3Service.Exists(bucket))
{
    Console.WriteLine($"'{config["S3:Bucket:Name"]}' bucket exists");
}

Console.WriteLine($"'{config["S3:Bucket:Name"]}' bucket location is '{await s3Service.Location(bucket)}'");

if (await s3Service.Delete(bucket))
{
    Console.WriteLine($"'{config["S3:Bucket:Name"]}' bucket deleted");
}

await host.RunAsync();