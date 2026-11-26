using RoomBooking.Core.Facilities.ItSupport;

namespace RoomBooking.Web.Models.Facilities;

public sealed class ItSupportQuoteViewModel
{
    public required string ItemId { get; init; }
    public int Quantity { get; init; }
    public ItSupportItem? Item { get; set; }
    public ItSupportQuote? Quote { get; set; }
    public string? Error { get; set; }
}

public sealed class ItSupportReportViewModel
{
    public int Orders { get; init; }
    public decimal Revenue { get; init; }
}
