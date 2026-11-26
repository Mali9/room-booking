using RoomBooking.Core.Facilities.Studio;

namespace RoomBooking.Web.Models.Facilities;

public sealed class StudioQuoteViewModel
{
    public required string ItemId { get; init; }
    public int Quantity { get; init; }
    public StudioItem? Item { get; set; }
    public StudioQuote? Quote { get; set; }
    public string? Error { get; set; }
}

public sealed class StudioReportViewModel
{
    public int Orders { get; init; }
    public decimal Revenue { get; init; }
}
