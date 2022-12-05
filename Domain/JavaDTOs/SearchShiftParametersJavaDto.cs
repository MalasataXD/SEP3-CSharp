using Domain.DTOs.SearchParameters;

namespace Domain.DTOs.JavaDTOs;

public class SearchShiftParametersJavaDto
{
    // # Fields
    // NOTE: Could add FromTime and ToTime, if needed...
    public string? date { get; set; }
    public string? workerName { get; set; }
    
    // ¤ Constructor
    public SearchShiftParametersJavaDto(string? date, string? workerName)
    {
        this.date = date;
        this.workerName = workerName;
    }

    public SearchShiftParametersJavaDto(SearchShiftParametersDto dto)
    {
        this.date = dto.Date;
        this.workerName = dto.WorkerName;
    }


    public SearchShiftParametersJavaDto()
    {
    }
}