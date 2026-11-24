# Test cases: RoomBooking

Generated from `test-cases.json` by `tools/render_docs.py`. Edit the JSON, then re-run.

Common preconditions for every case:

- System time is Monday 2027-01-04 08:00 (shown in the page footer).
- Seed data is as described in docs/test-data.md.

Every case starts with a reset step: Open /Dev and select 'Reset test data'. Expected: The message 'Test data has been reset.' is shown.

| ID | Title | Requirements | Persona | Priority | Type |
| --- | --- | --- | --- | --- | --- |
| TC-001 | Customer signs in with valid credentials | REQ-01 | Signed out | High | Functional |
| TC-002 | Sign-in with a wrong password is rejected | REQ-01 | Signed out | High | Negative |
| TC-003 | Customer cannot open the Add room page | REQ-01, REQ-03 | Customer Sara | High | Security |
| TC-004 | Customer room list shows active rooms only | REQ-02 | Customer Sara | Medium | Functional |
| TC-005 | Admin room list shows all rooms with status | REQ-02 | Admin | Medium | Functional |
| TC-006 | Admin adds a new room | REQ-03 | Admin | High | Functional |
| TC-007 | Room with capacity 0 is rejected | REQ-03 | Admin | Medium | Negative |
| TC-008 | Duplicate room name is rejected | REQ-03 | Admin | Medium | Negative |
| TC-009 | Admin changes a room's hourly rate | REQ-03 | Admin | Medium | Functional |
| TC-010 | Room with upcoming bookings cannot be deactivated | REQ-04 | Admin | High | Negative |
| TC-011 | Admin activates an inactive room | REQ-04 | Admin | Medium | Functional |
| TC-012 | Search excludes booked and inactive rooms | REQ-05, REQ-08 | Customer Sara | High | Functional |
| TC-013 | Search filters rooms by capacity | REQ-05 | Customer Sara | Medium | Functional |
| TC-014 | Slot ending after 22:00 is rejected | REQ-06 | Customer Sara | Medium | Negative |
| TC-015 | Slot in the past is rejected | REQ-06 | Customer Sara | Medium | Negative |
| TC-016 | Customer books a room at peak price | REQ-07, REQ-08 | Customer Sara | High | Functional |
| TC-017 | Weekend hours are off-peak | REQ-08 | Customer Sara | High | Functional |
| TC-018 | Slot crossing the end of peak hours is priced per hour | REQ-08 | Customer Sara | High | Functional |
| TC-019 | Four-hour booking gets a 10% discount | REQ-09 | Customer Sara | High | Functional |
| TC-020 | Evening long booking is off-peak and discounted | REQ-08, REQ-09 | Customer Sara | Medium | Functional |
| TC-021 | Booking issues an invoice with discount and VAT | REQ-10, REQ-09 | Customer Sara | High | Functional |
| TC-022 | Existing invoice shows discount and VAT | REQ-10 | Customer Omar | Medium | Functional |
| TC-023 | Cancelling more than 24 hours ahead is free | REQ-11 | Customer Sara | High | Functional |
| TC-024 | Late cancellation charges 50% plus VAT | REQ-11, REQ-10 | Customer Sara | High | Functional |
| TC-025 | Admin cancellation waives the fee | REQ-15 | Admin | Medium | Functional |
| TC-026 | Reschedule re-prices the booking and re-issues the invoice | REQ-12, REQ-08, REQ-10 | Customer Sara | High | Functional |
| TC-027 | Reschedule into a booked slot is rejected | REQ-12, REQ-05 | Customer Sara | High | Negative |
| TC-028 | Booking sends a confirmation notification | REQ-13 | Customer Sara | Medium | Functional |
| TC-029 | Cancellation sends a notification with the fee | REQ-13 | Customer Sara | Medium | Functional |
| TC-030 | Customer sees only their own bookings | REQ-14 | Customer Sara | High | Security |
| TC-031 | Admin sees all bookings with customer names | REQ-14 | Admin | Medium | Functional |
| TC-032 | Customer cannot open another customer's booking | REQ-14 | Customer Sara | High | Security |

## TC-001 Customer signs in with valid credentials

A customer signs in with a valid e-mail and password and is shown as signed in.

- Requirements: REQ-01
- Persona: Signed out
- Priority: High; type: Functional
- Automated reference: AuthTests.Valid_customer_credentials_sign_in

Preconditions:

- The user is signed out.

| # | Action | Test data | Expected result |
| --- | --- | --- | --- |
| 1 | Open /Dev and select 'Reset test data'. |  | The message 'Test data has been reset.' is shown. |
| 2 | Open /Account/Login. |  | The Sign in page with Email and Password fields is shown. |
| 3 | Enter the e-mail and password and select 'Sign in'. | Email: sara@roombooking.test; Password: Sara#2027 | The Home page opens and the header shows 'Signed in as Sara Al-Harbi'. |

