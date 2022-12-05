using System.Net.Http.Json;
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

    public async Task<Worker> CreateAsync(WorkerCreationDto dto)
    {
        HttpResponseMessage response = await client.PostAsJsonAsync("/Worker", dto);
        string result = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(result);
        }

        Worker worker = JsonSerializer.Deserialize<Worker>(result)!;
        return worker;    
    }

    public async Task<IEnumerable<Worker>> GetAsync(string? nameContains = null)
    {
        string uri = "/Worker";
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

    public async Task DeleteAsync(int id)
    {
        HttpResponseMessage response = await client.DeleteAsync($"Worker/{id}");
        if (!response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            throw new Exception(content);
        }    
    }
}