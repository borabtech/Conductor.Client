namespace Conductor.Client;

public interface IConductorClient
{
    Task<(bool Status, string Message)> RegisterWorkflows(string path);
}