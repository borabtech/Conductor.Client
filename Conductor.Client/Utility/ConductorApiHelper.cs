using Conductor.Api;
using Conductor.Client.Models;
using conductor_csharp.Api;

namespace Conductor.Client.Utility;

internal class ConductorApiHelper 
{
    internal static IWorkflowResourceApi GetConductor(ConductorServerConfiguration config)
    {
        var conductorConfiguration = new Configuration
        {
            BasePath = $"{config.Url}:{config.Port}"
        };
        var workflowClient = conductorConfiguration.GetClient<WorkflowResourceApi>();
        return workflowClient;
    }
}