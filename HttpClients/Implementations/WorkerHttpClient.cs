using System.Text.Json;
using Domain.DTOs.Worker;
using Domain.Models;
using HttpClients.Interfaces;

namespace HttpClients.Implementations;

public class WorkerHttpClient : IWorkerService
{
    private readonly HttpClient client;

    public WorkerHttpClient(HttpClient client)
    {
        this.client = client;
    }

    public Task<Worker> CreateAsync(WorkerCreationDto dto)
    {
        
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Worker>> GetUsersAsync(string? nameContains = null)
    {
        string uri = "/worker";
        if (!string.IsNullOrEmpty(nameContains))
        {
            uri += $"?workerName={nameContains}";
        }
        HttpResponseMessage response = await client.GetAsync(uri);
        string result = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(result);
        }

        IEnumerable<Worker> workers = JsonSerializer.Deserialize<IEnumerable<Worker>>(result, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
        return workers;
    }
}