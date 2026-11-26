using RoomBooking.Core.Facilities.Charging;

namespace RoomBooking.Web.Models.Facilities;

public sealed class ChargingQuoteViewModel
{
    public required string ItemId { get; init; }
    public int Quantity { get; init; }
    public ChargingItem? Item { get; set; }
    public ChargingQuote? Quote { get; set; }
    public string? Error { get; set; }
}

public sealed class ChargingReportViewModel
{
    public int Orders { get; init; }
    public decimal Revenue { get; init; }
}
