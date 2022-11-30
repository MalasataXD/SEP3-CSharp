using Domain.DTOs.JavaDTOs;
using Domain.Models;

namespace RabbitMQ.Interfaces;

public interface ISender
{
    //Worker methods
    void CreateWorker(Worker toSend);

    //Shift methods
    void CreateShift(WorkShift toSend);
    void EditShift(WorkShift toSend);
    void RemoveShift(int shiftId);
}