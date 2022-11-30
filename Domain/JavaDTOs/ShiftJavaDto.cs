namespace Domain.DTOs.JavaDTOs;

public class ShiftJavaDto
{
    private int shiftId;
    public String date;
    public int fromHour;
    public int fromMinute;
    public int toHour;
    public int toMinute;
    public int workerId;
    public int breakAmount;
    public int bossId;

    public ShiftJavaDto(int shiftId, string date, int fromHour, int fromMinute, int toHour, int toMinute, int workerId, int breakAmount, int bossId)
    {
        this.shiftId = shiftId;
        this.date = date;
        this.fromHour = fromHour;
        this.fromMinute = fromMinute;
        this.toHour = toHour;
        this.toMinute = toMinute;
        this.workerId = workerId;
        this.breakAmount = breakAmount;
        this.bossId = bossId;
    }

    public ShiftJavaDto(Models.WorkShift workShift)
    {
        this.shiftId = workShift.ShiftId;
        this.date = workShift.Date;
        
        string[] FromTime = workShift.FromTime.Split(":");
        string[] ToTime = workShift.ToTime.Split(":");

        this.fromHour = Convert.ToInt32(FromTime[0]);
        this.fromMinute = Convert.ToInt32(FromTime[1]);
        
        this.toHour = Convert.ToInt32(ToTime[0]);
        this.toMinute = Convert.ToInt32(ToTime[1]);
        
        this.workerId = workShift.Worker.WorkerId;
        this.breakAmount = Convert.ToInt32(workShift.BreakAmount);
    }
}

