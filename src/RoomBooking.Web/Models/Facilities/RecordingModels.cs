using RoomBooking.Core.Facilities.Recording;

namespace RoomBooking.Web.Models.Facilities;

public sealed class RecordingQuoteViewModel
{
    public required string ItemId { get; init; }
    public int Quantity { get; init; }
    public RecordingItem? Item { get; set; }
    public RecordingQuote? Quote { get; set; }
    public string? Error { get; set; }
}

public sealed class RecordingReportViewModel
{
    public int Orders { get; init; }
    public decimal Revenue { get; init; }
}
