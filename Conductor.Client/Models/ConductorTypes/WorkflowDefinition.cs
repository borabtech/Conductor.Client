namespace Conductor.Client.Models.ConductorTypes;

public class WorkflowDefinition
{
    public long CreateTime { get; set; }
    public long UpdateTime { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Version { get; set; }
    public List<TaskDefinition> Tasks { get; set; }
    public List<object> InputParameters { get; set; }
    public Dictionary<string, object> OutputParameters { get; set; }
    public int SchemaVersion { get; set; }
    public bool Restartable { get; set; }
    public bool WorkflowStatusListenerEnabled { get; set; }
    public string OwnerEmail { get; set; }
    public string TimeoutPolicy { get; set; }
    public int TimeoutSeconds { get; set; }
    public Dictionary<string, object> Variables { get; set; }
    public Dictionary<string, object> InputTemplate { get; set; }
    public bool EnforceSchema { get; set; }
    public Dictionary<string, object> Metadata { get; set; }
    public List<object> MaskedFields { get; set; }
}