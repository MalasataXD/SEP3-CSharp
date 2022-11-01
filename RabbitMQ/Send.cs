using System.Text;
using System.Text.Json;
using Domain.Models;
using RabbitMQ.Client;

namespace RabbitMQ;

public class Sender
{
    public static void SendWorker(Worker toSend)
    {
        ConnectionFactory factory = new ConnectionFactory() { HostName = "localhost" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: "hello", durable: false, exclusive: false, autoDelete: false, arguments: null);
                
            string message = JsonSerializer.Serialize(toSend);
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "", routingKey: "hello", basicProperties: null, body: body);
            Console.WriteLine(" [x] Sent {0}",message);
        }
    }
}