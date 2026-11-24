# Change scenarios for regression-impact testing

Each scenario is a small, realistic code change on its own branch. Open a pull request from the
branch to `main` and compare what Jawdah selects with the expected sets below.

Two sets are given for each scenario:

- **Must rerun**: test cases whose execution path reaches the changed code. Impact analysis should
  select at least these (recall). Selecting many more lowers precision.
- **Expected to fail**: test cases whose expected results change because of the change. Everything
  else in "Must rerun" should still pass.

The sets were derived from the call paths in the code and checked with the xUnit reference tests
(`dotnet test --filter "TestCase=TC-0xx"`).

## S1 Peak multiplier from 1.5 to 1.75

Branch: `change/pricing-peak`. File: `src/RoomBooking.Core/Services/PricingPolicy.cs`.
Changed symbol: `PricingPolicy.PeakMultiplier`, read by `PricingService.Quote`.

| Set | Test cases |
| --- | --- |
| Must rerun | TC-012, TC-013, TC-016, TC-017, TC-018, TC-019, TC-020, TC-021, TC-026, TC-027, TC-028 |
| Expected to fail | TC-012, TC-013, TC-016, TC-018, TC-019, TC-021 |
| Rerun but pass | TC-017 and TC-020 (no peak hours), TC-026 (moves to Friday), TC-027 (rejected before pricing), TC-028 (message has no price) |
| Not affected | TC-022 and TC-024 (seeded invoices and stored prices do not change) |

New values: TC-012 Al Waha 350.00 and Board Room 700.00; TC-013 1,400.00; TC-016 700.00;
TC-018 275.00; TC-019 630.00 with a 70.00 discount; TC-021 total 724.50.

## S2 Free-cancellation window from 24 to 72 hours

Branch: `change/cancellation-window`. File: `src/RoomBooking.Core/Services/CancellationPolicy.cs`.
Changed symbol: `CancellationPolicy.FreeCancellationHours`, read by `CancellationPolicy.CalculateFee`.

| Set | Test cases |
| --- | --- |
| Must rerun | TC-023, TC-024, TC-025, TC-029 |
| Expected to fail | TC-023 and TC-029 (BK-1002 is 53 hours ahead, so the fee becomes 300.00 SAR) |
| Rerun but pass | TC-024 (already late, still 150.00), TC-025 (admin fee is waived) |

## S3 Confirmation notification wording

Branch: `change/notification-wording`. File: `src/RoomBooking.Core/Services/NotificationService.cs`.
Changed symbol: `NotificationService.BookingConfirmed`.

| Set | Test cases |
| --- | --- |
| Must rerun | TC-016, TC-021, TC-028 |
| Expected to fail | TC-028 |
| Rerun but pass | TC-016 and TC-021 (they do not check the notification text) |

## S4 Optional floor number on rooms

Branch: `change/room-floor`. Files: `Room.cs`, `RoomService.cs`, `RoomsController.cs`,
`ViewModels.cs`, `Views/Rooms/*`.
Changed symbols: `Room.Floor` (new), `RoomService.Create`, `RoomService.Update`,
`RoomsController.Create`, `RoomsController.Edit`, `RoomsController.Index`.

| Set | Test cases |
| --- | --- |
| Must rerun | TC-003, TC-004, TC-005, TC-006, TC-007, TC-008, TC-009 |
| Expected to fail | none (the field is optional and existing texts are unchanged) |

This scenario checks that impact analysis selects tests by the changed members, not by the whole
`Room` type. `Room` is used by almost every test case, so a type-level selection would rerun the
whole suite.

## How to measure

For each scenario:

1. Recall = |selected AND must rerun| / |must rerun|. Target 100%.
2. Precision = |selected AND must rerun| / |selected|.
3. After the rerun, every test case in "Expected to fail" is `failed` and every other selected case
   is `passed`.
