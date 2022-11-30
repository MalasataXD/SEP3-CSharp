namespace Domain.DTOs.WorkShift;

public class WorkShiftCreationDto
{
    // # Fields
    public string Date {get;} // ? Which date? (dd/MM/yyyy)
    
    public string FromTime {get;} // ? From time (ex. 16:00)
    public string ToTime { get;} // ? To time (ex. 20:00)
    public int WorkerId { get;} // ? Who?
    public string BreakAmount{get;} // ? How much break the worker has ex. 15 min
    
    public string BossId { get; set; } //Who make the shift

    // Â¤ Constructor
    public WorkShiftCreationDto(string date, string fromTime, string toTime, int workerId, string breakAmount, string bossId)
    {
        Date = date;
        FromTime = fromTime;
        ToTime = toTime;
        WorkerId = workerId;
        BreakAmount = breakAmount;
        BossId = bossId;
    }
}