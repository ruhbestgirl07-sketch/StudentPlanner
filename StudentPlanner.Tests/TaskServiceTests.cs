using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging.Abstractions;
using StudentPlanner.Models;
using StudentPlanner.Services;
using Xunit;

namespace StudentPlanner.Tests;

public sealed class TaskServiceTests : IDisposable
{
    private readonly string _directory = Path.Combine(Path.GetTempPath(), "StudentPlannerTests", Guid.NewGuid().ToString("N"));
    private readonly TaskService _service;

    public TaskServiceTests()
    {
        Directory.CreateDirectory(_directory);
        _service = CreateService();
    }

    [Fact]
    public void AddTask_AssignsId_AndPersists()
    {
        _service.AddTask(NewTask("Discrete mathematics"));
        Assert.Single(_service.GetTasks());
        Assert.Equal(1, _service.GetTasks()[0].Id);
        Assert.True(File.Exists(DataFile));
    }

    [Fact]
    public void AddTask_RejectsBlankTitle()
    {
        Assert.Throws<ArgumentException>(() => _service.AddTask(NewTask(" ")));
        Assert.Empty(_service.GetTasks());
    }

    [Fact]
    public void AddTask_RejectsBlankSubject()
    {
        var task = NewTask("Valid");
        task.Subject = " ";
        Assert.Throws<ArgumentException>(() => _service.AddTask(task));
    }

    [Theory]
    [InlineData(101, 10)]
    [InlineData(10, 61)]
    public void AddTask_RejectsTextBeyondLimits(int titleLength, int subjectLength)
    {
        var task = NewTask(new string('T', titleLength));
        task.Subject = new string('S', subjectLength);
        Assert.Throws<ArgumentException>(() => _service.AddTask(task));
    }

    [Fact]
    public void AddTask_RejectsInvalidPriority()
    {
        var task = NewTask("Valid");
        task.Priority = "Urgent";
        Assert.Throws<ArgumentException>(() => _service.AddTask(task));
    }

    [Fact]
    public void AddTask_RejectsDefaultDeadline()
    {
        var task = NewTask("Valid");
        task.Deadline = default;
        Assert.Throws<ArgumentException>(() => _service.AddTask(task));
    }

    [Fact]
    public void UpdateTask_ChangesExistingTask()
    {
        _service.AddTask(NewTask("Draft"));
        var task = _service.GetTasks()[0];
        task.Title = "Final draft";
        Assert.True(_service.UpdateTask(task));
        Assert.Equal("Final draft", _service.GetTasks()[0].Title);
    }

    [Fact]
    public void UpdateTask_ReturnsFalseForUnknownId()
    {
        var task = NewTask("Missing");
        task.Id = 999;
        Assert.False(_service.UpdateTask(task));
    }

    [Fact]
    public void SetCompleted_UpdatesStatistics()
    {
        _service.AddTask(NewTask("Read chapter"));
        Assert.True(_service.SetCompleted(1, true));
        var stats = _service.GetStatistics();
        Assert.Equal(1, stats.Completed);
        Assert.Equal(0, stats.Pending);
    }

    [Fact]
    public void SetCompleted_ReturnsFalseForUnknownId() =>
        Assert.False(_service.SetCompleted(999, true));

    [Fact]
    public void DeleteTask_RemovesTask()
    {
        _service.AddTask(NewTask("Remove me"));
        Assert.True(_service.DeleteTask(1));
        Assert.Empty(_service.GetTasks());
    }

    [Fact]
    public void DeleteTask_ReturnsFalseForUnknownId() =>
        Assert.False(_service.DeleteTask(999));

    [Fact]
    public void NewServiceInstance_LoadsPersistedTasks()
    {
        _service.AddTask(NewTask("Persisted task"));
        var reloaded = CreateService();
        Assert.Single(reloaded.GetTasks());
        Assert.Equal("Persisted task", reloaded.GetTasks()[0].Title);
    }