Code touchpoints: `AccountController.Login`, `AuthService.ValidateCredentials`, `PasswordHasher.Verify`, `InMemoryUserRepository.GetByEmail`

## TC-002 Sign-in with a wrong password is rejected

Invalid credentials keep the user signed out and show an error.

- Requirements: REQ-01
- Persona: Signed out
- Priority: High; type: Negative
- Automated reference: AuthTests.Wrong_password_is_rejected

Preconditions:

- The user is signed out.

| # | Action | Test data | Expected result |
| --- | --- | --- | --- |
| 1 | Open /Dev and select 'Reset test data'. |  | The message 'Test data has been reset.' is shown. |
| 2 | Open /Account/Login. |  | The Sign in page is shown. |
| 3 | Enter the e-mail and a wrong password and select 'Sign in'. | Email: sara@roombooking.test; Password: wrong-password | The message 'Invalid email or password.' is shown and the header still shows 'Sign in'. |

Code touchpoints: `AccountController.Login`, `AuthService.ValidateCredentials`, `PasswordHasher.Verify`

## TC-003 Customer cannot open the Add room page

Admin-only pages are denied to customers.

- Requirements: REQ-01, REQ-03
- Persona: Customer Sara
- Priority: High; type: Security
- Automated reference: none (UI only)

Preconditions:

- Signed in as Customer Sara.

| # | Action | Test data | Expected result |
| --- | --- | --- | --- |
| 1 | Open /Dev and select 'Reset test data'. |  | The message 'Test data has been reset.' is shown. |
| 2 | Open /Rooms/Create. |  | The 'Access denied' page is shown with 'You do not have permission to open this page.' |

Code touchpoints: `RoomsController.Create`, `Program (cookie authentication, AccessDeniedPath)`

## TC-004 Customer room list shows active rooms only

Customers see the active rooms and no admin controls.

- Requirements: REQ-02
- Persona: Customer Sara
- Priority: Medium; type: Functional
- Automated reference: RoomTests.Customer_catalogue_hides_inactive_rooms

Preconditions:

- Signed in as Customer Sara.

| # | Action | Test data | Expected result |
| --- | --- | --- | --- |
| 1 | Open /Dev and select 'Reset test data'. |  | The message 'Test data has been reset.' is shown. |
| 2 | Open /Rooms. |  | The table lists exactly Al Nakheel (R-101), Al Waha (R-102) and Board Room (R-201) with their capacity and hourly rate. |
| 3 | Check the page for Focus Pod and for an 'Add room' button. |  | Focus Pod is not listed, there is no 'Add room' button and no Status column. |

Code touchpoints: `RoomsController.Index`, `RoomService.GetCatalogue`

## TC-005 Admin room list shows all rooms with status

Admins see active and inactive rooms.

- Requirements: REQ-02
- Persona: Admin
- Priority: Medium; type: Functional
- Automated reference: RoomTests.Admin_catalogue_includes_inactive_rooms

Preconditions:

- Signed in as Admin.

| # | Action | Test data | Expected result |
| --- | --- | --- | --- |
| 1 | Open /Dev and select 'Reset test data'. |  | The message 'Test data has been reset.' is shown. |
| 2 | Open /Rooms. |  | Four rooms are listed. Al Nakheel, Al Waha and Board Room show status Active; Focus Pod (R-301) shows status Inactive. An 'Add room' button is shown. |

Code touchpoints: `RoomsController.Index`, `RoomService.GetCatalogue`

## TC-006 Admin adds a new room

A valid room is created as Active with the next room ID.

- Requirements: REQ-03
- Persona: Admin
- Priority: High; type: Functional
- Automated reference: RoomTests.Admin_creates_room_with_next_id

Preconditions:

- Signed in as Admin.

| # | Action | Test data | Expected result |
| --- | --- | --- | --- |
| 1 | Open /Dev and select 'Reset test data'. |  | The message 'Test data has been reset.' is shown. |
| 2 | Open /Rooms and select 'Add room'. |  | The 'Add room' form is shown. |
| 3 | Enter the room details and select 'Create room'. | Name: Al Rawdah; Capacity: 8; Hourly rate: 150 | The Rooms page shows 'Room Al Rawdah created with ID R-401.' |
| 4 | Find Al Rawdah in the room list. |  | R-401 Al Rawdah is listed with capacity 8, hourly rate 150.00 SAR and status Active. |

Code touchpoints: `RoomsController.Create`, `RoomService.Create`, `RoomService.Validate`, `InMemoryRoomRepository.NextId`

