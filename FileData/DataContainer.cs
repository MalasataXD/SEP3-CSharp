using Domain.Models;

namespace FileData;

public class DataContainer
{
    // # Fields
    public ICollection<Worker> Workers { get; set; }
    public ICollection<WorkShift> Shifts { get; set; }
}