namespace RabbitMQ;

public class MessageHeader
{
    public string queue { get; }
    public string action { get; }
    public object payload { get; }
    public MessageHeader(string queue, string action, object payload)
    {
        this.queue = queue;
        this.payload = payload;
        this.action = action;
    }
}