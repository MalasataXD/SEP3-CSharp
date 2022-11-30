namespace Domain.DTOs.Worker;

public class WorkerUpdateDto
{
    // # Fields
    public int WorkerId {get; }
    public string? FirstName {get; set; } 
    public string? LastName {get; set; }
    public int? PhoneNumber { get; set; } 
    public string? Mail{get; set; } 
    public string? Adress{get; set; }
    
    // ¤ Constructor
    public WorkerUpdateDto(int workerId, string? firstName, string? lastName, int? phoneNumber, string? mail, string? adress)
    {
        WorkerId = workerId;
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        Mail = mail;
        Adress = adress;
    }
}