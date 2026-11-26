using RoomBooking.Core.Facilities.Lockers;

namespace RoomBooking.Web.Models.Facilities;

public sealed class LockersQuoteViewModel
{
    public required string ItemId { get; init; }
    public int Quantity { get; init; }
    public LockersItem? Item { get; set; }
    public LockersQuote? Quote { get; set; }
    public string? Error { get; set; }
}

public sealed class LockersReportViewModel
{
    public int Orders { get; init; }
    public decimal Revenue { get; init; }
}
