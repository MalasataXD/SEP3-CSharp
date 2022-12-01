using Domain.DTOs.JavaDTOs;
using Domain.Models;

namespace RabbitMQ.Interfaces;

public interface ISender
{
    //Worker methods
    void CreateWorker(Worker toSend);
    void EditWorker(Worker toSend);
    void RemoveWorker(int workerId);
    void GetWorkerById(int workerId);
    
    //Shift methods
    void CreateShift(WorkShift toSend);
    void EditShift(WorkShift toSend);
    void RemoveShift(int shiftId);
    void GetShiftById(int shiftId);
    
}