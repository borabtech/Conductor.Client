using Conductor.Client.Models;
using Conductor.Client.Models.ConductorResponseTypes;
using Conductor.Client.Models.ConductorTypes;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Conductor.Client;

public class ConductorDefinitionClient : IConductorDefinitionClient
{
    private readonly ILogger<ConductorDefinitionClient> _logger;
    private readonly HttpClient _httpClient;
    public ConductorDefinitionClient(
        ILogger<ConductorDefinitionClient> logger,
        HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
    }

    public async Task<Result<string>> RegisterWorkflows(string path)
    {
        _logger.LogInformation("Workflow registration started for path : {path}", path);
        string[] definitionFiles = Directory.GetFiles(path, "*Workflow.json");
        _logger.LogInformation("{count} definition found", definitionFiles.Length);

        var errorMessagesBuilder = new StringBuilder();
        int wfFailed = 0, wfRegistered = 0;
        foreach (string definitionFile in definitionFiles)
        {
            _logger.LogInformation("Upload starting... {filename}", definitionFile);

            try
            {
                var reader = new StreamReader(definitionFile, Encoding.UTF8);
                string jsonDefinition = await reader.ReadToEndAsync().ConfigureAwait(false);
                reader.Close();

                var options = new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true
                };
                var workflowDefinition = JsonSerializer.Deserialize<WorkflowDefinition>(jsonDefinition, options);
                HttpResponseMessage conductorHttpResponse = await _httpClient
                    .PostAsJsonAsync("api/metadata/workflow", workflowDefinition)
                    .ConfigureAwait(false);

                if (conductorHttpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    var responseAsString = await conductorHttpResponse.Content
                        .ReadAsStringAsync()
                        .ConfigureAwait(false);
                    var conductorResponse = JsonSerializer.Deserialize<CreateWorkflowResponse>(responseAsString, options);

                    wfFailed++;
                    string error = $"{Path.GetFileName(definitionFile)} failed. Error : {conductorResponse?.message}";
                    _logger.LogError(error);
                    errorMessagesBuilder.AppendLine(error);
                }
                else
                {
                    wfRegistered++;
                    _logger.LogInformation("Upload completed... {filename}", definitionFile);
                }
            }
            catch (Exception ex)
            {
                wfFailed++;
                _logger.LogError("Upload failed for {filename}. Error : {ex}", definitionFile, ex);
            }
        }

        string msg = $"Workflow registration completed with {wfRegistered} success {wfFailed} fails for path : {path}. Details : {errorMessagesBuilder.ToString()}";
        _logger.LogInformation(msg);
        return new Result<string>
        {
            Status = true,
            Message = msg
        };
    }

    public async Task<Result<IEnumerable<WorkflowDefinition>>> GetWorkflowDefinitions()
    {
        try
        {
            var conductorHttpResponse = await _httpClient
                .GetAsync("api/metadata/workflow")
                .ConfigureAwait(false);

            var response = await (conductorHttpResponse.Content
                .ReadFromJsonAsync<List<WorkflowDefinition>>())
                .ConfigureAwait(false);

            return new Result<IEnumerable<WorkflowDefinition>>
            {
                Status = true,
                Data = response
            };
        }
        catch(Exception ex)
        {
            _logger.LogError("GetWorkflowDefinitions failed. Error : {ex}", ex);
            return new Result<IEnumerable<WorkflowDefinition>>
            {
                Status = false,
                Message = JsonSerializer.Serialize(ex)
            };
        }
    }

    public Task<Result<string>> RegisterWorkflow(string jsonDefinition)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<string>> RemoveWorkflowDefinition(string name, int version)
    {
        try
        {
            HttpResponseMessage conductorHttpResponse = await _httpClient
                .DeleteAsync($"api/metadata/workflow/{name}/{version}")
                .ConfigureAwait(false);

            if (conductorHttpResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return new Result<string>
                {
                    Status = true
                };
            }

            var responseAsString = await conductorHttpResponse.Content
                .ReadAsStringAsync()
                .ConfigureAwait(false);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var response = JsonSerializer.Deserialize<DeleteWorkflowDefinitionResponse>(responseAsString, options);

            return new Result<string>
            {
                Status = false,
                Message = $"Conductor Status : {response.status} Message: {response.message}"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError("RemoveWorkflowDefinitions failed. Error : {ex}", ex);
            return new Result<string>
            {
                Status = false,
                Message = ex.Message
            };
        }
        return new Result<string>
        {
            Status = true
        };
    }

    public Task<Result<string>> UpdateWorkflowDefinition(string jsonDefinition)
    {
        throw new NotImplementedException();
    }
}