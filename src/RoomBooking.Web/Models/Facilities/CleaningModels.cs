using RoomBooking.Core.Facilities.Cleaning;

namespace RoomBooking.Web.Models.Facilities;

public sealed class CleaningQuoteViewModel
{
    public required string ItemId { get; init; }
    public int Quantity { get; init; }
    public CleaningItem? Item { get; set; }
    public CleaningQuote? Quote { get; set; }
    public string? Error { get; set; }
}

public sealed class CleaningReportViewModel
{
    public int Orders { get; init; }
    public decimal Revenue { get; init; }
}