## TC-007 Room with capacity 0 is rejected

Capacity must be between 1 and 50.

- Requirements: REQ-03
- Persona: Admin
- Priority: Medium; type: Negative
- Automated reference: RoomTests.Capacity_zero_is_rejected

Preconditions:

- Signed in as Admin.

| # | Action | Test data | Expected result |
| --- | --- | --- | --- |
| 1 | Open /Dev and select 'Reset test data'. |  | The message 'Test data has been reset.' is shown. |
| 2 | Open /Rooms/Create. |  | The 'Add room' form is shown. |
| 3 | Enter the room details and select 'Create room'. | Name: Al Rawdah; Capacity: 0; Hourly rate: 150 | The form stays open and shows 'Capacity must be between 1 and 50.' |

Code touchpoints: `RoomsController.Create`, `RoomService.Create`, `RoomService.Validate`

## TC-008 Duplicate room name is rejected

Room names are unique.

- Requirements: REQ-03
- Persona: Admin
- Priority: Medium; type: Negative
- Automated reference: RoomTests.Duplicate_room_name_is_rejected

Preconditions:

- Signed in as Admin.

| # | Action | Test data | Expected result |
| --- | --- | --- | --- |
| 1 | Open /Dev and select 'Reset test data'. |  | The message 'Test data has been reset.' is shown. |
| 2 | Open /Rooms/Create. |  | The 'Add room' form is shown. |
| 3 | Enter the room details and select 'Create room'. | Name: Al Waha; Capacity: 5; Hourly rate: 150 | The form stays open and shows "A room named 'Al Waha' already exists." |

Code touchpoints: `RoomsController.Create`, `RoomService.Create`, `RoomService.Validate`

## TC-009 Admin changes a room's hourly rate

Edits are saved and shown in the room list.

- Requirements: REQ-03
- Persona: Admin
- Priority: Medium; type: Functional
- Automated reference: RoomTests.Admin_updates_hourly_rate

Preconditions:

- Signed in as Admin.

| # | Action | Test data | Expected result |
| --- | --- | --- | --- |
| 1 | Open /Dev and select 'Reset test data'. |  | The message 'Test data has been reset.' is shown. |
| 2 | Open /Rooms and select 'Edit' for Al Nakheel. |  | The 'Edit room' form shows Room ID R-101, name Al Nakheel, capacity 6 and hourly rate 100. |
| 3 | Change the hourly rate and select 'Save changes'. | Hourly rate: 120 | The Rooms page shows 'Room Al Nakheel updated.' and Al Nakheel has hourly rate 120.00 SAR. |

Code touchpoints: `RoomsController.Edit`, `RoomService.Update`, `RoomService.Validate`

## TC-010 Room with upcoming bookings cannot be deactivated

Deactivation is blocked while confirmed future bookings exist.

- Requirements: REQ-04
- Persona: Admin
- Priority: High; type: Negative
- Automated reference: RoomTests.Room_with_upcoming_bookings_cannot_be_deactivated

Preconditions:

- Signed in as Admin.
- Board Room has upcoming booking BK-1003.

| # | Action | Test data | Expected result |
| --- | --- | --- | --- |
| 1 | Open /Dev and select 'Reset test data'. |  | The message 'Test data has been reset.' is shown. |
| 2 | Open /Rooms and select 'Deactivate' for Board Room. |  | The message 'Room Board Room has upcoming bookings and cannot be deactivated.' is shown and Board Room is still Active. |

Code touchpoints: `RoomsController.SetActive`, `RoomService.SetActive`, `RoomService.HasUpcomingBookings`, `InMemoryBookingRepository.GetConfirmedForRoom`

## TC-011 Admin activates an inactive room

An inactive room can be activated.

- Requirements: REQ-04
- Persona: Admin
- Priority: Medium; type: Functional
- Automated reference: RoomTests.Inactive_room_can_be_activated

Preconditions:

- Signed in as Admin.

| # | Action | Test data | Expected result |
| --- | --- | --- | --- |
| 1 | Open /Dev and select 'Reset test data'. |  | The message 'Test data has been reset.' is shown. |
| 2 | Open /Rooms and select 'Activate' for Focus Pod. |  | The message 'Room Focus Pod activated.' is shown and Focus Pod has status Active. |

Code touchpoints: `RoomsController.SetActive`, `RoomService.SetActive`

## TC-012 Search excludes booked and inactive rooms

Al Nakheel is booked at the requested time and Focus Pod is inactive, so neither is offered.

- Requirements: REQ-05, REQ-08
- Persona: Customer Sara
- Priority: High; type: Functional
- Automated reference: SearchAndPricingTests.Search_excludes_booked_and_inactive_rooms

Preconditions:

