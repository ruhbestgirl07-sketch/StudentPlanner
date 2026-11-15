# Final Project — 20-Criteria Traceability Matrix

This matrix maps the submitted implementation to the lecturer's 20 extended criteria. It distinguishes implemented evidence from checks that must still be demonstrated by the team. A feature in source code is not proof that every environment or scenario has been tested.

| # | Official criterion | Implementation evidence | Evidence still needed for a defensible 5/5 |
|---:|---|---|---|
| 1 | Code Structure and Design | Layered Pages / Shared Components / Models / Services; DI through `ITaskService`; documented data flow | Explain design decisions and show code organization during defense |
| 2 | Component Reusability | `TaskForm` and `TaskCard` use parameters and `EventCallback`; form IDs are configurable | Demonstrate component reuse and event flow in the source |
| 3 | Cross-Platform Compatibility | ASP.NET Core and browser-based UI; documented LAN phone access | Run and capture evidence on Windows, macOS and Linux if available, plus multiple device types |
| 4 | State Management | Singleton application service, synchronized state, defensive snapshots and `TasksChanged` notifications | Demonstrate updates in two active browser sessions; explain shared-board scope |
| 5 | User Interface Design | Consistent Bootstrap/custom design system, dashboard cards, status styles and layout | Include final screenshots and obtain usability feedback |
| 6 | User Experience | Search, filters, sorting, CRUD feedback, validation, empty state, upcoming deadlines | Demonstrate edge cases and keyboard-only navigation |
| 7 | Responsiveness | Bootstrap grid, flexible controls, media queries, mobile navigation and compact layouts | Capture phone/tablet/desktop tests and verify no horizontal overflow |
| 8 | Core Functionality | CRUD, complete/reopen, validation, search/filter/sort, dashboard and exports | Run the full manual regression matrix and attach evidence |
| 9 | Advanced Features | Upcoming deadline view, CSV/JSON/ICS exports, health endpoint and resilient persistence | Demonstrate each feature live; advanced feature depth remains coursework-scale |
| 10 | Error Handling and Resilience | Service validation, logging, atomic-style file replacement, malformed-data recovery copy and safe event notifications | Demonstrate invalid data recovery and file permission failure handling |
| 11 | Efficiency | In-memory snapshots, LINQ filtering, small dependency footprint and static assets | Measure startup, response time and memory on a documented test device |
| 12 | Scalability | Service abstraction makes a future storage provider replaceable; design boundaries documented | Replace JSON with a transactional database and test concurrent/multi-instance load for production-level 5/5 |
| 13 | Resource Utilization | No polling loop or unnecessary external network calls; simple in-memory task operations | Collect CPU/memory/network measurements under representative use |
| 14 | Testing Coverage | xUnit tests cover service validation, state changes, persistence resilience and exporters | Run tests and capture results; add browser integration and end-to-end tests for the strongest evidence |
| 15 | Quality Assurance | Manual regression matrix includes browser, device, accessibility and export checks | Execute and sign off the matrix; attach screenshots from real browsers/devices |
| 16 | Documentation | README, architecture, technology comparison, user guide, QA plan and presentation outline | Add team-specific setup screenshots and final test results |
| 17 | Innovation | Upcoming deadline overview and calendar/backup exports improve student workflow | Explain the problem solved and collect feedback; originality is partly a lecturer judgment |
| 18 | Creative Use of Blazor/C#/.NET Features | Interactive Razor components, EventCallbacks, DI, server state events, data annotations and .NET file/JSON APIs | Explain the component lifecycle, state notifications and service boundary in defense |
| 19 | Professionalism | Consistent UI, documentation, validation, exports, health endpoint and explicit limitations | Verify clean Release build, deployment configuration, accessibility and final ZIP contents |
| 20 | Presentation | `docs/presentation-outline.md` follows the required presentation topics and demo flow | Create and deliver the actual presentation; complete the 15–20 minute defense |

## Submission evidence checklist

- [ ] Run `dotnet restore` and `dotnet build StudentPlanner.sln` on the submission machine.
- [ ] Run `dotnet test StudentPlanner.sln` and capture the actual test summary.
- [ ] Demonstrate create, validation, edit, cancel, complete, reopen and delete.
- [ ] Demonstrate dashboard, upcoming deadlines, search, filters and sorting.
- [ ] Demonstrate CSV, JSON backup and ICS downloads.
- [ ] Demonstrate persistence after application restart.
- [ ] Open the app at phone, tablet and desktop widths and capture screenshots.
- [ ] Test at least two current browsers and record versions.
- [ ] Show `/health` and explain the shared-board data model.
- [ ] Prepare slides with team names, aim, goals, plan, technology comparison, architecture, relevance, methodology and mockups.
- [ ] Rehearse a 15–20 minute defense; every team member should be able to explain any project area.

**Important:** Criteria 3, 7, 11, 12, 13, 15 and 20 cannot honestly be marked fully verified from source code alone. They require real environment tests, measurements, a production-grade storage decision where applicable, and the actual presentation/defense. This matrix is intended to make those remaining actions visible rather than claim evidence that does not exist.
