namespace Domain.Models;

public class WorkShift
{ 
    // # Fields
    public int ShiftId {get; set;}
    public string Date {get; set; } // ? Which date?
    public string Time {get; set; } // ? When?
    public Worker Worker {get; set; } // ? Who works the shift?
    public string BreakAmount{get; set; } // ? How much break the worker has ex. 15 min
    
    
    // ¤ Constructor
    public WorkShift(string date, string time, Worker worker, string breakAmount)
    {
        Date = date;
        Time = time;
        Worker = worker;
        BreakAmount = breakAmount;
    }
}