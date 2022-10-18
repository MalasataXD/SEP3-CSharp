using System.Text.Json;
using Domain.Models;

namespace FileData;

public class FileContext
{
    // # Fields
    private const string FilePath = "temp.json";
    private DataContainer? _dataContainer;
    
    // ¤ Get Data
    public ICollection<Worker> Workers
    {
        get
        {
            LoadData();
            return _dataContainer!.Workers;
        }
    }
    public ICollection<WorkShift> Shifts
    {
        get
        {
            LoadData();
            return _dataContainer!.Shifts;
        }
    }
    
    // ¤ Extract Data from file (Temp)
    private void LoadData()
    {
        // * Check if data already has been load
        if (_dataContainer != null)
        {
            return;
        }
        
        // * Check if there is a file at the Filepath, then create a new DataContainer
        if (!File.Exists(FilePath))
        {
            _dataContainer = new()
            {
                Workers = new List<Worker>(),
                Shifts = new List<WorkShift>()
            };
            return;
        }
        
        // ! Extract content from file
        string content = File.ReadAllText(FilePath);
        _dataContainer = JsonSerializer.Deserialize<DataContainer>(content);


    }
    
    // ¤ Save data to file (Temp)
    public void SaveChanges()
    {
        string serialized = JsonSerializer.Serialize(_dataContainer,new JsonSerializerOptions
        {
            WriteIndented = true
        });
        File.WriteAllText(FilePath,serialized);
        _dataContainer = null; // ! Reset data container!
    }


}