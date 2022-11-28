using System.Text;
using System.Text.Json;
using Domain.DTOs.JavaDTOs;
using Domain.Models;
using RabbitMQ.Client;

namespace RabbitMQ;

public class Sender
{
    private string HostName { get; }
    private string DispatcherName { get; }
    
    //Den her skal hedde CreateWorker
    public void SendWorker(Worker toSend)
    {
        send("CrateWorker", new WorkerJavaDto(toSend));
    }
    
    public void CreateShift(ShiftJavaDto toSend)
    {
        send("CrateShift", toSend);
    }
    
    public void EditShift(ShiftJavaDto toSend)
    {
        send("EditShift", toSend);
    }

    public void RemoveShift(int shiftId)
    {
        send("RemoveShift", shiftId);
    }

    private void send(string Queue, object toSend)
    {
        ConnectionFactory factory = new ConnectionFactory() { HostName = this.HostName };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: Queue, durable: false, exclusive: false, autoDelete: false, arguments: null);

            string message = JsonSerializer.Serialize(toSend);
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "", routingKey: Queue, basicProperties: null, body: body);
            Console.WriteLine(" [x] Sent {0}",message);

            // Dispatcher queue
            channel.QueueDeclare(queue: DispatcherName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            string dispatcherMessage = Queue;
            var dispatcherBody = Encoding.UTF8.GetBytes(dispatcherMessage);

            channel.BasicPublish(exchange: "", routingKey: DispatcherName, basicProperties: null, body: dispatcherBody);
            Console.WriteLine(" Dispatcher Sent {0}",dispatcherMessage);
        }
    }
}