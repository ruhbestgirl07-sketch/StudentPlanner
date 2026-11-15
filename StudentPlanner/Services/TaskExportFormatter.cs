using System.Text;
using System.Text.Json;
using StudentPlanner.Models;

namespace StudentPlanner.Services;

/// <summary>Pure export formatting functions, kept separate from UI event handlers for easy testing.</summary>
public static class TaskExportFormatter
{
    public static string ToCsv(IEnumerable<StudentTask> tasks)
    {
        var lines = new List<string> { "Id,Title,Subject,Deadline,Priority,Status" };
        lines.AddRange(tasks.Select(task => string.Join(",",
            task.Id,
            CsvCell(task.Title),
            CsvCell(task.Subject),
            task.Deadline.ToString("yyyy-MM-dd"),
            CsvCell(task.Priority),
            CsvCell(GetStatus(task)))));
        return string.Join("\r\n", lines);
    }

    public static string ToJsonBackup(IEnumerable<StudentTask> tasks) =>
        JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });

    public static string ToIcalendar(IEnumerable<StudentTask> tasks, DateTime? utcNow = null)
    {
        var stamp = (utcNow ?? DateTime.UtcNow).ToUniversalTime().ToString("yyyyMMdd'T'HHmmss'Z'");
        var builder = new StringBuilder();
        builder.Append("BEGIN:VCALENDAR\r\nVERSION:2.0\r\nPRODID:-//Student Planner//EN\r\nCALSCALE:GREGORIAN\r\n");

        foreach (var task in tasks)
        {
            builder.Append("BEGIN:VEVENT\r\n")
                .Append("UID:studentplanner-").Append(task.Id).Append("@local\r\n")
                .Append("DTSTAMP:").Append(stamp).Append("\r\n")
                .Append("DTSTART;VALUE=DATE:").Append(task.Deadline.ToString("yyyyMMdd")).Append("\r\n")
                .Append("DTEND;VALUE=DATE:").Append(task.Deadline.AddDays(1).ToString("yyyyMMdd")).Append("\r\n")
                .Append("SUMMARY:").Append(IcsEscape(task.Title)).Append("\r\n")
                .Append("DESCRIPTION:").Append(IcsEscape($"Subject: {task.Subject} | Priority: {task.Priority} | Status: {GetStatus(task)}")).Append("\r\n")
                .Append("END:VEVENT\r\n");
        }

        builder.Append("END:VCALENDAR\r\n");
        return builder.ToString();
    }

    public static string GetStatus(StudentTask task) =>
        task.IsCompleted ? "Completed" : task.IsOverdue ? "Overdue" : "Pending";

    private static string CsvCell(string value)
    {
        // Prevent spreadsheet formula execution when exported values begin with formula characters.
        var trimmed = value.TrimStart();
        if (trimmed.Length > 0 && trimmed[0] is '=' or '+' or '-' or '@' or '\t' or '\r')
            value = "'" + value;
        return "\"" + value.Replace("\"", "\"\"") + "\"";
    }

    private static string IcsEscape(string value) => value
        .Replace("\\", "\\\\", StringComparison.Ordinal)
        .Replace(";", "\\;", StringComparison.Ordinal)
        .Replace(",", "\\,", StringComparison.Ordinal)
        .Replace("\r", string.Empty, StringComparison.Ordinal)
        .Replace("\n", "\\n", StringComparison.Ordinal);
}
