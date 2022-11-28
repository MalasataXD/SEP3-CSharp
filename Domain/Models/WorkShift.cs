namespace Domain.Models;

public class WorkShift
{ 
    // # Fields
    public int ShiftId {get; set;}
    public string Date {get; set; } // ? Which date? (dd/MM/yyyy)
    public string FromTime {get; set; } // ? From time (ex. 16:00)
    public string ToTime { get; set; } // ? To time (ex. 20:00)
    public Worker Worker {get; set; } // ? Who works the shift?
    public string BreakAmount{get; set; } // ? How much break the worker has ex. 15 min
    
    public string BossId { get; set; } //Who make the shift
    
    // ¤ Constructor
    public WorkShift(string date, string fromTime,string toTime, Worker worker, string breakAmount, string bossId)
    {
        Date = date;
        FromTime = fromTime;
        ToTime = toTime;
        Worker = worker;
        BreakAmount = breakAmount;
        BossId = bossId;
    }

    public WorkShift(string dateToUse, string fromTimeToUse, string toTimeToUse, Worker workerToUse, string breakAmountToUse)
    {
        Date = dateToUse;
        FromTime = fromTimeToUse;
        ToTime = toTimeToUse;
        Worker = workerToUse;
        BreakAmount = breakAmountToUse;
    }
}