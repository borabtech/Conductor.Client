using Conductor.Client.Models;
using Conductor.Client.Models.ConductorTypes;

namespace Conductor.Client;

public interface IConductorDefinitionClient
{
    Task<Result<string>> RegisterWorkflows(string workflowDirectory);
    Task<Result<string>> RegisterWorkflow(string jsonDefinition);
    Task<Result<string>> UpdateWorkflowDefinition(string jsonDefinition);
    Task<Result<IEnumerable<WorkflowDefinition>>> GetWorkflowDefinitions();
    Task<Result<string>> RemoveWorkflowDefinition(string name, int version);
}