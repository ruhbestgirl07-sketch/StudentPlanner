# Student Planner — User Guide

## Dashboard
The dashboard shows total, completed, pending, and overdue tasks. The progress bar shows the percentage of tasks marked complete. The Upcoming deadlines panel lists pending tasks due within the next seven days. Select **Manage my tasks** to open task management.

## Add a task
Enter a title, subject, deadline, and priority, then select **Add task**. Title and subject are required. The title is limited to 100 characters and subject to 60 characters.

## Manage tasks
- **Complete:** marks a pending task as completed.
- **Reopen:** returns a completed task to pending.
- **Edit:** opens the task form with existing values.
- **Save changes:** saves the edited task.
- **Cancel:** closes editing without saving.
- **Delete:** removes the task from the list and JSON storage.

## Search, filter, and sort
Search matches part of a task title or subject. Status filters include All, Pending, Completed, and Overdue. Sorting is available by deadline, priority, or title.

## Export
- **Export CSV:** downloads the currently visible (filtered and sorted) tasks as a spreadsheet-friendly CSV file.
- **Backup JSON:** downloads the complete task list as a JSON backup. Store it somewhere safe; this version provides export but not an in-app restore workflow.
- **Calendar ICS:** downloads task deadlines as all-day calendar events that can be imported into a calendar application. It is a one-time export, not live synchronization.

## Mobile access
Run the application using the computer's local network address, for example `dotnet run -- --urls http://0.0.0.0:5130`. Open the computer's LAN IP address from a phone connected to the same Wi-Fi network. Keep the development server running.

## About and service health

Open **About** in the top navigation to review the technology stack and data/privacy notes. The `/health` endpoint reports application health for basic monitoring.
