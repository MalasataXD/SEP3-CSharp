using System.Text.Json;
using Shared.Models;

namespace Shared.FileIO;

public class LoginContext
{
    // # Fields
    private const string FilePath = "login.json";
    private LoginDataContainer? _dataContainer;
    
    // ¤ Get Data
    public ICollection<User> Users
    {
        get
        {
            LoadData();
            return _dataContainer!.Users;
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
                Users = new List<User>(),
            };
            return;
        }
        
        // ! Extract content from file
        string content = File.ReadAllText(FilePath);
        _dataContainer = JsonSerializer.Deserialize<LoginDataContainer>(content);


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