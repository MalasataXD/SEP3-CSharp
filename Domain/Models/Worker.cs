namespace Domain.Models;

public class Worker
{
    // # Fields
    public int WorkerId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int PhoneNumber { get; set; }
    public string Mail{ get; set; } 
    public string Address { get; set; }
    
    // TODO: Authentication

    public Worker(string firstName, string lastName, int phoneNumber, string mail, string address)
    {
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        Mail = mail;
        Address = address;
    }

    public Worker()
    {
    }

    public string getFullName()
    {
        return FirstName + " " + LastName;
    }
    
}