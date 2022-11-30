namespace RabbitMQ;

public class MQConfig
{
    //Singleton & lock
    private static MQConfig instance = null;
    private static readonly object padlock = new object();
    
    //get obj
    public static MQConfig GetInstance
    {
        get
        {
            if (instance == null)
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new MQConfig();
                    }
                }
            }
            return instance;
        }
    }

    public string HostName { get; }
    public string DispatcherName { get; }
    
    public string QueueName { get; }
    
    private MQConfig()
    {
        QueueName = "localClient1";
        HostName = "localhost";
        DispatcherName = "Dispatcher";
    }
}