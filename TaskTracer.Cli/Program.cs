using TaskTracker.Cli.Models;
using TaskTracker.Cli.Services;

var storage = new JsonStorage();
string filePath = "tasks.json";

if (args.Length == 0)
{
    Console.WriteLine("Usage: task-cli <command> [arguments]");
    return;
}

var command = args[0].ToLower();

switch (command)
{
    case "add":
        if (args.Length < 2)
        {
            Console.WriteLine("Description required.");
            return;
        }
        var description = args[1];
        var tasks = storage.LoadTasks(filePath);

        var newTask = new TaskItem
        {
            Id = tasks.Count > 0 ? tasks.Max(t => t.Id) + 1 : 1,
            Description = description,
            Status = TaskTracker.Cli.Models.TaskStatus.Todo,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        tasks.Add(newTask);
        storage.SaveTasks(filePath, tasks);
        Console.WriteLine($"Task added successfully (ID: {newTask.Id})");
        break;

    case "list":
    var listTasks = storage.LoadTasks(filePath);

    if (args.Length == 1)
    {
        foreach (var t in listTasks)
        {
            Console.WriteLine($"{t.Id}. {t.Description} [{t.Status}]");
        }
    }
    else
    {
        var filter = args[1].ToLower();
        IEnumerable<TaskItem> filtered = listTasks;

        if (filter == "done")
            filtered = listTasks.Where(t => t.Status == TaskTracker.Cli.Models.TaskStatus.Done);
        else if (filter == "todo")
            filtered = listTasks.Where(t => t.Status == TaskTracker.Cli.Models.TaskStatus.Todo);
        else if (filter == "in-progress")
            filtered = listTasks.Where(t => t.Status == TaskTracker.Cli.Models.TaskStatus.InProgress);

        foreach (var t in filtered)
        {
            Console.WriteLine($"{t.Id}. {t.Description} [{t.Status}]");
        }
    }
    break;


    case "update":
    if (args.Length < 3)
    {
        Console.WriteLine("Usage: task-cli update <id> <new description>");
        return;
    }

    if (!int.TryParse(args[1], out int updateId))
    {
        Console.WriteLine("Invalid task ID.");
        return;
    }

    var updateTasks = storage.LoadTasks(filePath);
    var taskToUpdate = updateTasks.FirstOrDefault(t => t.Id == updateId);

    if (taskToUpdate == null)
    {
        Console.WriteLine($"Task with ID {updateId} not found.");
        return;
    }

    taskToUpdate.Description = args[2];
    taskToUpdate.UpdatedAt = DateTime.Now;

    storage.SaveTasks(filePath, updateTasks);
    Console.WriteLine($"Task {updateId} updated successfully.");
    break;
    case "delete":
    if (args.Length < 2)
    {
        Console.WriteLine("Usage: task-cli delete <id>");
        return;
    }

    if (!int.TryParse(args[1], out int deleteId))
    {
        Console.WriteLine("Invalid task ID.");
        return;
    }

    var deleteTasks = storage.LoadTasks(filePath);
    var taskToDelete = deleteTasks.FirstOrDefault(t => t.Id == deleteId);

    if (taskToDelete == null)
    {
        Console.WriteLine($"Task with ID {deleteId} not found.");
        return;
    }

    deleteTasks.Remove(taskToDelete);
    storage.SaveTasks(filePath, deleteTasks);
    Console.WriteLine($"Task {deleteId} deleted successfully.");
    break;
    case "mark-in-progress":
    if (args.Length < 2)
    {
        Console.WriteLine("Usage: task-cli mark-in-progress <id>");
        return;
    }

    if (!int.TryParse(args[1], out int progressId))
    {
        Console.WriteLine("Invalid task ID.");
        return;
    }

    var progressTasks = storage.LoadTasks(filePath);
    var taskInProgress = progressTasks.FirstOrDefault(t => t.Id == progressId);

    if (taskInProgress == null)
    {
        Console.WriteLine($"Task with ID {progressId} not found.");
        return;
    }

    taskInProgress.Status = TaskTracker.Cli.Models.TaskStatus.InProgress;
    taskInProgress.UpdatedAt = DateTime.Now;

    storage.SaveTasks(filePath, progressTasks);
    Console.WriteLine($"Task {progressId} marked as in progress.");
    break;
    case "mark-done":
    if (args.Length < 2)
    {
        Console.WriteLine("Usage: task-cli mark-done <id>");
        return;
    }

    if (!int.TryParse(args[1], out int doneId))
    {
        Console.WriteLine("Invalid task ID.");
        return;
    }

    var doneTasks = storage.LoadTasks(filePath);
    var taskDone = doneTasks.FirstOrDefault(t => t.Id == doneId);

    if (taskDone == null)
    {
        Console.WriteLine($"Task with ID {doneId} not found.");
        return;
    }

    taskDone.Status = TaskTracker.Cli.Models.TaskStatus.Done;
    taskDone.UpdatedAt = DateTime.Now;

    storage.SaveTasks(filePath, doneTasks);
    Console.WriteLine($"Task {doneId} marked as done.");
    break;

  
    default:
        Console.WriteLine("Unknown command.");
        break;
}
