using System.Text.Json;
using TaskTracker.Cli.Models;
namespace TaskTracker.Cli.Services;
public class JsonStorage
{
   
   public List<TaskItem> LoadTasks(string filePath)
{
    if (File.Exists(filePath))
    {
        var json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<List<TaskItem>>(json) ?? new List<TaskItem>();
    }
    else
    {
        var emptyList = new List<TaskItem>();
        File.WriteAllText(filePath, JsonSerializer.Serialize(emptyList));
        return emptyList;
    }
}
public void SaveTasks(string filePath, List<TaskItem> tasks)
{
   
    var json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions
    {
        WriteIndented = true 
    });

   
    File.WriteAllText(filePath, json);
}


}