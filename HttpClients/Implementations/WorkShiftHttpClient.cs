using System.Net.Http.Json;
using System.Text.Json;
using Domain.DTOs.WorkShift;
using Domain.Models;
using HttpClients.Interfaces;

namespace HttpClients.Implementations;

public class WorkShiftHttpClient : IWorkShiftService
{
    
    private readonly HttpClient client;

    public WorkShiftHttpClient(HttpClient client)
    {
        this.client = client;
    }
    public async Task<WorkShift> CreateAsync(WorkShiftCreationDto dto)
    {
        HttpResponseMessage response = await client.PostAsJsonAsync("/workShift", dto);
        string result = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(result);
        }

        WorkShift workShift = JsonSerializer.Deserialize<WorkShift>(result)!;
        return workShift;       
    }

    public async Task<IEnumerable<WorkShift>> GetAsync(string? date = null, string? workerName = null)
    {
        string uri = "";
        
        if (!string.IsNullOrEmpty(date))
        {
            uri = $"/workShift?date={date}";
        }
        if (!string.IsNullOrEmpty(workerName))
        {

            if (string.IsNullOrEmpty(uri))
            {
                uri = $"/workShift?workerName={workerName}";
            }
            else
            {
                uri += $"&workerName={workerName}";
            }
        }
        
        HttpResponseMessage response = await client.GetAsync(uri);
        string result = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(result);
        }

        IEnumerable<WorkShift> workShifts = JsonSerializer.Deserialize<IEnumerable<WorkShift>>(result, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
        return workShifts;
    }
}