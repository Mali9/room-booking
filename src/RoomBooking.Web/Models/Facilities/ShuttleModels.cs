using RoomBooking.Core.Facilities.Shuttle;

namespace RoomBooking.Web.Models.Facilities;

public sealed class ShuttleQuoteViewModel
{
    public required string ItemId { get; init; }
    public int Quantity { get; init; }
    public ShuttleItem? Item { get; set; }
    public ShuttleQuote? Quote { get; set; }
    public string? Error { get; set; }
}

public sealed class ShuttleReportViewModel
{
    public int Orders { get; init; }
    public decimal Revenue { get; init; }
}
