namespace Domain.DTOs.JavaDTOs;

public class WorkerJavaDto
{
    public int workerId { get; }
    public string firstName { get; }
    public string lastName { get; }
    public int phoneNumber { get; }
    public string mail { get; }
    public string address { get; }

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