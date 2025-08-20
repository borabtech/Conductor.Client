namespace Conductor.Client.Models;

internal record ConductorHttpResponse
{
    public int status { get; set; }
    public string message { get; set; }
    public string instance { get; set; }
    public bool retryable { get; set; }
}