- Signed in as Customer Sara.
- BK-1001 holds Al Nakheel on 2027-01-04 10:00 - 12:00.

| # | Action | Test data | Expected result |
| --- | --- | --- | --- |
| 1 | Open /Dev and select 'Reset test data'. |  | The message 'Test data has been reset.' is shown. |
| 2 | Open /Bookings/Search. |  | The 'Find a room' form is shown. |
| 3 | Enter the search criteria and select 'Search'. | Date: 2027-01-04; Start time: 10:00; Duration: 1; Attendees: 4 | Available rooms are, in order: Al Waha at 300.00 SAR and Board Room at 600.00 SAR, each with 1 / 0 peak / off-peak hours. Al Nakheel and Focus Pod are not listed. |

Code touchpoints: `BookingsController.Search`, `AvailabilityService.FindAvailableRooms`, `AvailabilityService.IsRoomFree`, `AvailabilityService.ValidateSlot`, `PricingService.Quote`, `PricingService.IsPeak`

## TC-013 Search filters rooms by capacity

Only rooms that fit the attendees are offered.

- Requirements: REQ-05
- Persona: Customer Sara
- Priority: Medium; type: Functional
- Automated reference: SearchAndPricingTests.Search_filters_by_capacity

Preconditions:

- Signed in as Customer Sara.

| # | Action | Test data | Expected result |
| --- | --- | --- | --- |
| 1 | Open /Dev and select 'Reset test data'. |  | The message 'Test data has been reset.' is shown. |
| 2 | Open /Bookings/Search. |  | The 'Find a room' form is shown. |
| 3 | Enter the search criteria and select 'Search'. | Date: 2027-01-06; Start time: 10:00; Duration: 2; Attendees: 15 | Only Board Room is listed, at 1,200.00 SAR. |

Code touchpoints: `BookingsController.Search`, `AvailabilityService.FindAvailableRooms`, `PricingService.Quote`

## TC-014 Slot ending after 22:00 is rejected

Bookings must end by closing time.

- Requirements: REQ-06
- Persona: Customer Sara
- Priority: Medium; type: Negative
- Automated reference: SearchAndPricingTests.Booking_must_end_by_closing_time

Preconditions:

- Signed in as Customer Sara.

| # | Action | Test data | Expected result |
| --- | --- | --- | --- |
| 1 | Open /Dev and select 'Reset test data'. |  | The message 'Test data has been reset.' is shown. |
| 2 | Open /Bookings/Search. |  | The 'Find a room' form is shown. |
| 3 | Enter the search criteria and select 'Search'. | Date: 2027-01-06; Start time: 21:00; Duration: 2; Attendees: 4 | The message 'Bookings must start at 08:00 or later and end by 22:00.' is shown and no rooms are listed. |

Code touchpoints: `BookingsController.Search`, `AvailabilityService.ValidateSlot`

## TC-015 Slot in the past is rejected

Bookings must start after the current system time.

- Requirements: REQ-06
- Persona: Customer Sara
- Priority: Medium; type: Negative
- Automated reference: SearchAndPricingTests.Start_in_the_past_is_rejected

Preconditions:

- Signed in as Customer Sara.

| # | Action | Test data | Expected result |
| --- | --- | --- | --- |
| 1 | Open /Dev and select 'Reset test data'. |  | The message 'Test data has been reset.' is shown. |
| 2 | Open /Bookings/Search. |  | The 'Find a room' form is shown. |
| 3 | Enter the search criteria and select 'Search'. | Date: 2027-01-03; Start time: 10:00; Duration: 1; Attendees: 4 | The message 'The start time must be in the future.' is shown and no rooms are listed. |

Code touchpoints: `BookingsController.Search`, `AvailabilityService.ValidateSlot`

## TC-016 Customer books a room at peak price

A 2-hour weekday booking inside peak hours costs 1.5 times the rate.

- Requirements: REQ-07, REQ-08
- Persona: Customer Sara
- Priority: High; type: Functional
- Automated reference: BookingLifecycleTests.Customer_books_room_at_peak_price

Preconditions:

- Signed in as Customer Sara.

| # | Action | Test data | Expected result |
| --- | --- | --- | --- |
| 1 | Open /Dev and select 'Reset test data'. |  | The message 'Test data has been reset.' is shown. |
| 2 | Open /Bookings/Search, enter the search criteria and select 'Search'. | Date: 2027-01-07; Start time: 10:00; Duration: 2; Attendees: 6 | Al Waha is listed at 600.00 SAR with 2 / 0 peak / off-peak hours. |
| 3 | Select 'Book' for Al Waha. |  | The booking page shows 'Booking BK-1005 confirmed.' |
| 4 | Review the booking details. |  | Status Confirmed, room Al Waha (R-102), date 2027-01-07, time 10:00 - 12:00 (2 h), attendees 6, price 600.00 SAR. |

