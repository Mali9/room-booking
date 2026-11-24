# Traceability: RoomBooking

Generated from `test-cases.json` by `tools/render_docs.py`.

## Requirement to test cases

| Requirement | Test cases |
| --- | --- |
| REQ-01 | TC-001, TC-002, TC-003 |
| REQ-02 | TC-004, TC-005 |
| REQ-03 | TC-003, TC-006, TC-007, TC-008, TC-009 |
| REQ-04 | TC-010, TC-011 |
| REQ-05 | TC-012, TC-013, TC-027 |
| REQ-06 | TC-014, TC-015 |
| REQ-07 | TC-016 |
| REQ-08 | TC-012, TC-016, TC-017, TC-018, TC-020, TC-026 |
| REQ-09 | TC-019, TC-020, TC-021 |
| REQ-10 | TC-021, TC-022, TC-024, TC-026 |
| REQ-11 | TC-023, TC-024 |
| REQ-12 | TC-026, TC-027 |
| REQ-13 | TC-028, TC-029 |
| REQ-14 | TC-030, TC-031, TC-032 |
| REQ-15 | TC-025 |

## Code symbol to test cases

This is the hand-written ground truth for change-impact analysis: a change to a symbol
should select at least the test cases listed for it.

| Symbol | Test cases |
| --- | --- |
| `AccountController.Login` | TC-001, TC-002 |
| `AuthService.ValidateCredentials` | TC-001, TC-002 |
| `AvailabilityService.EnsureBookable` | TC-016, TC-026, TC-027 |
| `AvailabilityService.FindAvailableRooms` | TC-012, TC-013 |
| `AvailabilityService.IsRoomFree` | TC-012, TC-027 |
| `AvailabilityService.ValidateSlot` | TC-012, TC-014, TC-015 |
| `BookingService.Cancel` | TC-023, TC-024, TC-025, TC-029 |
| `BookingService.Create` | TC-016, TC-021, TC-028 |
| `BookingService.Get` | TC-032 |
| `BookingService.GetVisibleTo` | TC-030, TC-031 |
| `BookingService.PreviewCancellationFee` | TC-023 |
| `BookingService.Reschedule` | TC-026, TC-027 |
| `BookingsController.Cancel` | TC-023, TC-024, TC-025 |
| `BookingsController.CancelConfirmed` | TC-023, TC-024, TC-025 |
| `BookingsController.Create` | TC-016, TC-021 |
| `BookingsController.Details` | TC-016, TC-032 |
| `BookingsController.Index` | TC-030, TC-031 |
| `BookingsController.Reschedule` | TC-026, TC-027 |
| `BookingsController.Search` | TC-012, TC-013, TC-014, TC-015, TC-016, TC-017, TC-018, TC-019, TC-020 |
| `CancellationPolicy.CalculateFee` | TC-023, TC-024, TC-025, TC-029 |
| `InMemoryBookingRepository.GetAll` | TC-031 |
| `InMemoryBookingRepository.GetByCustomer` | TC-030 |
| `InMemoryBookingRepository.GetConfirmedForRoom` | TC-010 |
| `InMemoryRoomRepository.NextId` | TC-006 |
| `InMemoryUserRepository.GetByEmail` | TC-001 |
| `InvoiceService.Get` | TC-022 |
| `InvoiceService.GetVisibleTo` | TC-022 |
| `InvoiceService.IssueCancellationFee` | TC-024 |
| `InvoiceService.IssueForBooking` | TC-016, TC-021, TC-026 |
| `InvoiceService.VoidBookingInvoices` | TC-023, TC-024, TC-026 |
| `InvoicesController.Details` | TC-021, TC-022 |
| `InvoicesController.Index` | TC-022 |
| `NotificationService.BookingCancelled` | TC-023, TC-024, TC-029 |
| `NotificationService.BookingConfirmed` | TC-016, TC-028 |
| `NotificationService.BookingRescheduled` | TC-026 |
| `NotificationService.MarkAllRead` | TC-028 |
| `NotificationService.UnreadCount` | TC-028 |
| `NotificationsController.Index` | TC-028, TC-029 |
| `PasswordHasher.Verify` | TC-001, TC-002 |
| `PricingPolicy.LongBookingDiscountRate` | TC-019 |
| `PricingPolicy.LongBookingHours` | TC-019 |
| `PricingPolicy.PeakDays` | TC-017 |
| `PricingPolicy.PeakEnd` | TC-018 |
| `PricingPolicy.PeakMultiplier` | TC-018 |
| `PricingService.CalculateVat` | TC-021, TC-024 |
| `PricingService.IsPeak` | TC-012, TC-017, TC-018, TC-020 |
| `PricingService.Quote` | TC-012, TC-013, TC-016, TC-017, TC-018, TC-019, TC-020, TC-021, TC-026 |
| `Program (cookie authentication, AccessDeniedPath)` | TC-003 |
| `RoomService.Create` | TC-006, TC-007, TC-008 |
| `RoomService.GetCatalogue` | TC-004, TC-005 |
| `RoomService.HasUpcomingBookings` | TC-010 |
| `RoomService.SetActive` | TC-010, TC-011 |
| `RoomService.Update` | TC-009 |
| `RoomService.Validate` | TC-006, TC-007, TC-008, TC-009 |
| `RoomsController.Create` | TC-003, TC-006, TC-007, TC-008 |
| `RoomsController.Edit` | TC-009 |
| `RoomsController.Index` | TC-004, TC-005 |
| `RoomsController.SetActive` | TC-010, TC-011 |
| `_Layout` | TC-028 |
