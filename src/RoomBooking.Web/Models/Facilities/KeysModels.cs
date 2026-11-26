using RoomBooking.Core.Facilities.Keys;

namespace RoomBooking.Web.Models.Facilities;

public sealed class KeysQuoteViewModel
{
    public required string ItemId { get; init; }
    public int Quantity { get; init; }
    public KeysItem? Item { get; set; }
    public KeysQuote? Quote { get; set; }
    public string? Error { get; set; }
}

public sealed class KeysReportViewModel
{
    public int Orders { get; init; }
    public decimal Revenue { get; init; }
}
