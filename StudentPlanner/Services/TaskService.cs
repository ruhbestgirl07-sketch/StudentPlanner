using System.Text.Json;
using StudentPlanner.Models;

namespace StudentPlanner.Services;

/// <summary>
/// Central task state and JSON persistence service.
/// Registered as a singleton so all active Blazor circuits see consistent task changes.
/// </summary>
public sealed class TaskService : ITaskService
{
    private readonly object _sync = new();
    private readonly string _filePath;
    private readonly ILogger<TaskService> _logger;
    private List<StudentTask> _tasks;
    private int _nextId;

    public event Action? TasksChanged;

    public TaskService(IWebHostEnvironment environment, ILogger<TaskService> logger)
    {
        _logger = logger;
        var dataDirectory = Path.Combine(environment.ContentRootPath, "App_Data");
        Directory.CreateDirectory(dataDirectory);
        _filePath = Path.Combine(dataDirectory, "tasks.json");

        // Preserve data from the earlier application version when upgrading.
        var legacyPath = Path.Combine(AppContext.BaseDirectory, "tasks.json");
        if (!File.Exists(_filePath) && File.Exists(legacyPath))
            File.Copy(legacyPath, _filePath);

        _tasks = LoadTasks();
        _nextId = _tasks.Count == 0 ? 1 : _tasks.Max(task => task.Id) + 1;
    }

    public IReadOnlyList<StudentTask> GetTasks()
    {
        lock (_sync)
            return _tasks.Select(Clone).ToList();
    }

    public TaskStatistics GetStatistics()
    {
        lock (_sync)
        {
            var completed = _tasks.Count(task => task.IsCompleted);
            var overdue = _tasks.Count(task => !task.IsCompleted && task.Deadline.Date < DateTime.Today);
            return new TaskStatistics(_tasks.Count, completed, _tasks.Count - completed, overdue);
        }
    }

    public void AddTask(StudentTask task)
    {
        ArgumentNullException.ThrowIfNull(task);
        Validate(task);
        lock (_sync)
        {
            var candidate = _tasks.Select(Clone).ToList();
            var newTask = Clone(task);
            newTask.Id = _nextId;
            newTask.Title = newTask.Title.Trim();
            newTask.Subject = newTask.Subject.Trim();
            candidate.Add(newTask);
            Persist(candidate);
            _tasks = candidate;
            _nextId++;
        }
        NotifyChanged();
    }

    public bool UpdateTask(StudentTask updatedTask)
    {
        ArgumentNullException.ThrowIfNull(updatedTask);
        Validate(updatedTask);
        var changed = false;
        lock (_sync)
        {
            var candidate = _tasks.Select(Clone).ToList();
            var existing = candidate.FirstOrDefault(task => task.Id == updatedTask.Id);
            if (existing is null) return false;

            existing.Title = updatedTask.Title.Trim();
            existing.Subject = updatedTask.Subject.Trim();
            existing.Deadline = updatedTask.Deadline;
            existing.Priority = updatedTask.Priority;
            Persist(candidate);
            _tasks = candidate;
            changed = true;
        }
        if (changed) NotifyChanged();
        return changed;
    }

    public bool SetCompleted(int id, bool isCompleted)
    {
        lock (_sync)
        {
            var candidate = _tasks.Select(Clone).ToList();
            var task = candidate.FirstOrDefault(item => item.Id == id);
            if (task is null) return false;
            if (task.IsCompleted == isCompleted) return true;
            task.IsCompleted = isCompleted;
            Persist(candidate);
            _tasks = candidate;
        }
        NotifyChanged();
        return true;
    }

    public bool DeleteTask(int id)
    {
        lock (_sync)
        {
            var candidate = _tasks.Select(Clone).ToList();
            var removed = candidate.RemoveAll(task => task.Id == id);
            if (removed == 0) return false;
            Persist(candidate);
            _tasks = candidate;
        }
        NotifyChanged();
        return true;
    }

