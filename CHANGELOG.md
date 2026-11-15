# Quality Upgrade Changelog

## Student Planner — quality enhancement pass

- Added an About page with stack, purpose and data/privacy notes.
- Added a dashboard upcoming-deadlines panel.
- Added JSON backup and iCalendar deadline exports alongside CSV.
- Extracted CSV/JSON/ICS formatting into a separately testable utility.
- Hardened CSV exports against spreadsheet formula prefixes and improved ICS escaping.
- Improved JSON persistence with unique temporary files, flush-to-disk, recovery copies for malformed JSON, duplicate-ID cleanup and safer event notifications.
- Added baseline HTTP security headers and a `/health` endpoint.
- Added a keyboard skip link, accessible navigation label, reduced-motion support and mobile refinements.
- Fixed duplicate form control IDs when add and edit forms are both rendered.
- Expanded xUnit coverage for validation, persistence, resilience, concurrent operations and export formatting.
- Added a 20-criteria evidence matrix, expanded architecture and QA documentation, and defense preparation material.

## Verification note

The source archive was inspected and packaged in this environment, but the .NET SDK is not installed here. Therefore `dotnet restore`, `dotnet build` and `dotnet test` could not be executed during this enhancement pass. Run the provided test script on a machine with .NET 8 before submission and capture the actual output.
