namespace Domain.DTOs.JavaDTOs;

public class WorkerJavaDto
{
    public int workerId{ set; get; }
    public string firstName{ set; get; }
    public string lastName{ set; get; }
    public int phoneNumber{ set; get; }
    public string mail{ set; get;}
    public string address { set; get;}

    public WorkerJavaDto(int workerId, string firstName, string lastName, int phoneNumber, string mail, string address)
    {
        this.workerId = workerId;
        this.firstName = firstName;
        this.lastName = lastName;
        this.phoneNumber = phoneNumber;
        this.mail = mail;
        this.address = address;
    }
    
    public WorkerJavaDto(Models.Worker worker)
    {
        this.workerId = worker.WorkerId;
        this.firstName = worker.FirstName;
        this.lastName = worker.LastName;
        this.phoneNumber = worker.PhoneNumber;
        this.mail = worker.Mail;
        this.address = worker.Address;
    }

    public WorkerJavaDto()
    {
    }
}