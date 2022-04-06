using AWS.SDK.Samples.Application;
using AWS.SDK.Samples.Cognito.Models;
using AWS.SDK.Samples.Cognito.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = App.Create((context, services) =>
{
    services.AddSingleton<IIdentityProvider, CognitoService>();
});

var config = host.Services.GetRequiredService<IConfiguration>();
var cognito = host.Services.GetRequiredService<IIdentityProvider>();

var userPool = new UserPool(config["Cognito:UserPool:Name"]);

if (await cognito.Create(userPool))
{
    Console.WriteLine($"'{config["Cognito:UserPool:Name"]}' user pool created");
}

if (await cognito.Delete(userPool))
{
    Console.WriteLine($"'{config["Cognito:UserPool:Name"]}' user pool deleted");
}

await host.RunAsync();