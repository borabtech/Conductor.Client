namespace Conductor.Client.Models;

public record ConductorServerConfiguration
{
    public required string Url { get; set; }
    public int Port { get; set; }
}