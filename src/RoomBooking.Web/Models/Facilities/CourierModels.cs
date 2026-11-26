using RoomBooking.Core.Facilities.Courier;

namespace RoomBooking.Web.Models.Facilities;

public sealed class CourierQuoteViewModel
{
    public required string ItemId { get; init; }
    public int Quantity { get; init; }
    public CourierItem? Item { get; set; }
    public CourierQuote? Quote { get; set; }
    public string? Error { get; set; }
}

public sealed class CourierReportViewModel
{
    public int Orders { get; init; }
    public decimal Revenue { get; init; }
}
