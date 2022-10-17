namespace Domain.Models;

public class Worker
{
    // # Fields
    public int WorkerId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Phone { get; set; }
    public string Mail{ get; set; } 
    public string Address { get; set; }
    
    // TODO: Authentication

    public Worker(string firstName, string lastName, string phone, string mail, string address)
    {
        FirstName = firstName;
        LastName = lastName;
        Phone = phone;
        Mail = mail;
        Address = address;
    }


    public string getFullName()
    {
        return FirstName + " " + LastName;
    }
    
}