Code touchpoints: `BookingsController.Search`, `BookingsController.Create`, `BookingsController.Details`, `BookingService.Create`, `AvailabilityService.EnsureBookable`, `PricingService.Quote`, `InvoiceService.IssueForBooking`, `NotificationService.BookingConfirmed`

## TC-017 Weekend hours are off-peak

Friday has no peak hours.

- Requirements: REQ-08
- Persona: Customer Sara
- Priority: High; type: Functional
- Automated reference: SearchAndPricingTests.Weekend_is_off_peak

Preconditions:

- Signed in as Customer Sara.

| # | Action | Test data | Expected result |
| --- | --- | --- | --- |
| 1 | Open /Dev and select 'Reset test data'. |  | The message 'Test data has been reset.' is shown. |
| 2 | Open /Bookings/Search. |  | The 'Find a room' form is shown. |
| 3 | Enter the search criteria and select 'Search'. | Date: 2027-01-08; Start time: 10:00; Duration: 2; Attendees: 4 | Al Nakheel is listed at 200.00 SAR with 0 / 2 peak / off-peak hours. |

Code touchpoints: `BookingsController.Search`, `PricingService.Quote`, `PricingService.IsPeak`, `PricingPolicy.PeakDays`

## TC-018 Slot crossing the end of peak hours is priced per hour

16:00 to 18:00 on a weekday has one peak and one off-peak hour.

- Requirements: REQ-08
- Persona: Customer Sara
- Priority: High; type: Functional
- Automated reference: SearchAndPricingTests.Slot_crossing_peak_end_is_priced_per_hour

Preconditions:

- Signed in as Customer Sara.

| # | Action | Test data | Expected result |
| --- | --- | --- | --- |
| 1 | Open /Dev and select 'Reset test data'. |  | The message 'Test data has been reset.' is shown. |
| 2 | Open /Bookings/Search. |  | The 'Find a room' form is shown. |
| 3 | Enter the search criteria and select 'Search'. | Date: 2027-01-06; Start time: 16:00; Duration: 2; Attendees: 4 | Al Nakheel is listed at 250.00 SAR with 1 / 1 peak / off-peak hours. |

Code touchpoints: `BookingsController.Search`, `PricingService.Quote`, `PricingService.IsPeak`, `PricingPolicy.PeakMultiplier`, `PricingPolicy.PeakEnd`

## TC-019 Four-hour booking gets a 10% discount

The long-booking discount is shown in the search results.

- Requirements: REQ-09
- Persona: Customer Sara
- Priority: High; type: Functional
- Automated reference: SearchAndPricingTests.Four_hours_or_more_get_ten_percent_discount

Preconditions:

- Signed in as Customer Sara.

| # | Action | Test data | Expected result |
| --- | --- | --- | --- |
| 1 | Open /Dev and select 'Reset test data'. |  | The message 'Test data has been reset.' is shown. |
| 2 | Open /Bookings/Search. |  | The 'Find a room' form is shown. |
| 3 | Enter the search criteria and select 'Search'. | Date: 2027-01-07; Start time: 09:00; Duration: 4; Attendees: 4 | Al Nakheel is listed at 540.00 SAR with the note '(includes 10% discount of 60.00 SAR)' and 4 / 0 peak / off-peak hours. |

Code touchpoints: `BookingsController.Search`, `PricingService.Quote`, `PricingPolicy.LongBookingHours`, `PricingPolicy.LongBookingDiscountRate`

## TC-020 Evening long booking is off-peak and discounted

17:00 to 21:00 on Sunday has no peak hours and gets the long-booking discount.

- Requirements: REQ-08, REQ-09
- Persona: Customer Sara
- Priority: Medium; type: Functional
- Automated reference: SearchAndPricingTests.Evening_long_booking_is_off_peak_with_discount

Preconditions:

- Signed in as Customer Sara.

| # | Action | Test data | Expected result |
| --- | --- | --- | --- |
| 1 | Open /Dev and select 'Reset test data'. |  | The message 'Test data has been reset.' is shown. |
| 2 | Open /Bookings/Search. |  | The 'Find a room' form is shown. |
| 3 | Enter the search criteria and select 'Search'. | Date: 2027-01-10; Start time: 17:00; Duration: 4; Attendees: 6 | Al Waha is listed at 720.00 SAR with the note '(includes 10% discount of 80.00 SAR)' and 0 / 4 peak / off-peak hours. |

Code touchpoints: `BookingsController.Search`, `PricingService.Quote`, `PricingService.IsPeak`

## TC-021 Booking issues an invoice with discount and VAT

