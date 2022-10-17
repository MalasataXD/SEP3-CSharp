namespace Domain.DTOs.SearchParameters;

public class SearchShiftParametersDto
{
    // # Fields
    // NOTE: Could add FromTime and ToTime, if needed...
    public string? Date { get; }
    public string? WorkerName { get; }
    
    // ¤ Constructor
    public SearchShiftParametersDto(string? date, string? workerName)
    {
        Date = date;
        WorkerName = workerName;
    }
    
    
}