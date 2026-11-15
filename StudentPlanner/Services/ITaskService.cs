using StudentPlanner.Models;

namespace StudentPlanner.Services;

/// <summary>Application-facing contract for task management.</summary>
public interface ITaskService
{
    event Action? TasksChanged;
    IReadOnlyList<StudentTask> GetTasks();
    TaskStatistics GetStatistics();
    void AddTask(StudentTask task);
    bool UpdateTask(StudentTask updatedTask);
    bool SetCompleted(int id, bool isCompleted);
    bool DeleteTask(int id);
}