    [Fact]
    public void AddTask_UsesIncreasingIdsAfterDeletion()
    {
        _service.AddTask(NewTask("First"));
        _service.AddTask(NewTask("Second"));
        _service.DeleteTask(1);
        _service.AddTask(NewTask("Third"));
        Assert.Equal(new[] { 2, 3 }, _service.GetTasks().Select(task => task.Id));
    }

    [Fact]
    public void GetTasks_ReturnsDefensiveCopies()
    {
        _service.AddTask(NewTask("Original"));
        var snapshot = _service.GetTasks();
        snapshot[0].Title = "Mutated outside service";
        Assert.Equal("Original", _service.GetTasks()[0].Title);
    }

    [Fact]
    public void GetStatistics_CountsOverdueTasks()
    {
        var overdue = NewTask("Late");
        overdue.Deadline = DateTime.Today.AddDays(-1);
        _service.AddTask(overdue);
        _service.AddTask(NewTask("On time"));
        var stats = _service.GetStatistics();
        Assert.Equal(2, stats.Total);
        Assert.Equal(2, stats.Pending);
        Assert.Equal(1, stats.Overdue);
    }

    [Fact]
    public void TasksChanged_IsRaisedAfterMutation()
    {
        var calls = 0;
        _service.TasksChanged += () => calls++;
        _service.AddTask(NewTask("Notify"));
        Assert.Equal(1, calls);
    }

    [Fact]
    public void FailingSubscriberDoesNotUndoPersistedMutation()
    {
        _service.TasksChanged += () => throw new InvalidOperationException("Disconnected UI");
        _service.AddTask(NewTask("Still saved"));
        Assert.Single(_service.GetTasks());
        Assert.True(File.Exists(DataFile));
    }

    [Fact]
    public void InvalidJson_IsPreservedAsRecoveryCopy()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(DataFile)!);
        File.WriteAllText(DataFile, "{ this is not valid JSON");
        var recovered = CreateService();
        Assert.Empty(recovered.GetTasks());
        Assert.Single(Directory.GetFiles(Path.GetDirectoryName(DataFile)!, "tasks.json.corrupt-*"));
        Assert.Contains("not valid JSON", File.ReadAllText(DataFile));
    }

    [Fact]
    public void LoadedRecordsWithDuplicateIdsAreDeduplicated()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(DataFile)!);
        var first = NewTask("First"); first.Id = 1;
        var second = NewTask("Second"); second.Id = 1;
        File.WriteAllText(DataFile, System.Text.Json.JsonSerializer.Serialize(new[] { first, second }));
        var loaded = CreateService().GetTasks();
        Assert.Single(loaded);
        Assert.Equal("First", loaded[0].Title);
    }

    [Fact]
    public void ConcurrentAdds_AssignUniqueIdsAndPersistAllTasks()
    {
        Parallel.For(0, 25, index => _service.AddTask(NewTask($"Concurrent task {index}")));
        var tasks = _service.GetTasks();
        Assert.Equal(25, tasks.Count);
        Assert.Equal(25, tasks.Select(task => task.Id).Distinct().Count());
        Assert.Equal(25, System.Text.Json.JsonSerializer.Deserialize<List<StudentTask>>(File.ReadAllText(DataFile))!.Count);
    }

    private string DataFile => Path.Combine(_directory, "App_Data", "tasks.json");
    private TaskService CreateService() => new(new TestEnvironment(_directory), NullLogger<TaskService>.Instance);

    private static StudentTask NewTask(string title) => new()
    {
        Title = title,
        Subject = "Computer Science",
        Deadline = DateTime.Today.AddDays(3),
        Priority = "Medium"
    };

    public void Dispose()
    {
        if (Directory.Exists(_directory))
            Directory.Delete(_directory, recursive: true);
    }

    private sealed class TestEnvironment(string root) : IWebHostEnvironment
    {
        public string ApplicationName { get; set; } = "StudentPlanner.Tests";
        public IFileProvider ContentRootFileProvider { get; set; } = new NullFileProvider();
        public string ContentRootPath { get; set; } = root;
        public string EnvironmentName { get; set; } = "Development";
        public string WebRootPath { get; set; } = root;
        public IFileProvider WebRootFileProvider { get; set; } = new NullFileProvider();
    }
}
