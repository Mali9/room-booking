using RoomBooking.Core.Facilities.Kitchen;

namespace RoomBooking.Web.Models.Facilities;

public sealed class KitchenQuoteViewModel
{
    public required string ItemId { get; init; }
    public int Quantity { get; init; }
    public KitchenItem? Item { get; set; }
    public KitchenQuote? Quote { get; set; }
    public string? Error { get; set; }
}

public sealed class KitchenReportViewModel
{
    public int Orders { get; init; }
    public decimal Revenue { get; init; }
}
