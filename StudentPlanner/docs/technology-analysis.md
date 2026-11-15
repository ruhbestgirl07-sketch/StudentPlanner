# Technology Analysis and Comparison

## Alternatives considered

| Technology / architecture | Advantages | Trade-offs | Decision for this project |
|---|---|---|---|
| Blazor Interactive Server | C# end-to-end, reusable Razor components, server-side .NET execution and fast iteration | Requires a live connection; server resources are used per active circuit | Selected for this coursework application |
| ASP.NET Core MVC | Mature request/response model, explicit controller/view separation and broad ecosystem | More page navigation and often more JavaScript for interactive UI behavior | Suitable alternative for content-oriented workflows |
| Blazor WebAssembly | Client-side .NET execution and potential offline-oriented experiences | Larger initial download and client/API architecture complexity | Could be selected if offline client execution becomes a requirement |
| JSON file persistence | Human-readable, portable, simple to inspect and package | No transactions, indexing, multi-instance coordination or robust concurrent multi-user semantics | Selected only for the small single-board demonstration |
| Relational database + EF Core | Transactions, indexing, migrations, constraints and stronger multi-user scaling | More setup, schema management and deployment dependencies | Recommended production persistence layer |

## Selected stack

- **C# and .NET 8:** typed application logic and platform libraries.
- **Blazor Interactive Server:** interactive UI without duplicating business logic in JavaScript.
- **Razor components:** composable pages and reusable UI elements.
- **Data Annotations and service validation:** validation at both presentation and application boundaries.
- **Dependency Injection:** separates presentation from application services.
- **JSON persistence:** transparent storage for coursework scale.
- **Bootstrap and custom CSS:** responsive layout and consistent visual design.
- **xUnit:** repeatable unit tests for service behavior and export formatting.

## Why the current design is appropriate to the coursework scope

The project prioritizes demonstrable CRUD workflows, state updates, validation, exports and a compact deployment footprint. It does not claim production-grade multi-tenant operation. The README and architecture document explicitly identify the database, authentication, distributed coordination and monitoring work needed before production deployment.
