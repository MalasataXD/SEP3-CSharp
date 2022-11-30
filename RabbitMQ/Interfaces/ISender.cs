using Domain.DTOs.JavaDTOs;
using Domain.Models;

namespace RabbitMQ.Interfaces;

public interface ISender
{
    //Test methods TODO: Delete later
    void Test(MessageHeader messageHeader);
    
    //Worker methods
    void CreateWorker(Worker toSend);

    //Shift methods
    void CreateShift(ShiftJavaDto toSend);
    void EditShift(ShiftJavaDto toSend);
    void RemoveShift(int shiftId);
}