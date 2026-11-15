# Student Planner

Student Planner is a responsive academic task-management application built with **C#, ASP.NET Core 8 and Blazor Interactive Server**. It helps students organize assignments by subject, deadline and priority, track progress, and export their work.

## Highlights

- Dashboard with total, completed, pending and overdue task statistics.
- Study progress indicator and upcoming-deadline overview.
- Create, edit, complete, reopen and delete tasks.
- Server-side business validation in addition to Blazor form validation.
- Search by title or subject; filter by status; sort by deadline, priority or title.
- CSV export with spreadsheet-formula injection protection.
- JSON backup export for portable copies of the task list.
- iCalendar (`.ics`) export for importing deadlines into calendar applications.
- Shared state notifications so connected Blazor circuits refresh after task changes.
- JSON persistence using a temporary file and same-directory replacement; malformed JSON is copied to a recovery file before the application continues.
- Health endpoint at `/health` and baseline HTTP security headers.
- Reusable `TaskForm` and `TaskCard` Razor components.
- Automated xUnit tests for business rules, persistence, resilience and export formatting.

## Requirements

- .NET 8 SDK
- A modern browser (current Chrome, Edge, Firefox or Safari)
- Visual Studio Code, Visual Studio or another C# editor

## Run locally

Open a terminal in the folder containing `StudentPlanner.sln`:

```bash
dotnet restore
dotnet build
dotnet run --project StudentPlanner/StudentPlanner.csproj
```

Open the local URL printed by ASP.NET Core. The included development profile uses port `5130`.

Check application health at `http://localhost:5130/health` when using the HTTP development profile.

### Access from a phone on the same Wi-Fi network

```bash
dotnet run --project StudentPlanner/StudentPlanner.csproj -- --urls http://0.0.0.0:5130
```

Open `http://YOUR-COMPUTER-LAN-IP:5130` on the phone. Keep the development server running and allow the port through the computer firewall if prompted.

## Run automated tests

From the solution root:

```bash
dotnet test StudentPlanner.sln
```

For a Release build:

```bash
dotnet build StudentPlanner.sln --configuration Release
dotnet test StudentPlanner.sln --configuration Release
```

## Project structure

```text
StudentPlanner.sln
├── StudentPlanner/
│   ├── Components/
│   │   ├── Layout/       # Main layout and navigation
│   │   ├── Pages/        # Dashboard, tasks, counter, weather, about, error
│   │   └── Shared/       # Reusable TaskForm and TaskCard
│   ├── Models/           # StudentTask and validation rules
│   ├── Services/         # Task service contract, state, persistence, exporters
│   ├── App_Data/         # JSON task store
│   ├── docs/             # Architecture, user guide, QA and presentation material
│   ├── wwwroot/          # CSS, JavaScript and static assets
│   └── Program.cs        # DI, middleware and endpoints
└── StudentPlanner.Tests/ # xUnit service and export tests
```

## Data, privacy and deployment

The coursework build stores a **single shared task board** in `StudentPlanner/App_Data/tasks.json`. This is appropriate for a local demonstration, not for private multi-user production use. There are no user accounts or per-user access controls; do not enter confidential information. Keep backups of `tasks.json` and use the JSON Backup action regularly.

The application attempts to preserve malformed task data in a file named `tasks.json.corrupt-*` and uses a temporary file for safer writes. These safeguards reduce accidental data loss but do not replace database transactions or a backup strategy.

For production deployment, replace the JSON store with a transactional database (for example, PostgreSQL or SQL Server through EF Core), add authentication and per-user authorization, configure persistent storage and backups, use HTTPS, and add monitoring. Multiple application instances must not write to the same JSON file.

## Export formats

- **CSV:** exports the currently filtered and sorted task list. Values are quoted and formula-like prefixes are neutralized for spreadsheet safety.
- **JSON:** exports the complete task list as a backup file.
- **ICS:** exports task deadlines as all-day calendar events. Import the file into a calendar app; it does not create live calendar synchronization.

## Weather page

The Weather page is a demonstration of tabular data presentation. Its forecast values are sample data and are not retrieved from a live weather service.

## Documentation

- `docs/architecture.md` — architecture, data flow and design decisions.
- `docs/technology-analysis.md` — comparison of implementation approaches.
- `docs/user-guide.md` — user instructions and export workflows.
- `docs/testing-and-quality.md` — automated test scope, manual QA matrix and limitations.
- `docs/requirements-traceability.md` — evidence map for the 20 grading criteria.
- `docs/presentation-outline.md` — presentation structure and demo plan.

## Suggested demonstration sequence

1. Show the dashboard and upcoming deadlines.
2. Add a task and demonstrate required-field validation.
3. Edit and cancel an edit.
4. Complete and reopen a task.
5. Search, filter and sort the list.
6. Export CSV, JSON backup and ICS calendar.
7. Show `/health` and the automated test results.
8. Explain the architecture and the documented production limitations.
