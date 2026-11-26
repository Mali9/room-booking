using RoomBooking.Core.Facilities.Concierge;

namespace RoomBooking.Web.Models.Facilities;

public sealed class ConciergeQuoteViewModel
{
    public required string ItemId { get; init; }
    public int Quantity { get; init; }
    public ConciergeItem? Item { get; set; }
    public ConciergeQuote? Quote { get; set; }
    public string? Error { get; set; }
}

public sealed class ConciergeReportViewModel
{
    public int Orders { get; init; }
    public decimal Revenue { get; init; }
}
