namespace Domain.DTOs.Worker;

public class WorkerCreationDto
{
    // # Fields
    public string FirstName { get; }
    public string LastName { get; }
    public int PhoneNumber { get; }
    public string Mail { get; }
    public string Address { get; }
    
    // ¤ Constructor
    public WorkerCreationDto(string firstName, string lastName, int phoneNumber, string mail, string address)
    {
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        Mail = mail;
        Address = address;
    }

    public void String()
    {
        string.Equals(FirstName,LastName);
    }
}