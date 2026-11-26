using RoomBooking.Core.Facilities.Badges;

namespace RoomBooking.Web.Models.Facilities;

public sealed class BadgesQuoteViewModel
{
    public required string ItemId { get; init; }
    public int Quantity { get; init; }
    public BadgesItem? Item { get; set; }
    public BadgesQuote? Quote { get; set; }
    public string? Error { get; set; }
}

public sealed class BadgesReportViewModel
{
    public int Orders { get; init; }
    public decimal Revenue { get; init; }
}
