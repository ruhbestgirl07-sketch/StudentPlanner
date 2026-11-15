# Student Planner — Presentation and Defense Plan (15–20 minutes)

Replace the team placeholders with the real full names before submission. Do not invent participant names or claim tests that have not been run.

## Suggested 10-slide structure

1. **Title and team** — Student Planner, full names of team members, course and project topic.
2. **Aim and objectives** — help students organize academic tasks, deadlines and priorities; list measurable project goals and the work plan.
3. **Problem relevance** — missed deadlines, scattered assignment notes and lack of progress visibility; practical value for students.
4. **Technology analysis** — compare Blazor Interactive Server, ASP.NET Core MVC and Blazor WebAssembly; explain the chosen .NET 8/C#/Blazor stack.
5. **Architecture** — show `docs/architecture.md`; explain Pages, reusable components, model, `ITaskService`, `TaskService`, JSON storage and export formatter.
6. **Methodology and mockups** — requirements analysis, interface design, implementation, iterative testing; show dashboard and task-management screenshots.
7. **Core functionality demo** — create, validate, edit, complete, reopen, delete, search, filter and sort.
8. **Additional capabilities** — upcoming deadlines, CSV export, JSON backup, ICS calendar export, health endpoint and resilience behavior.
9. **Testing, results and limitations** — show actual `dotnet test` output and manual QA evidence; clearly explain shared JSON storage, no authentication and sample Weather data.
10. **Conclusion and future work** — summarize outcomes; discuss database/EF Core, authentication, reminders, live weather and deployment monitoring as future improvements.

## Suggested timing

| Section | Time |
|---|---:|
| Introduction, team, aim and relevance | 2 min |
| Technology comparison and architecture | 4 min |
| Methodology and interface | 2 min |
| Live demonstration | 5–6 min |
| Testing, limitations and conclusion | 2–3 min |
| Questions | Remaining time |

## Defense preparation questions

1. Why did you choose Blazor Interactive Server rather than MVC or WebAssembly?
2. What is the responsibility of `ITaskService` and why does the UI depend on an interface?
3. How do `TaskForm` and `TaskCard` reuse code and communicate with parent pages?
4. How does the application notify connected pages when task data changes?
5. How are invalid user inputs handled at the UI and service layers?
6. How is task data saved and what happens if the JSON file is malformed?
7. What is the difference between CSV, JSON backup and ICS export?
8. What are the limitations of a shared JSON file compared with a relational database?
9. Which tests are automated and which checks must be performed manually?
10. What would you change before deploying the application for real multi-user use?

## Live demo checklist

- Start the app and show `/health`.
- Open the dashboard and explain statistics and upcoming deadlines.
- Add a task; show validation for missing required fields.
- Edit a task and cancel once to show the original data remains intact.
- Complete and reopen a task; show dashboard updates.
- Search, filter and sort the task list.
- Download CSV, JSON and ICS files.
- Restart the app and show that tasks persist.
- Finish by explaining the architecture and known limitations honestly.
