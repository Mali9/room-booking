# RoomBooking

A small ASP.NET Core MVC (.NET 8, C#) meeting-room booking application. It exists to exercise
Jawdah end to end: code-to-graph analysis, test execution by the Runner agent, and
change-impact (regression) selection.

## Run

```sh
docker compose up -d --build
```

Open http://localhost:5080. The system time is frozen at Monday 2027-01-04 08:00 and the data
is in memory, so every start (and every reset at `/Dev`) returns to the same state.

Sign-in accounts and seed data: [docs/test-data.md](docs/test-data.md).

## Test

```sh
docker run --rm -v "$PWD:/src" -w /src mcr.microsoft.com/dotnet/sdk:8.0 dotnet test
```

Run the reference tests for given test cases:

```sh
dotnet test --filter "TestCase=TC-016|TestCase=TC-021"
```

## Structure

```
src/RoomBooking.Core            Domain, repository interfaces, business services
src/RoomBooking.Infrastructure  In-memory repositories, seed data, clocks
src/RoomBooking.Web             MVC controllers and Razor views
tests/RoomBooking.Tests         xUnit reference tests, tagged with TestCase and Requirement traits
```

Main dependencies between services:

```
BookingService -> AvailabilityService, PricingService, CancellationPolicy,
                  InvoiceService, NotificationService
InvoiceService -> PricingService (VAT)
PricingService -> PricingPolicy
RoomService    -> room and booking repositories
```

## Documents

| Document | Content |
| --- | --- |
| [docs/BRD.md](docs/BRD.md) | Business requirements (REQ-01 to REQ-15, BR-01 to BR-20) |
| [docs/test-cases.json](docs/test-cases.json) | 32 hand-written UI test cases, the source of truth |
| [docs/test-cases.md](docs/test-cases.md) | Readable rendering of the test cases |
| [docs/traceability.md](docs/traceability.md) | Requirement to test case, and code symbol to test case |
| [docs/change-scenarios.md](docs/change-scenarios.md) | Prepared code changes with the expected impact |
| [docs/test-data.md](docs/test-data.md) | Accounts, seed data and system time |

After editing `test-cases.json`, regenerate the Markdown with `python tools/render_docs.py`.

## Using it with Jawdah

1. Register the GitHub repository as an application and run code analysis on `main`.
2. Upload `docs/BRD.md`, analyze it and publish the requirement set.
3. Create a discovery snapshot with the app reachable from Jawdah's containers at
   `http://host.docker.internal:5080`, with personas named **Admin**, **Customer Sara** and
   **Customer Omar** (credentials in `docs/test-data.md`).
4. Create a draft test set, then import the hand-written cases with
   [tools/jawdah-import/import_test_cases.py](tools/jawdah-import/import_test_cases.py).
5. Review and publish the set, then run it with the Runner agent.
6. Open a pull request from a `change/*` branch and compare the selected test cases with
   [docs/change-scenarios.md](docs/change-scenarios.md).

The `/Dev` reset page and the fixed clock are test-environment features. Do not enable
`Testing:EnableReset` in production.