The booking invoice shows the room line, the discount line, subtotal, 15% VAT and total.

- Requirements: REQ-10, REQ-09
- Persona: Customer Sara
- Priority: High; type: Functional
- Automated reference: BookingLifecycleTests.Booking_issues_invoice_with_discount_and_vat

Preconditions:

- Signed in as Customer Sara.

| # | Action | Test data | Expected result |
| --- | --- | --- | --- |
| 1 | Open /Dev and select 'Reset test data'. |  | The message 'Test data has been reset.' is shown. |
| 2 | Open /Bookings/Search, enter the search criteria, select 'Search', then select 'Book' for Al Nakheel. | Date: 2027-01-07; Start time: 09:00; Duration: 4; Attendees: 4 | The booking page shows 'Booking BK-1005 confirmed.' and price 540.00 SAR. |
| 3 | Under Invoices on the booking page, open INV-5005. |  | Invoice INV-5005 has type Booking and status Issued. |
| 4 | Review the invoice lines and totals. |  | Lines: 'Al Nakheel, 4 h (4 peak, 0 off-peak)' 600.00 SAR and 'Long booking discount (10%)' -60.00 SAR. Subtotal 540.00 SAR, VAT (15%) 81.00 SAR, Total 621.00 SAR. |

Code touchpoints: `BookingsController.Create`, `BookingService.Create`, `PricingService.Quote`, `PricingService.CalculateVat`, `InvoiceService.IssueForBooking`, `InvoicesController.Details`

## TC-022 Existing invoice shows discount and VAT

A customer views a seeded invoice with a discount.

- Requirements: REQ-10
- Persona: Customer Omar
- Priority: Medium; type: Functional
- Automated reference: BookingLifecycleTests.Seeded_invoice_shows_discount_and_vat

Preconditions:

- Signed in as Customer Omar.

| # | Action | Test data | Expected result |
| --- | --- | --- | --- |
| 1 | Open /Dev and select 'Reset test data'. |  | The message 'Test data has been reset.' is shown. |
| 2 | Open /Invoices. |  | INV-5003 (BK-1003) and INV-5004 (BK-1004) are listed, both Booking and Issued. |
| 3 | Open INV-5003. |  | Lines: 'Board Room, 4 h (4 peak, 0 off-peak)' 2,400.00 SAR and 'Long booking discount (10%)' -240.00 SAR. Subtotal 2,160.00 SAR, VAT (15%) 324.00 SAR, Total 2,484.00 SAR. |

Code touchpoints: `InvoicesController.Index`, `InvoicesController.Details`, `InvoiceService.GetVisibleTo`, `InvoiceService.Get`

## TC-023 Cancelling more than 24 hours ahead is free

BK-1002 starts on 2027-01-06 13:00, more than 24 hours after the system time.

- Requirements: REQ-11
- Persona: Customer Sara
- Priority: High; type: Functional
- Automated reference: BookingLifecycleTests.Cancelling_more_than_24_hours_ahead_is_free

Preconditions:

- Signed in as Customer Sara.

| # | Action | Test data | Expected result |
| --- | --- | --- | --- |
| 1 | Open /Dev and select 'Reset test data'. |  | The message 'Test data has been reset.' is shown. |
| 2 | Open /Bookings/Details/BK-1002 and select 'Cancel booking'. |  | The cancellation page shows 'Cancellation fee: 0.00 SAR'. |
| 3 | Select 'Confirm cancellation'. |  | The message 'Booking BK-1002 cancelled. Cancellation fee: 0.00 SAR.' is shown, status is Cancelled and the cancellation fee is 0.00 SAR. |
| 4 | Open /Invoices. |  | INV-5002 has status Void. No cancellation fee invoice exists for BK-1002. |

Code touchpoints: `BookingsController.Cancel`, `BookingsController.CancelConfirmed`, `BookingService.PreviewCancellationFee`, `BookingService.Cancel`, `CancellationPolicy.CalculateFee`, `InvoiceService.VoidBookingInvoices`, `NotificationService.BookingCancelled`

## TC-024 Late cancellation charges 50% plus VAT

BK-1001 starts on 2027-01-04 10:00, two hours after the system time.

- Requirements: REQ-11, REQ-10
- Persona: Customer Sara
- Priority: High; type: Functional
- Automated reference: BookingLifecycleTests.Late_cancellation_charges_half_the_price

Preconditions:

- Signed in as Customer Sara.

