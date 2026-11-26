using RoomBooking.Core.Facilities.Gym;

namespace RoomBooking.Web.Models.Facilities;

public sealed class GymQuoteViewModel
{
    public required string ItemId { get; init; }
    public int Quantity { get; init; }
    public GymItem? Item { get; set; }
    public GymQuote? Quote { get; set; }
    public string? Error { get; set; }
}

public sealed class GymReportViewModel
{
    public int Orders { get; init; }
    public decimal Revenue { get; init; }
}
