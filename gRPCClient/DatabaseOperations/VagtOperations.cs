using Grpc.Core;
using Grpc.Net.Client;
using gRPCService;

namespace gRPCClient.DatabaseOperations;

public class VagtOperations
{
    //Attributes
    private GrpcChannel channel;
    
    //Constructor
    public VagtOperations(string url)
    {
        //A new channel is created with the url given in the constructor
        channel = GrpcChannel.ForAddress(url);
    }
    
    public void FjernVagt(string vagtID)
    {
        //A specific request object is made, depending on the desired usage
        var input = new FjernVagtRequest()
        {
            VagtId = vagtID
        };

        //Create new client based on the grpc service with the channel object, the channel subscribes to the grpc service
        var client = new DatabaseOperationService.DatabaseOperationServiceClient(channel);

        //A client operation is made and a reply is captured using the APIresponse
        var reply = client.fjernVagt(input);

        //WriteLine command for diagnostics
        Console.WriteLine($"Message: {reply.ResponseMessage}" +
                          $" code: {reply.ResponseCode}");

        //ReadLine command to prevent the client connection to server from closing until user input
        Console.ReadLine();
    }

    public void OpretVagt(string date, string fromHour, string fromMinute, string toHour, string toMinute, string workerId, string breakAmount, string bossId)
    {
        var input = new OpretVagtRequest()
        {
            Date = date,
            FromHour = fromHour,
            FromMinute = fromHour,
            ToHour = toHour,
            ToMinute = toMinute,
            WorkerId = workerId,
            BreakAmount = breakAmount,
            BossId = bossId
        };

        var client = new DatabaseOperationService.DatabaseOperationServiceClient(channel);

        var reply = client.opretVagt(input);
        
        Console.WriteLine($"Message: {reply.ResponseMessage}" +
                          $" code: {reply.ResponseCode}");
        
        //ReadLine command to prevent the client connection to server from closing until user input
        Console.ReadLine();
    }

    public void RedigerVagt(string vagtId, string date, string fromHour, string fromMinute, string toHour, string toMinute, string workerId, string breakAmount, string bossId)
    {
        var input = new RedigerVagtRequest()
        {
            VagtId = vagtId,
            Date = date,
            FromHour = fromHour,
            FromMinute = fromMinute,
            ToHour = toHour,
            ToMinute = toMinute,
            WorkerId = workerId,
            BreakAmount = breakAmount,
            BossId = bossId
        };

        var client = new DatabaseOperationService.DatabaseOperationServiceClient(channel);

        var reply = client.redigerVagt(input);
        
        Console.WriteLine($"Message: {reply.ResponseMessage}" +
                           $" code: {reply.ResponseCode}");
        
        //ReadLine command to prevent the client connection to server from closing until user input
        Console.ReadLine();
    }
}