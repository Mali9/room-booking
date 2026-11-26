using RoomBooking.Core.Facilities.Translation;

namespace RoomBooking.Web.Models.Facilities;

public sealed class TranslationQuoteViewModel
{
    public required string ItemId { get; init; }
    public int Quantity { get; init; }
    public TranslationItem? Item { get; set; }
    public TranslationQuote? Quote { get; set; }
    public string? Error { get; set; }
}

public sealed class TranslationReportViewModel
{
    public int Orders { get; init; }
    public decimal Revenue { get; init; }
}
