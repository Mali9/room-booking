using RoomBooking.Core.Facilities.Events;

namespace RoomBooking.Web.Models.Facilities;

public sealed class EventsQuoteViewModel
{
    public required string ItemId { get; init; }
    public int Quantity { get; init; }
    public EventsItem? Item { get; set; }
    public EventsQuote? Quote { get; set; }
    public string? Error { get; set; }
}

public sealed class EventsReportViewModel
{
    public int Orders { get; init; }
    public decimal Revenue { get; init; }
}
