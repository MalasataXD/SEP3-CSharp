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
    private string QueueName { get; }

    public Sender()
    {
        MQConfig mqConfig = MQConfig.GetInstance;

        HostName = mqConfig.HostName;
        DispatcherName = mqConfig.DispatcherName;
        QueueName = mqConfig.QueueName;
    }
    
    //Worker methods
    public void CreateWorker(Worker toSend)
    {
        FormatAndSend("CreateWorker", new WorkerJavaDto(toSend));
    }

    public void EditWorker(Worker toSend)
    {
        FormatAndSend("EditWorker", new WorkerJavaDto(toSend));
    }

    public void RemoveWorker(int workerId)
    {
        FormatAndSend("RemoveWorker", workerId);
    }

    public void GetWorkerById(int workerId)
    {
        FormatAndSend("GetWorkerById", workerId);
    }

    //Shift methods
    public void CreateShift(WorkShift toSend)
    {
        FormatAndSend("CreateShift", new ShiftJavaDto(toSend));
    }
    
    public void EditShift(WorkShift toSend)
    {
        FormatAndSend("EditShift", new ShiftJavaDto(toSend));
    }

    public void RemoveShift(int shiftId)
    {
        FormatAndSend("RemoveShift", shiftId);
    }

    public void GetShiftById(int shiftId)
    {
        FormatAndSend("GetShiftById", shiftId);
    }

    private void FormatAndSend(string Queue, object payload)
    {
        send(Queue, new MessageHeader(QueueName, Queue, payload));
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