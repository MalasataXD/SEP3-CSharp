namespace Domain.DTOs.WorkShift;

public class WorkShiftBasicDto
{
    // # Fields
    public int ShiftId {get; set;}
    public string Date {get; set; } // ? Which date? (dd/MM/yyyy)
    
    public string FromTime {get; set; } // ? From time (ex. 16:00)
    public string ToTime { get; set; } // ? To time (ex. 20:00)
    public string WorkerFullName {get; set; } // ? Who works the shift?
    public string BreakAmount{get; set; } // ? How much break the worker has ex. 15 min
    
    // ¤ Constructor
    public WorkShiftBasicDto(string date, string fromTime,string toTime, string workerFullName, string breakAmount)
    {
        Date = date;
        FromTime = fromTime;
        ToTime = toTime;
        WorkerFullName = workerFullName;
        BreakAmount = breakAmount;
    }
}