using Domain.DTOs.JavaDTOs;
using Domain.Models;



namespace RabbitMQ;

public class Test
{
    
    public static void Main()
    {
        Sender sender = new Sender();
        
        sender.Test(new MessageHeader("localClient1", "TEST", new WorkerJavaDto("","",8,"","")));

        Receiver receiver = new Receiver();
        
        Console.WriteLine(" [Test]: " + receiver.ReceiveTest());





        //WorkerJavaDto dto = (WorkerJavaDto) Payload;

        //Console.WriteLine("[Receiver]: " + Payload);

    }

}