| # | Action | Test data | Expected result |
| --- | --- | --- | --- |
| 1 | Open /Dev and select 'Reset test data'. |  | The message 'Test data has been reset.' is shown. |
| 2 | Open /Bookings/Details/BK-1001 and select 'Cancel booking'. |  | The cancellation page shows 'Cancellation fee: 150.00 SAR (plus VAT 22.50 SAR)'. |
| 3 | Select 'Confirm cancellation'. |  | The message 'Booking BK-1001 cancelled. Cancellation fee: 150.00 SAR.' is shown and status is Cancelled. |
| 4 | Open /Invoices. |  | INV-5001 has status Void. INV-5005 for BK-1001 has type Cancellation fee, status Issued and total 172.50 SAR. |

Code touchpoints: `BookingsController.Cancel`, `BookingsController.CancelConfirmed`, `BookingService.Cancel`, `CancellationPolicy.CalculateFee`, `InvoiceService.VoidBookingInvoices`, `InvoiceService.IssueCancellationFee`, `PricingService.CalculateVat`, `NotificationService.BookingCancelled`

## TC-025 Admin cancellation waives the fee

An admin cancels a booking that would otherwise incur a late fee.

- Requirements: REQ-15
- Persona: Admin
- Priority: Medium; type: Functional
- Automated reference: BookingLifecycleTests.Admin_cancellation_waives_the_fee

Preconditions:

- Signed in as Admin.

| # | Action | Test data | Expected result |
| --- | --- | --- | --- |
| 1 | Open /Dev and select 'Reset test data'. |  | The message 'Test data has been reset.' is shown. |
| 2 | Open /Bookings/Details/BK-1001 and select 'Cancel booking'. |  | The cancellation page shows 'Cancellation fee: 0.00 SAR'. |
| 3 | Select 'Confirm cancellation'. |  | The message 'Booking BK-1001 cancelled. Cancellation fee: 0.00 SAR.' is shown and status is Cancelled. |

Code touchpoints: `BookingsController.Cancel`, `BookingsController.CancelConfirmed`, `BookingService.Cancel`, `CancellationPolicy.CalculateFee`

## TC-026 Reschedule re-prices the booking and re-issues the invoice

Moving BK-1002 from a weekday peak slot to Friday makes it off-peak.

- Requirements: REQ-12, REQ-08, REQ-10
- Persona: Customer Sara
- Priority: High; type: Functional
- Automated reference: BookingLifecycleTests.Reschedule_reprices_and_reissues_invoice

Preconditions:

- Signed in as Customer Sara.

| # | Action | Test data | Expected result |
| --- | --- | --- | --- |
| 1 | Open /Dev and select 'Reset test data'. |  | The message 'Test data has been reset.' is shown. |
| 2 | Open /Bookings/Details/BK-1002 and select 'Reschedule'. |  | The reschedule form shows date 2027-01-06, start 13:00, duration 2. |
| 3 | Enter the new slot and select 'Save new time'. | New date: 2027-01-08; New start time: 10:00; Duration: 2 | The message 'Booking BK-1002 rescheduled. New price: 400.00 SAR.' is shown. Status Confirmed, date 2027-01-08, time 10:00 - 12:00. |
| 4 | Open /Invoices. |  | INV-5002 has status Void. INV-5005 for BK-1002 has type Booking, status Issued and total 460.00 SAR. |

Code touchpoints: `BookingsController.Reschedule`, `BookingService.Reschedule`, `AvailabilityService.EnsureBookable`, `PricingService.Quote`, `InvoiceService.VoidBookingInvoices`, `InvoiceService.IssueForBooking`, `NotificationService.BookingRescheduled`

## TC-027 Reschedule into a booked slot is rejected

Al Waha is booked by BK-1004 on 2027-01-07 14:00 - 16:00.

- Requirements: REQ-12, REQ-05
- Persona: Customer Sara
- Priority: High; type: Negative
- Automated reference: BookingLifecycleTests.Reschedule_into_a_booked_slot_is_rejected

Preconditions:

- Signed in as Customer Sara.

| # | Action | Test data | Expected result |
| --- | --- | --- | --- |
| 1 | Open /Dev and select 'Reset test data'. |  | The message 'Test data has been reset.' is shown. |
| 2 | Open /Bookings/Reschedule/BK-1002. |  | The reschedule form is shown. |
| 3 | Enter the new slot and select 'Save new time'. | New date: 2027-01-07; New start time: 15:00; Duration: 1 | The form stays open and shows 'Room Al Waha is already booked at that time.' |
| 4 | Open /Bookings/Details/BK-1002. |  | The booking is unchanged: date 2027-01-06, time 13:00 - 15:00, price 600.00 SAR. |

Code touchpoints: `BookingsController.Reschedule`, `BookingService.Reschedule`, `AvailabilityService.EnsureBookable`, `AvailabilityService.IsRoomFree`

## TC-028 Booking sends a confirmation notification

The unread count increases and the message is listed.

