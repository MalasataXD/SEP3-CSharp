using Domain.DTOs.SearchParameters;

namespace Domain.DTOs.JavaDTOs;

public class SearchWorkerParametersJavaDto
{
    public string? workerName { get; set; }

    // ¤ Constructor
    public SearchWorkerParametersJavaDto(string? workerName)
    {
        this.workerName = workerName;
    }

    public SearchWorkerParametersJavaDto(SearchWorkerParametersDto dto)
    {
        workerName = dto.WorkerName;
    }

    public SearchWorkerParametersJavaDto()
    {
    }
}