using System.Security.AccessControl;
using System.Text.Json;
using Domain.DTOs.JavaDTOs;

namespace RabbitMQ;

public class Test
{
    public static async Task Main(string[] args)
    {
        Receiver receiver = new Receiver();
        Sender sender = new Sender();
        
        sender.GetWorkerById(1);
        
        
        object o = await receiver.Receive("GetWorkerById");

        
        
        
        WorkerJavaDto? workerJavaDto = JsonSerializer.Deserialize<WorkerJavaDto>((JsonElement)o);

        Console.WriteLine(workerJavaDto.workerId);




}
}