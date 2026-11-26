using RoomBooking.Core.Facilities.Lounge;

namespace RoomBooking.Web.Models.Facilities;

public sealed class LoungeQuoteViewModel
{
    public required string ItemId { get; init; }
    public int Quantity { get; init; }
    public LoungeItem? Item { get; set; }
    public LoungeQuote? Quote { get; set; }
    public string? Error { get; set; }
}

public sealed class LoungeReportViewModel
{
    public int Orders { get; init; }
    public decimal Revenue { get; init; }
}
