using RoomBooking.Core.Facilities.Garden;

namespace RoomBooking.Web.Models.Facilities;

public sealed class GardenQuoteViewModel
{
    public required string ItemId { get; init; }
    public int Quantity { get; init; }
    public GardenItem? Item { get; set; }
    public GardenQuote? Quote { get; set; }
    public string? Error { get; set; }
}

public sealed class GardenReportViewModel
{
    public int Orders { get; init; }
    public decimal Revenue { get; init; }
}
