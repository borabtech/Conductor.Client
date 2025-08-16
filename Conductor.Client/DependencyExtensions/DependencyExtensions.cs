using Conductor.Client.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Conductor.Client.DependencyExtensions;

public static class DependencyExtensions
{
    public static void AddConductor(this IHostApplicationBuilder builder)
    {
        builder.Services.Configure<ConductorServerConfiguration>(
            builder.Configuration.GetSection("Conductor"));
        builder.Services.AddScoped(typeof(IConductorClient), typeof(ConductorClient));
    }
}