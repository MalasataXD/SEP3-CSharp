namespace Domain.DTOs.WorkShift;

public class WorkShiftUpdateDto
{
    // # Fields
    public int ShiftId { get; }
    // NOTE: Date could be removed, if we decide not needed!
    public string? Date {get; set; } // ? Which date? (dd/MM/yyyy)
    public string? FromTime {get; set; } // ? From time (ex. 16:00)
    public string? ToTime { get; set; } // ? To time (ex. 20:00)
    public int? WorkerId {get; set; } // ? Who works the shift?
    public string? BreakAmount{get; set; } // ? How much break the worker has ex. 15 min
    
    // ¤ Constructor
    public WorkShiftUpdateDto(int shiftId)
    {
        ShiftId = shiftId;
    }
    
    
}