    private List<StudentTask> LoadTasks()
    {
        if (!File.Exists(_filePath)) return new List<StudentTask>();
        try
        {
            var json = File.ReadAllText(_filePath);
            var loaded = JsonSerializer.Deserialize<List<StudentTask>>(json) ?? new List<StudentTask>();

            // Ignore malformed records instead of allowing one bad row to break the whole application.
            return loaded
                .Where(task => task is not null && task.Id > 0 &&
                    !string.IsNullOrWhiteSpace(task.Title) && task.Title.Trim().Length <= 100 &&
                    !string.IsNullOrWhiteSpace(task.Subject) && task.Subject.Trim().Length <= 60 &&
                    task.Deadline != default && (task.Priority is "Low" or "Medium" or "High"))
                .GroupBy(task => task.Id)
                .Select(group => Clone(group.First()))
                .OrderBy(task => task.Id)
                .ToList();
        }
        catch (JsonException exception)
        {
            PreserveInvalidDataFile();
            _logger.LogError(exception, "Task data is invalid JSON. A recovery copy was created; the original file has been preserved.");
            return new List<StudentTask>();
        }
        catch (IOException exception)
        {
            _logger.LogError(exception, "Task data could not be read. The application will start with an empty in-memory list.");
            return new List<StudentTask>();
        }
        catch (UnauthorizedAccessException exception)
        {
            _logger.LogError(exception, "The application does not have permission to read task data.");
            return new List<StudentTask>();
        }
    }

    private void Persist(List<StudentTask> candidate)
    {
        var json = JsonSerializer.Serialize(candidate, new JsonSerializerOptions { WriteIndented = true });
        var temporaryPath = _filePath + "." + Guid.NewGuid().ToString("N") + ".tmp";
        try
        {
            // Write-through plus same-directory rename prevents readers seeing a partial JSON document.
            using (var stream = new FileStream(temporaryPath, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            using (var writer = new StreamWriter(stream))
            {
                writer.Write(json);
                writer.Flush();
                stream.Flush(flushToDisk: true);
            }
            File.Move(temporaryPath, _filePath, overwrite: true);
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException)
        {
            try { if (File.Exists(temporaryPath)) File.Delete(temporaryPath); }
            catch (IOException cleanupException) { _logger.LogWarning(cleanupException, "Could not remove a temporary task-data file."); }
            _logger.LogError(exception, "Could not persist task changes.");
            throw;
        }
    }

    private void PreserveInvalidDataFile()
    {
        try
        {
            if (!File.Exists(_filePath)) return;
            var recoveryPath = _filePath + ".corrupt-" + DateTime.UtcNow.ToString("yyyyMMdd-HHmmssfff");
            File.Copy(_filePath, recoveryPath, overwrite: false);
            _logger.LogWarning("Invalid task data was copied to {RecoveryPath}.", recoveryPath);
        }
        catch (IOException exception)
        {
            _logger.LogError(exception, "Could not create a recovery copy of invalid task data.");
        }
        catch (UnauthorizedAccessException exception)
        {
            _logger.LogError(exception, "Could not create a recovery copy of invalid task data.");
        }
    }

    private static void Validate(StudentTask task)
    {
        if (string.IsNullOrWhiteSpace(task.Title))
            throw new ArgumentException("Please enter a task title.");
        if (task.Title.Trim().Length > 100)
            throw new ArgumentException("The task title cannot exceed 100 characters.");
        if (string.IsNullOrWhiteSpace(task.Subject))
            throw new ArgumentException("Please enter a subject.");
        if (task.Subject.Trim().Length > 60)
            throw new ArgumentException("The subject cannot exceed 60 characters.");
        if (task.Deadline == default)
            throw new ArgumentException("Please choose a valid deadline.");
        if (task.Priority is not ("Low" or "Medium" or "High"))
            throw new ArgumentException("Choose a valid priority.");
    }

    private static StudentTask Clone(StudentTask task) => new()
    {
        Id = task.Id,
        Title = task.Title,
        Subject = task.Subject,
        Deadline = task.Deadline,
        Priority = task.Priority,
        IsCompleted = task.IsCompleted
    };

    private void NotifyChanged()
    {
        var handlers = TasksChanged;
        if (handlers is null) return;

        foreach (Action handler in handlers.GetInvocationList())
        {
            try { handler.Invoke(); }
            catch (Exception exception)
            {
                // A disconnected UI subscriber must not make a successfully persisted command look failed.
                _logger.LogWarning(exception, "A task-change subscriber failed while refreshing its UI.");
            }
        }
    }
}

public sealed record TaskStatistics(int Total, int Completed, int Pending, int Overdue);
