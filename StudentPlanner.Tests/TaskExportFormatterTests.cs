using StudentPlanner.Models;
using StudentPlanner.Services;
using Xunit;

namespace StudentPlanner.Tests;

public sealed class TaskExportFormatterTests
{
    [Fact]
    public void Csv_ContainsHeaderAndTaskValues()
    {
        var csv = TaskExportFormatter.ToCsv(new[] { Task("Essay") });
        Assert.Contains("Id,Title,Subject,Deadline,Priority,Status", csv);
        Assert.Contains("\"Essay\"", csv);
        Assert.Contains("\"Pending\"", csv);
    }

    [Fact]
    public void Csv_EscapesQuotesAndProtectsFormulaPrefixes()
    {
        var task = Task("=HYPERLINK(\"https://example.com\")");
        task.Subject = "Math, Science";
        var csv = TaskExportFormatter.ToCsv(new[] { task });
        Assert.Contains("'=HYPERLINK(\"\"https://example.com\"\")", csv);
        Assert.Contains("\"Math, Science\"", csv);
    }

    [Fact]
    public void Calendar_ProducesValidAllDayEventEnvelope()
    {
        var calendar = TaskExportFormatter.ToIcalendar(new[] { Task("Exam") }, new DateTime(2026, 11, 15, 12, 0, 0, DateTimeKind.Utc));
        Assert.StartsWith("BEGIN:VCALENDAR\r\n", calendar);
        Assert.Contains("BEGIN:VEVENT\r\n", calendar);
        Assert.Contains("DTSTART;VALUE=DATE:20261118", calendar);
        Assert.Contains("SUMMARY:Exam", calendar);
        Assert.EndsWith("END:VCALENDAR\r\n", calendar);
    }

    [Fact]
    public void Calendar_EscapesSpecialCharactersAndLineBreaks()
    {
        var task = Task("Exam, Part 1; Final\\Review\nNext line");
        var calendar = TaskExportFormatter.ToIcalendar(new[] { task });
        Assert.Contains("SUMMARY:Exam\\, Part 1\\; Final\\\\Review\\nNext line", calendar);
    }

    [Fact]
    public void JsonBackup_IsDeserializable()
    {
        var json = TaskExportFormatter.ToJsonBackup(new[] { Task("Backup") });
        var restored = System.Text.Json.JsonSerializer.Deserialize<List<StudentTask>>(json);
        Assert.NotNull(restored);
        Assert.Single(restored);
        Assert.Equal("Backup", restored[0].Title);
    }

    private static StudentTask Task(string title) => new()
    {
        Id = 7,
        Title = title,
        Subject = "Discrete Mathematics",
        Deadline = new DateTime(2026, 11, 18),
        Priority = "High"
    };
}
