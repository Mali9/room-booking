using RoomBooking.Core.Facilities.Training;

namespace RoomBooking.Web.Models.Facilities;

public sealed class TrainingQuoteViewModel
{
    public required string ItemId { get; init; }
    public int Quantity { get; init; }
    public TrainingItem? Item { get; set; }
    public TrainingQuote? Quote { get; set; }
    public string? Error { get; set; }
}

public sealed class TrainingReportViewModel
{
    public int Orders { get; init; }
    public decimal Revenue { get; init; }
}
