using System.Net.NetworkInformation;
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
    void GetWorkerByFullName(String fullName);
    void GetWorkerBySearchParameters(SearchWorkerParametersJavaDto dto);
    
    //Shift methods
    void CreateShift(WorkShift toSend);
    void EditShift(WorkShift toSend);
    void RemoveShift(int shiftId);
    void GetShiftById(int shiftId);
    void GetShiftBySearchParameters(SearchShiftParametersJavaDto dto);
    
    void DeleteAsync(List<int> shiftIds); // ¤ Delete
    void CreateAsync(List<WorkShift> shifts); // ¤ Delete
    void UpdateAsync(List<WorkShift> toUpdate); // ¤ Update (Patch)
    
    
    
}