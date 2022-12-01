using System.Security.AccessControl;
using System.Text;
using System.Text.Json;
using Domain.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQ;

public class Receiver 
{
    private string HostName;
    private string Id;
    private static string QueueName;


    //Oof
    private static Dictionary<string, object> map;
    
    public Receiver()
    {
        MQConfig mqConfig = MQConfig.GetInstance;
        map = new Dictionary<string, object>();
        
        QueueName = mqConfig.QueueName;
        HostName = mqConfig.HostName;
        
        Thread thread = new Thread(Run);
        thread.Start();
    }
    
    public async Task<object> Receive(string key)
    {
        object oldValue;
        map.TryGetValue(key, out oldValue);

        for (int i = 0; i < 100; i++)
        {
            Thread.Sleep(100);
            object newValue;
            map.TryGetValue(key, out newValue);
            if (oldValue != newValue)
            {
                return newValue;
            }
        }
        throw new Exception("Didn't get new value form server");
    }
    
    private static void Run()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using(var connection = factory.CreateConnection())
        using(var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: QueueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                MessageHeader? messageHeader = JsonSerializer.Deserialize<MessageHeader>(message);
                Console.WriteLine(" [x] Received {0}", messageHeader);
                
                Console.WriteLine(messageHeader.payload + " Class: " + messageHeader.payload.GetType());
                
                //virker nok ikke
                map.Add(messageHeader.action, messageHeader.payload);
            };
            channel.BasicConsume(queue: QueueName,
                autoAck: true,
                consumer: consumer);

            while (true)
            {
                Thread.Sleep(1000);
            }
        }
    }
}


