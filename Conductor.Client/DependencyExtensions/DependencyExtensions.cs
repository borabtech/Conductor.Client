using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Conductor.Client.DependencyExtensions;

public static class DependencyExtensions
{
    public static void AddConductor(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped(typeof(IConductorDefinitionClient), typeof(ConductorDefinitionClient));
        builder.Services.AddHttpClient<IConductorDefinitionClient, ConductorDefinitionClient>(client =>
        {
            var conductorApiConfig = builder.Configuration.GetSection("ConductorServer");
            var baseUrl = conductorApiConfig.GetValue<string>("Url");
            var port = conductorApiConfig.GetValue<int>("Port");

            client.BaseAddress = new Uri($"{baseUrl}:{port}/");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });
    }
}