using System.Text;
using System.Text.Json;
using Domain.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQ;

public class Receiver
{
    public static void ReceiveWorker()
    {
        ConnectionFactory factory = new ConnectionFactory() { HostName = "localhost" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: "hello", durable: false, exclusive: false, autoDelete: false, arguments: null);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [y] Received '{0}'", message);

                Worker? worker = JsonSerializer.Deserialize<Worker>(message);
                Console.WriteLine(worker.getFullName());
            };
            channel.BasicConsume(queue: "hello", autoAck: true, consumer: consumer);
        }
    }
}