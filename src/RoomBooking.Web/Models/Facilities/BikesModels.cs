using RoomBooking.Core.Facilities.Bikes;

namespace RoomBooking.Web.Models.Facilities;

public sealed class BikesQuoteViewModel
{
    public required string ItemId { get; init; }
    public int Quantity { get; init; }
    public BikesItem? Item { get; set; }
    public BikesQuote? Quote { get; set; }
    public string? Error { get; set; }
}

public sealed class BikesReportViewModel
{
    public int Orders { get; init; }
    public decimal Revenue { get; init; }
}
