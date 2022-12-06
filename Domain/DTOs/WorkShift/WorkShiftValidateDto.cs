namespace Domain.DTOs.WorkShift;

public class WorkShiftValidateDto
{
    // # Fields
    public string Date {get;}
    public string FromTime {get;} 
    public string ToTime { get;}
    public int WorkerId { get;} 
    public string BreakAmount{get;}
    public IEnumerable<Models.WorkShift> WorkShifts { get; set; }
    
    // Â¤ Constructor
    public WorkShiftValidateDto(string date, string fromTime, string toTime, int workerId, string breakAmount, IEnumerable<Models.WorkShift> workShifts)
    {
        Date = date;
        FromTime = fromTime;
        ToTime = toTime;
        WorkerId = workerId;
        BreakAmount = breakAmount;
        WorkShifts = workShifts;
    }
}