using Conductor.Client.Models;
using Conductor.Definition;
using Conductor.Executor;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace Conductor.Client;

public class ConductorClient : IConductorClient
{
    private readonly ILogger<ConductorClient> _logger;
    private readonly IOptions<ConductorServerConfiguration> _conductorConfig;
    public ConductorClient(
        ILogger<ConductorClient> logger,
        IOptions<ConductorServerConfiguration> conductorConfig)
    {
        _logger = logger;
        _conductorConfig = conductorConfig;
    }
    public async Task<(bool Status, string Message)> RegisterWorkflows(string path)
    {
        _logger.LogInformation("Workflow registration started for path : {path}", path);

        //var conductorServer = ConductorApiHelper.GetConductor(_conductorConfig.Value);
        var workflowExecutor = new WorkflowExecutor(new Configuration());

        string[] definitionFiles = Directory.GetFiles(path, "*Workflow.json");
        _logger.LogInformation("{count} definition found", definitionFiles.Length);

        int wfFailed = 0, wfRegistered = 0;
        foreach (string definitionFile in definitionFiles)
        {
            _logger.LogInformation("Upload starting... {filename}", definitionFile);

            try
            {
                var reader = new StreamReader(definitionFile, Encoding.UTF8);
                string jsonDefinition = await reader.ReadToEndAsync().ConfigureAwait(false);

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                options.Converters.Clear();
                ConductorWorkflow workflowDefinition = JsonSerializer.Deserialize<ConductorWorkflow>(jsonDefinition, options);

                workflowExecutor.RegisterWorkflow(workflowDefinition, true);
                wfRegistered++;
            }
            catch (Exception ex)
            {
                wfFailed++;
                _logger.LogError("Upload failed for {filename}. Error : {ex}", definitionFile, ex);
            }
            _logger.LogInformation("Upload completed... {filename}", definitionFile);
        }

        _logger.LogInformation("Workflow registration completed with {ok} success {fail} fails for path : {path}", wfRegistered, wfFailed, path);
        return (true, "OK");
    }
}