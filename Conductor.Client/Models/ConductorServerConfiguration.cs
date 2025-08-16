namespace Conductor.Client.Models;

public record ConductorServerConfiguration
{
    public string Url { get; set; }
    public int Port { get; set; }
}