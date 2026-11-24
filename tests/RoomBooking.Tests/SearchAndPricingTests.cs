using RoomBooking.Core;
using static RoomBooking.Tests.TestApp;

namespace RoomBooking.Tests;

public sealed class SearchAndPricingTests
{
    private readonly TestApp _app = new();

    private (string Name, decimal Price)[] Search(DateTime start, int hours, int attendees) =>
        _app.Availability
            .FindAvailableRooms(start, hours, attendees)
            .Select(room => (room.Name, _app.Pricing.Quote(room, start, hours).Total))
            .ToArray();

    [Fact]
    [Trait("TestCase", "TC-012")]
    [Trait("Requirement", "REQ-05")]
    public void Search_excludes_booked_and_inactive_rooms()
    {
        var results = Search(At(4, 10), 1, 4);

        Assert.Equal([("Al Waha", 300m), ("Board Room", 600m)], results);
    }

    [Fact]
    [Trait("TestCase", "TC-013")]
    [Trait("Requirement", "REQ-05")]
    public void Search_filters_by_capacity()
    {
        var results = Search(At(6, 10), 2, 15);

        Assert.Equal([("Board Room", 1200m)], results);
    }

    [Fact]
    [Trait("TestCase", "TC-014")]
    [Trait("Requirement", "REQ-06")]
    public void Booking_must_end_by_closing_time()
    {
        var error = Assert.Throws<BusinessRuleException>(() => Search(At(6, 21), 2, 4));

        Assert.Equal("Bookings must start at 08:00 or later and end by 22:00.", error.Message);
    }

    [Fact]
    [Trait("TestCase", "TC-015")]
    [Trait("Requirement", "REQ-06")]
    public void Start_in_the_past_is_rejected()
    {
        var error = Assert.Throws<BusinessRuleException>(() => Search(At(3, 10), 1, 4));

        Assert.Equal("The start time must be in the future.", error.Message);
    }

    [Fact]
    [Trait("TestCase", "TC-017")]
    [Trait("Requirement", "REQ-08")]
    public void Weekend_is_off_peak()
    {
        var quote = _app.Pricing.Quote(_app.Rooms.GetById("R-101")!, At(8, 10), 2);

        Assert.Equal(0, quote.PeakHours);
        Assert.Equal(2, quote.OffPeakHours);
        Assert.Equal(200m, quote.Total);
    }

    [Fact]
    [Trait("TestCase", "TC-018")]
    [Trait("Requirement", "REQ-08")]
    public void Slot_crossing_peak_end_is_priced_per_hour()
    {
        var quote = _app.Pricing.Quote(_app.Rooms.GetById("R-101")!, At(6, 16), 2);

        Assert.Equal(1, quote.PeakHours);
        Assert.Equal(1, quote.OffPeakHours);
        Assert.Equal(250m, quote.Total);
    }

    [Fact]
    [Trait("TestCase", "TC-019")]
    [Trait("Requirement", "REQ-09")]
    public void Four_hours_or_more_get_ten_percent_discount()
    {
        var quote = _app.Pricing.Quote(_app.Rooms.GetById("R-101")!, At(7, 9), 4);

        Assert.Equal(600m, quote.Gross);
        Assert.Equal(60m, quote.Discount);
        Assert.Equal(540m, quote.Total);
    }

    [Fact]
    [Trait("TestCase", "TC-020")]
    [Trait("Requirement", "REQ-09")]
    public void Evening_long_booking_is_off_peak_with_discount()
    {
        var results = Search(At(10, 17), 4, 6);

        Assert.Contains(("Al Waha", 720m), results);
    }
}
