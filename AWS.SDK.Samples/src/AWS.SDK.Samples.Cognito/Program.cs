using Amazon.CognitoIdentityProvider;
using AWS.SDK.Cognito;
using AWS.SDK.Samples.Application;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = App.Create();

var config = host.Services.GetRequiredService<IConfiguration>();
var s3Client = host.Services.GetRequiredService<IAmazonCognitoIdentityProvider>();

var userPool = new UserPool(s3Client, config["Cognito:UserPool:Name"]);

if (await userPool.Create())
{
    Console.WriteLine($"'{config["Cognito:UserPool:Name"]}' user pool created");
}

if (await userPool.Delete())
{
    Console.WriteLine($"'{config["Cognito:UserPool:Name"]}' user pool deleted");
}

await host.RunAsync();