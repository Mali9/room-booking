using RoomBooking.Core.Facilities.Signage;

namespace RoomBooking.Web.Models.Facilities;

public sealed class SignageQuoteViewModel
{
    public required string ItemId { get; init; }
    public int Quantity { get; init; }
    public SignageItem? Item { get; set; }
    public SignageQuote? Quote { get; set; }
    public string? Error { get; set; }
}

public sealed class SignageReportViewModel
{
    public int Orders { get; init; }
    public decimal Revenue { get; init; }
}
