using RoomBooking.Core.Facilities.Parking;

namespace RoomBooking.Web.Models.Facilities;

public sealed class ParkingQuoteViewModel
{
    public required string ItemId { get; init; }
    public int Quantity { get; init; }
    public ParkingItem? Item { get; set; }
    public ParkingQuote? Quote { get; set; }
    public string? Error { get; set; }
}

public sealed class ParkingReportViewModel
{
    public int Orders { get; init; }
    public decimal Revenue { get; init; }
}
