using System.Net.Http.Json;
using System.Text;
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
        HttpResponseMessage response = await client.PostAsJsonAsync("/WorkShift", dto);
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
        string uri = "/WorkShift";
        if (!string.IsNullOrEmpty(date))
        {
            uri += $"?date={date}";
        }

        if (!string.IsNullOrEmpty(workerName))
        {
            if (date != null)
            {
                uri += $"&workerName={workerName}";
            }
            else
            {
                uri += $"?workerName={workerName}";
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
    
    public async Task UpdateAsync(WorkShiftUpdateDto dto)
    {
        string dtoAsJson = JsonSerializer.Serialize(dto);
        StringContent body = new StringContent(dtoAsJson, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await client.PatchAsync("/WorkShift",body);
        if (!response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            throw new Exception(content);
        }
    }

    public async Task DeleteAsync(int id)
    {
        HttpResponseMessage response = await client.DeleteAsync($"WorkShift/{id}");
            if (!response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                throw new Exception(content);
            }
    }

    public async Task ValidateAsync(WorkShiftValidateDto dto)
    {
        string dtoAsJson = JsonSerializer.Serialize(dto);
        StringContent body = new StringContent(dtoAsJson, Encoding.UTF8, "application/json");
        
        HttpResponseMessage response = await client.PostAsync("/WorkShift/Validate",body);
        if (!response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            throw new Exception(content);
        }
    }

    public async Task CreateAsync(IEnumerable<WorkShiftCreationDto> dtos)
    {
        HttpResponseMessage response = await client.PostAsJsonAsync("/WorkShifts", dtos);
        string result = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(result);
        }
    }

    public async Task UpdateAsync(IEnumerable<WorkShiftUpdateDto> dtos)
    {
        string dtoAsJson = JsonSerializer.Serialize(dtos);
        StringContent body = new StringContent(dtoAsJson, Encoding.UTF8, "application/json");
        
        HttpResponseMessage response = await client.PatchAsync("/WorkShifts", body);
        if (!response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            throw new Exception(content);
        }
    }

    public async Task DeleteAsync(List<int> ids)
    {
        HttpResponseMessage response = await client.PostAsJsonAsync($"/WorkShifts/Delete",ids);
        if (!response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            throw new Exception(content);
        }    
    }
}