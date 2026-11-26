using RoomBooking.Core.Facilities.Wellness;

namespace RoomBooking.Web.Models.Facilities;

public sealed class WellnessQuoteViewModel
{
    public required string ItemId { get; init; }
    public int Quantity { get; init; }
    public WellnessItem? Item { get; set; }
    public WellnessQuote? Quote { get; set; }
    public string? Error { get; set; }
}

public sealed class WellnessReportViewModel
{
    public int Orders { get; init; }
    public decimal Revenue { get; init; }
}
