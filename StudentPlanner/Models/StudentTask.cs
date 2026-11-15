using System.ComponentModel.DataAnnotations;

namespace StudentPlanner.Models;

public class StudentTask
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Please enter a task title.")]
    [StringLength(100, ErrorMessage = "The task title cannot exceed 100 characters.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter a subject.")]
    [StringLength(60, ErrorMessage = "The subject cannot exceed 60 characters.")]
    public string Subject { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please choose a deadline.")]
    public DateTime Deadline { get; set; } = DateTime.Today;

    [Required]
    [RegularExpression("^(Low|Medium|High)$", ErrorMessage = "Choose a valid priority.")]
    public string Priority { get; set; } = "Medium";

    public bool IsCompleted { get; set; }
    public bool IsOverdue => !IsCompleted && Deadline.Date < DateTime.Today;
}
