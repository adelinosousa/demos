using Amazon.S3;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AWS.SDK.Samples.S3;
using AWS.SDK.Samples.Application;

var host = App.Create();

var config = host.Services.GetRequiredService<IConfiguration>();
var s3Client = host.Services.GetRequiredService<IAmazonS3>();

var bucket = new Bucket(s3Client, config["S3:Bucket:Name"]);

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