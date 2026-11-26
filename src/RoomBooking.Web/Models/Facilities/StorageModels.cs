using RoomBooking.Core.Facilities.Storage;

namespace RoomBooking.Web.Models.Facilities;

public sealed class StorageQuoteViewModel
{
    public required string ItemId { get; init; }
    public int Quantity { get; init; }
    public StorageItem? Item { get; set; }
    public StorageQuote? Quote { get; set; }
    public string? Error { get; set; }
}

public sealed class StorageReportViewModel
{
    public int Orders { get; init; }
    public decimal Revenue { get; init; }
}
