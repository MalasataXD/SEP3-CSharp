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


    public string getFullName()
    {
        return FirstName + " " + LastName;
    }
    
}