# Test data

Test environments run with a fixed clock and in-memory seed data. Every test case starts from this
state. Reset it with the **Reset test data** button at `/Dev`, or `POST /Dev/Reset`.

## System time

Monday **2027-01-04 08:00** (`Clock:FixedNow`). The footer of every page shows it.

| Date | Day | Peak hours |
| --- | --- | --- |
| 2027-01-04 | Monday | 09:00 to 17:00 |
| 2027-01-05 | Tuesday | 09:00 to 17:00 |
| 2027-01-06 | Wednesday | 09:00 to 17:00 |
| 2027-01-07 | Thursday | 09:00 to 17:00 |
| 2027-01-08 | Friday | none |
| 2027-01-09 | Saturday | none |
| 2027-01-10 | Sunday | 09:00 to 17:00 |

## Users (personas)

| Persona | E-mail | Password | Role | Display name |
| --- | --- | --- | --- | --- |
| Admin | admin@roombooking.test | Admin#2027 | Admin | Facilities Admin |
| Customer Sara | sara@roombooking.test | Sara#2027 | Customer | Sara Al-Harbi |
| Customer Omar | omar@roombooking.test | Omar#2027 | Customer | Omar Haddad |

These are disposable test accounts that exist only in this sample application.

## Rooms

| ID | Name | Capacity | Hourly rate | Status |
| --- | --- | --- | --- | --- |
| R-101 | Al Nakheel | 6 | 100.00 SAR | Active |
| R-102 | Al Waha | 12 | 200.00 SAR | Active |
| R-201 | Board Room | 20 | 400.00 SAR | Active |
| R-301 | Focus Pod | 2 | 50.00 SAR | Inactive |

## Bookings

| ID | Customer | Room | Date | Time | Attendees | Price | Invoice (total) |
| --- | --- | --- | --- | --- | --- | --- | --- |
| BK-1001 | Sara | Al Nakheel | 2027-01-04 | 10:00 - 12:00 | 4 | 300.00 | INV-5001 (345.00) |
| BK-1002 | Sara | Al Waha | 2027-01-06 | 13:00 - 15:00 | 8 | 600.00 | INV-5002 (690.00) |
| BK-1003 | Omar | Board Room | 2027-01-05 | 09:00 - 13:00 | 15 | 2,160.00 | INV-5003 (2,484.00) |
| BK-1004 | Omar | Al Waha | 2027-01-07 | 14:00 - 16:00 | 10 | 600.00 | INV-5004 (690.00) |

All seeded bookings are Confirmed. Seeded notifications are already read, so every customer starts
with "Notifications (0)".

## Next identifiers

The next room is **R-401**, the next booking is **BK-1005**, and the next invoice is **INV-5005**.
