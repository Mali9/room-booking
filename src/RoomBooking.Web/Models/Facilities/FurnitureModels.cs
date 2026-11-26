using RoomBooking.Core.Facilities.Furniture;

namespace RoomBooking.Web.Models.Facilities;

public sealed class FurnitureQuoteViewModel
{
    public required string ItemId { get; init; }
    public int Quantity { get; init; }
    public FurnitureItem? Item { get; set; }
    public FurnitureQuote? Quote { get; set; }
    public string? Error { get; set; }
}

public sealed class FurnitureReportViewModel
{
    public int Orders { get; init; }
    public decimal Revenue { get; init; }
}