- Requirements: REQ-13
- Persona: Customer Sara
- Priority: Medium; type: Functional
- Automated reference: BookingLifecycleTests.Booking_sends_confirmation_notification

Preconditions:

- Signed in as Customer Sara.
- The header shows 'Notifications (0)'.

| # | Action | Test data | Expected result |
| --- | --- | --- | --- |
| 1 | Open /Dev and select 'Reset test data'. |  | The message 'Test data has been reset.' is shown. |
| 2 | Open /Bookings/Search, enter the search criteria, select 'Search', then select 'Book' for Al Waha. | Date: 2027-01-07; Start time: 10:00; Duration: 2; Attendees: 6 | 'Booking BK-1005 confirmed.' is shown and the header shows 'Notifications (1)'. |
| 3 | Select 'Notifications (1)' in the header. |  | The newest notification reads 'New: Booking BK-1005 confirmed: Al Waha on 2027-01-07 from 10:00 to 12:00.' |
| 4 | Open /Bookings. |  | The header shows 'Notifications (0)'. |

Code touchpoints: `BookingService.Create`, `NotificationService.BookingConfirmed`, `NotificationService.UnreadCount`, `NotificationService.MarkAllRead`, `NotificationsController.Index`, `_Layout`

## TC-029 Cancellation sends a notification with the fee

The cancellation message states the fee.

- Requirements: REQ-13
- Persona: Customer Sara
- Priority: Medium; type: Functional
- Automated reference: BookingLifecycleTests.Cancellation_sends_notification_with_fee

Preconditions:

- Signed in as Customer Sara.

| # | Action | Test data | Expected result |
| --- | --- | --- | --- |
| 1 | Open /Dev and select 'Reset test data'. |  | The message 'Test data has been reset.' is shown. |
| 2 | Open /Bookings/Cancel/BK-1002 and select 'Confirm cancellation'. |  | The message 'Booking BK-1002 cancelled. Cancellation fee: 0.00 SAR.' is shown. |
| 3 | Open /Notifications. |  | The newest notification reads 'New: Booking BK-1002 cancelled. Cancellation fee: 0.00 SAR.' |

Code touchpoints: `BookingService.Cancel`, `CancellationPolicy.CalculateFee`, `NotificationService.BookingCancelled`, `NotificationsController.Index`

## TC-030 Customer sees only their own bookings

My bookings lists the signed-in customer's bookings only.

- Requirements: REQ-14
- Persona: Customer Sara
- Priority: High; type: Security
- Automated reference: BookingLifecycleTests.Customer_sees_only_own_bookings

Preconditions:

- Signed in as Customer Sara.

| # | Action | Test data | Expected result |
| --- | --- | --- | --- |
| 1 | Open /Dev and select 'Reset test data'. |  | The message 'Test data has been reset.' is shown. |
| 2 | Open /Bookings. |  | The 'My bookings' table lists exactly BK-1001 (Al Nakheel, 2027-01-04) and BK-1002 (Al Waha, 2027-01-06). BK-1003 and BK-1004 are not listed. |

Code touchpoints: `BookingsController.Index`, `BookingService.GetVisibleTo`, `InMemoryBookingRepository.GetByCustomer`

## TC-031 Admin sees all bookings with customer names

All bookings lists every booking.

- Requirements: REQ-14
- Persona: Admin
- Priority: Medium; type: Functional
- Automated reference: BookingLifecycleTests.Admin_sees_all_bookings

Preconditions:

- Signed in as Admin.

| # | Action | Test data | Expected result |
| --- | --- | --- | --- |
| 1 | Open /Dev and select 'Reset test data'. |  | The message 'Test data has been reset.' is shown. |
| 2 | Open /Bookings. |  | The 'All bookings' table lists BK-1001 and BK-1002 for Sara Al-Harbi, and BK-1003 and BK-1004 for Omar Haddad. |

Code touchpoints: `BookingsController.Index`, `BookingService.GetVisibleTo`, `InMemoryBookingRepository.GetAll`

## TC-032 Customer cannot open another customer's booking

Direct links to other customers' bookings are not found.

- Requirements: REQ-14
- Persona: Customer Sara
- Priority: High; type: Security
- Automated reference: BookingLifecycleTests.Customer_cannot_open_another_customers_booking

Preconditions:

- Signed in as Customer Sara.
- BK-1003 belongs to Omar.

| # | Action | Test data | Expected result |
| --- | --- | --- | --- |
| 1 | Open /Dev and select 'Reset test data'. |  | The message 'Test data has been reset.' is shown. |
| 2 | Open /Bookings/Details/BK-1003. |  | The 'Not found' page shows 'Booking not found.' and no booking details are shown. |

Code touchpoints: `BookingsController.Details`, `BookingService.Get`
