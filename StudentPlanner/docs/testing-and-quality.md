# Testing and Quality Assurance

## Automated tests

The `StudentPlanner.Tests` project uses xUnit and targets the service and pure export logic. The suite includes tests for:

- Create, update, complete, reopen and delete behavior.
- Required values, length limits, valid priority and deadline validation.
- ID allocation and behavior after deletion.
- Statistics and overdue calculations.
- Persistence across service instances.
- Defensive copies returned to callers.
- Change notifications and subscriber-failure resilience.
- Recovery-copy creation for malformed JSON and duplicate-ID cleanup.
- CSV escaping and spreadsheet formula-prefix protection.
- JSON backup round-trip and iCalendar event formatting/escaping.

Run from the solution root:

```bash
dotnet restore
dotnet test StudentPlanner.sln
```

A successful local test run should be captured as a screenshot for the submission. Do not report a test count unless the command output confirms it.

## Manual regression matrix

| Area | Test action | Expected result |
|---|---|---|
| Dashboard | Open Home | Counts and progress match stored tasks |
| Upcoming | Add task due within seven days | Task appears in upcoming list |
| Create | Submit valid task | Task appears and persists |
| Validation | Submit empty title/subject | Submission is blocked with validation feedback |
| Validation | Exceed title/subject length | Submission is blocked |
| Update | Edit an existing task | Updated values persist |
| Cancel | Cancel edit | Original values remain unchanged |
| Status | Complete and reopen | Status and dashboard update |
| Delete | Delete a task | Task disappears and remains deleted after restart |
| Search | Search partial title and subject | Only matching tasks appear |
| Filter | Select Pending, Completed, Overdue | Correct subset appears |
| Sort | Select Deadline, Priority, Title | Results use the selected ordering |
| CSV | Export filtered list | CSV downloads and opens with correct columns |
| JSON | Export backup | JSON file can be parsed and contains task records |
| Calendar | Export ICS | Calendar file contains all-day deadline events |
| Persistence | Restart application | Existing tasks reload |
| Health | Open `/health` | Health endpoint returns healthy status |
| Responsive | Test phone, tablet and desktop widths | Content and controls remain usable without horizontal overflow |
| Browser | Test current Safari, Chrome, Firefox or Edge | Core workflows operate consistently |
| Accessibility | Keyboard navigate and inspect labels | Visible focus, skip link and form labels are present |

## Security and resilience controls

- Normal Razor text output is HTML-encoded.
- Task fields are validated at both form and service boundaries.
- Writes use a temporary file and replacement to reduce partial JSON writes.
- Invalid JSON is copied to a timestamped recovery file before the app starts with an empty in-memory list.
- UI subscriber exceptions are logged and do not roll back persisted changes.
- CSV values beginning with common spreadsheet formula characters are neutralized.
- HTTP responses include baseline content-type, frame, referrer and permissions policy headers.

## Known limitations and honest QA boundary

- The app uses a single shared task board; there is no authentication or per-user isolation.
- JSON storage is not suitable for multiple application instances or high write concurrency.
- The Weather page uses sample values, not a live API.
- The ICS export is a one-time file export, not two-way calendar synchronization.
- Automated unit tests do not replace browser-based integration and end-to-end testing.
- Cross-browser, cross-device and deployed-environment tests must be run by the project team and evidenced with screenshots before claiming completion.
- No performance benchmark or load test is bundled; performance claims should be based on measured results, not assumptions.
