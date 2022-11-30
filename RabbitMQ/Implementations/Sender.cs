using System.Security.AccessControl;
using System.Text;
using System.Text.Json;
using Domain.DTOs.JavaDTOs;
using Domain.Models;
using RabbitMQ.Client;
using RabbitMQ.Interfaces;

namespace RabbitMQ;

public class Sender : ISender
{
    private string HostName { get; }
    private string DispatcherName { get; }

    public Sender()
    {
        MQConfig mqConfig = MQConfig.GetInstance;

        HostName = mqConfig.HostName;
        DispatcherName = mqConfig.DispatcherName;
    }

    public void Test(MessageHeader messageHeader)
    {
        send("Test", messageHeader);
    }

    public void CreateWorker(Worker toSend)
    {
        //send("CrateWorker", new MessageHeader("test", new WorkerJavaDto(toSend)));
    }

    public void CreateShift(ShiftJavaDto toSend)
    {
        //send("CrateShift", toSend);
    }
    
    public void EditShift(ShiftJavaDto toSend)
    {
        //send("EditShift", toSend);
    }

    public void RemoveShift(int shiftId)
    {
        //send("RemoveShift", shiftId);
    }

    private void send(string Queue, MessageHeader toSend)
    {
        ConnectionFactory factory = new ConnectionFactory() { HostName = this.HostName };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: Queue, durable: false, exclusive: false, autoDelete: false, arguments: null);
            string message = JsonSerializer.Serialize(toSend);

            Console.WriteLine("toSend: " + message);
            
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "", routingKey: Queue, basicProperties: null, body: body);

            // Dispatcher queue
            channel.QueueDeclare(queue: DispatcherName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            string dispatcherMessage = Queue;
            var dispatcherBody = Encoding.UTF8.GetBytes(dispatcherMessage);

            channel.BasicPublish(exchange: "", routingKey: DispatcherName, basicProperties: null, body: dispatcherBody);
            Console.WriteLine(" Dispatcher Sent {0}",dispatcherMessage);
        }
    }
}