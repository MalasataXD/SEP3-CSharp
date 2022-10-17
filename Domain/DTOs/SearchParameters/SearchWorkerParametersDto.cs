namespace Domain.DTOs.SearchParameters;

public class SearchWorkerParametersDto
{
    // # Fields
    // NOTE: More fields could be added, if needed.
    public string? WorkerName { get; }

    // ¤ Constructor
    public SearchWorkerParametersDto(string? workerName)
    {
        WorkerName = workerName;
    }
    
    
}