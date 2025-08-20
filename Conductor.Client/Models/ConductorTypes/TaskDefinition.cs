namespace Conductor.Client.Models.ConductorTypes;

public class TaskDefinition
{
    public string Name { get; set; }
    public string TaskReferenceName { get; set; }
    public Dictionary<string, object> InputParameters { get; set; }
    public string Type { get; set; }
    public Dictionary<string, List<TaskDefinition>> DecisionCases { get; set; }
    public List<TaskDefinition> DefaultCase { get; set; }
    public List<List<TaskDefinition>> ForkTasks { get; set; }
    public int StartDelay { get; set; }
    public List<string> JoinOn { get; set; }
    public bool Optional { get; set; }
    public List<object> DefaultExclusiveJoinTask { get; set; }
    public bool AsyncComplete { get; set; }
    public List<object> LoopOver { get; set; }
    public Dictionary<string, object> OnStateChange { get; set; }
    public bool Permissive { get; set; }

    // Bazı task’lerde özel alanlar var:
    public string Sink { get; set; } // EVENT tipi için
    public string DynamicTaskNameParam { get; set; } // DYNAMIC için
    public string CaseValueParam { get; set; } // DECISION için
    public string DynamicForkTasksParam { get; set; } // FORK_JOIN_DYNAMIC için
    public string DynamicForkTasksInputParamName { get; set; } // FORK_JOIN_DYNAMIC için
    public SubWorkflowParam SubWorkflowParam { get; set; } // SUB_WORKFLOW için
}