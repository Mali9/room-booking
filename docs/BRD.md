# Business Requirements Document: RoomBooking

| Item | Value |
| --- | --- |
| Product | RoomBooking, meeting-room booking web application |
| Version | 1.0 |
| Status | Baseline for testing |
| Currency | Saudi Riyal (SAR) |

## 1. Purpose

RoomBooking lets employees and visitors (customers) find and book meeting rooms, and lets the
facilities team (administrators) manage the room catalogue and oversee all bookings. The system
prices each booking, issues invoices with VAT, applies a cancellation policy, and informs
customers of every change to their bookings.

## 2. Scope

In scope:

- Sign-in with role-based access (Customer, Admin).
- Room catalogue and room administration.
- Availability search, booking, cancellation and rescheduling.
- Pricing (peak and off-peak hours, long-booking discount), invoices with VAT.
- In-app notifications.

Out of scope: online payment, e-mail or SMS delivery, recurring bookings, calendar integration,
self-registration, and password reset.

## 3. Actors

| Actor | Description |
| --- | --- |
| Customer | Signs in, searches for rooms, books, cancels and reschedules their own bookings, views their own invoices and notifications. |
| Admin | Manages rooms, views all bookings and invoices, and can cancel any booking without a fee. Admins do not make bookings. |

## 4. Business rules

| ID | Rule |
| --- | --- |
| BR-01 | A room has a name (3 to 50 characters, unique, case-insensitive), a capacity (1 to 50 people) and an hourly rate (10 to 5,000 SAR). |
| BR-02 | An inactive room is hidden from customers and cannot be booked. A room with upcoming confirmed bookings cannot be deactivated. |
| BR-03 | A booking starts on the hour and lasts a whole number of hours, from 1 to 8. |
| BR-04 | A booking must start at 08:00 or later and end by 22:00 on the same day. |
| BR-05 | A booking must start in the future. |
| BR-06 | The number of attendees must not exceed the room capacity. |
| BR-07 | A room cannot have two confirmed bookings that overlap in time. Cancelled bookings free their slot. |
| BR-08 | Each booked hour is priced separately. Peak hours are Sunday to Thursday, 09:00 to 17:00, and cost 1.5 times the hourly rate. All other hours, including all of Friday and Saturday, cost the hourly rate. |
| BR-09 | Bookings of 4 hours or more receive a 10% discount on the booking price. |
| BR-10 | Prices shown when searching and booking exclude VAT. |
| BR-11 | Amounts are rounded to 2 decimal places. |
| BR-12 | Every confirmed booking produces a booking invoice. VAT is 15% of the invoice subtotal. |
| BR-13 | Cancelling 24 hours or more before the start is free. |
| BR-14 | Cancelling less than 24 hours before the start costs 50% of the booking price. The fee is invoiced separately, with VAT. |
| BR-15 | When a booking is cancelled or rescheduled, its booking invoice becomes Void. Rescheduling issues a new booking invoice at the new price. |
| BR-16 | Only upcoming confirmed bookings can be cancelled or rescheduled. |
| BR-17 | Rescheduling keeps the room and the number of attendees, and follows BR-03 to BR-09. |
| BR-18 | The customer receives an in-app notification when a booking is confirmed, cancelled or rescheduled. |
| BR-19 | Customers can only see their own bookings and invoices. Admins can see all of them. |
| BR-20 | An admin can cancel any upcoming booking. The cancellation fee is waived. |

## 5. Functional requirements

### REQ-01 Sign-in and roles

Users sign in with e-mail and password. The application shows who is signed in.

Acceptance criteria:

1. Valid credentials sign the user in and show "Signed in as <name>" in the header.
2. Invalid credentials show "Invalid email or password." and the user stays signed out.
3. A customer who opens an admin-only page sees "Access denied".

### REQ-02 Room catalogue

Signed-in users can view the list of rooms with ID, name, capacity and hourly rate.

Acceptance criteria:

1. Customers see active rooms only.
2. Admins see all rooms, with a Status column (Active or Inactive) and management actions.

### REQ-03 Room administration

Admins can add rooms and edit room name, capacity and hourly rate (BR-01).

Acceptance criteria:

1. A valid new room is created as Active with a system-assigned ID, and the message "Room <name> created with ID <id>." is shown.
2. Invalid values are rejected with a message naming the rule, for example "Capacity must be between 1 and 50."
3. A duplicate name is rejected with "A room named '<name>' already exists."
4. Saved changes appear in the room list.

### REQ-04 Room activation

Admins can deactivate and activate rooms (BR-02).

Acceptance criteria:

1. Deactivating a room with upcoming confirmed bookings is rejected with "Room <name> has upcoming bookings and cannot be deactivated."
2. Activating an inactive room shows "Room <name> activated." and sets its status to Active.

