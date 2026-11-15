# StudentPlanner – Academic Task Management System

## Project Overview

StudentPlanner is a web-based academic task management application developed using ASP.NET Core Blazor.

The application helps students organize their academic responsibilities, manage deadlines, track progress, and improve their study planning.

## Main Features

- **Dashboard:** Displays total tasks, completed tasks, pending tasks, overdue tasks, and study progress.
- **Task Management:** Create, edit, delete, complete, and reopen tasks.
- **Task Priorities:** Organize tasks according to their priority.
- **Search:** Find tasks by title or subject.
- **Filtering:** Filter tasks by completion status.
- **Sorting:** Organize tasks by deadline and priority.
- **Deadline Management:** Track upcoming deadlines and overdue tasks.
- **Data Export:** Export tasks to CSV, JSON backup, and ICS calendar formats.
- **Counter:** Interactive counter functionality.
- **Weather:** Weather information page.
- **Responsive Design:** Supports desktop and mobile devices.

## Technologies Used

- C#
- ASP.NET Core Blazor
- .NET 8
- HTML5
- CSS3
- JavaScript
- JSON
- Bootstrap
- Git and GitHub

## Project Structure

```text
StudentPlanner_Upgrade/
│
├── StudentPlanner/
│   ├── Components/
│   ├── Models/
│   ├── Services/
│   ├── Properties/
│   ├── wwwroot/
│   └── StudentPlanner.csproj
│
├── StudentPlanner.Tests/
│
├── StudentPlanner.sln
├── README.md
├── CHANGELOG.md
├── START_HERE.md
└── .gitignore
```

## Installation and Setup

### Requirements

- .NET 8 SDK
- Visual Studio Code or Visual Studio
- A modern web browser

### Run the Application

1. Clone the repository:

```bash
git clone https://github.com/ruhbestgirl07-sketch/StudentPlanner.git
```

2. Navigate to the project directory:

```bash
cd StudentPlanner
```

3. Restore dependencies:

```bash
dotnet restore
```

4. Run the application:

```bash
dotnet run --project StudentPlanner/StudentPlanner.csproj
```

5. Open the local URL displayed in the terminal.

## Testing

The project includes an automated test project:

`StudentPlanner.Tests`

The latest recorded test execution reported:

- Passed: 27
- Failed: 0
- Skipped: 0
- Total: 27

The solution also built successfully with zero warnings and zero errors in the recorded Release build.

## Responsive Compatibility

StudentPlanner was tested on desktop and mobile devices. The application interface and task management functionality were verified during manual testing.

## Repository

GitHub: https://github.com/ruhbestgirl07-sketch/StudentPlanner

## Conclusion

StudentPlanner provides a practical solution for organizing academic tasks and monitoring study progress. It combines task management, deadline tracking, search, filtering, sorting, and data export in one responsive web application.
