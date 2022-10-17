namespace Domain.DTOs.Worker;

public class WorkerCreationDto
{
    // # Fields
    public string FirstName { get; }
    public string LastName { get; }
    public string Phone { get; }
    public string Mail { get; }
    public string Address { get; }
    
    // ¤ Constructor
    public WorkerCreationDto(string firstName, string lastName, string phone, string mail, string address)
    {
        FirstName = firstName;
        LastName = lastName;
        Phone = phone;
        Mail = mail;
        Address = address;
    }
}