### REQ-05 Availability search

Customers search by date, start time, duration and number of attendees, and see available rooms
with the quoted price (BR-02, BR-06, BR-07, BR-10).

Acceptance criteria:

1. Results list only active rooms with enough capacity and no overlapping confirmed booking.
2. Results are ordered by hourly rate, lowest first.
3. Each result shows capacity, hourly rate, number of peak and off-peak hours, and the price excluding VAT.
4. If no room matches, "No rooms are available for this slot." is shown.

### REQ-06 Booking time rules

Slots must follow BR-03, BR-04 and BR-05.

Acceptance criteria:

1. A slot ending after 22:00 is rejected with "Bookings must start at 08:00 or later and end by 22:00."
2. A slot in the past is rejected with "The start time must be in the future."

### REQ-07 Create a booking

A customer books a room from the search results.

Acceptance criteria:

1. The booking is confirmed immediately and the message "Booking <id> confirmed." is shown.
2. The booking details page shows status Confirmed, room, date, time, attendees and price.

### REQ-08 Peak and off-peak pricing

The price follows BR-08.

Acceptance criteria:

1. A booking wholly inside peak hours costs hours x rate x 1.5.
2. A booking wholly outside peak hours costs hours x rate.
3. A booking that crosses the peak boundary is priced hour by hour.

### REQ-09 Long-booking discount

Bookings of 4 hours or more receive the BR-09 discount.

Acceptance criteria:

1. The search result shows the discounted price and the discount amount.
2. The invoice shows the discount as a separate negative line.

### REQ-10 Invoices

Invoices follow BR-12 and BR-15.

Acceptance criteria:

1. Booking a room issues an invoice with the room line, any discount line, subtotal, VAT (15%) and total.
2. The invoice list shows invoice ID, booking, type (Booking or Cancellation fee), issue time, status and total.

### REQ-11 Cancel a booking

Customers can cancel their own upcoming bookings (BR-13, BR-14, BR-15, BR-16).

Acceptance criteria:

1. Before confirming, the cancellation page shows the fee that will apply.
2. After cancellation, the booking status is Cancelled and the message "Booking <id> cancelled. Cancellation fee: <amount> SAR." is shown.
3. The booking invoice becomes Void. A late cancellation issues a Cancellation fee invoice with VAT.

### REQ-12 Reschedule a booking

Customers can move their own upcoming bookings to another date or time (BR-15, BR-16, BR-17).

Acceptance criteria:

1. A valid new slot updates the booking, re-prices it and shows "Booking <id> rescheduled. New price: <amount> SAR."
2. The old booking invoice becomes Void and a new booking invoice is issued.
3. A slot that overlaps another confirmed booking of the same room is rejected with "Room <name> is already booked at that time." and the booking is unchanged.

### REQ-13 Notifications

Customers are notified of booking changes (BR-18).

Acceptance criteria:

1. The header shows the number of unread notifications, for example "Notifications (1)".
2. The notifications page lists messages, newest first, and marks them as read.
3. Message formats:
   - "Booking <id> confirmed: <room> on <yyyy-MM-dd> from <HH:mm> to <HH:mm>."
   - "Booking <id> cancelled. Cancellation fee: <amount> SAR."
   - "Booking <id> moved from <yyyy-MM-dd HH:mm> to <yyyy-MM-dd HH:mm> (<room>)."

### REQ-14 Booking visibility

Bookings are visible according to BR-19.

Acceptance criteria:

1. A customer's "My bookings" lists only their own bookings.
2. An admin's "All bookings" lists every booking with the customer's name.
3. A customer who opens another customer's booking sees "Booking not found."

### REQ-15 Admin cancellation

Admins can cancel any upcoming booking without a fee (BR-20).

Acceptance criteria:

1. When an admin cancels a booking, the fee shown and charged is 0.00 SAR, regardless of notice.

## 6. Non-functional requirements

| ID | Requirement |
| --- | --- |
| NFR-01 | Pages are usable with a keyboard and screen reader: form fields have labels, messages use status or alert roles. |
| NFR-02 | All amounts are shown with two decimals and the SAR suffix, for example "1,200.00 SAR". |
| NFR-03 | Dates use yyyy-MM-dd and times use 24-hour HH:mm. |
| NFR-04 | Test environments run with a fixed system time and resettable seed data (see test-data.md). |

## 7. Glossary

| Term | Meaning |
| --- | --- |
| Peak hour | An hour starting Sunday to Thursday between 09:00 and 16:00 inclusive. |
| Booking price | Price after any discount, excluding VAT. |
| Upcoming booking | A confirmed booking whose start time is later than the current system time. |
