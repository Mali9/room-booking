using RoomBooking.Core.Facilities.Pantry;

namespace RoomBooking.Web.Models.Facilities;

public sealed class PantryQuoteViewModel
{
    public required string ItemId { get; init; }
    public int Quantity { get; init; }
    public PantryItem? Item { get; set; }
    public PantryQuote? Quote { get; set; }
    public string? Error { get; set; }
}

public sealed class PantryReportViewModel
{
    public int Orders { get; init; }
    public decimal Revenue { get; init; }
}
