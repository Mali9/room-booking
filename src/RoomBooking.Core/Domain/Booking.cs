namespace RoomBooking.Core.Domain;

public enum BookingStatus
{
    Confirmed,
    Cancelled,
}

public sealed class Booking
{
    public required string Id { get; init; }
    public required string RoomId { get; init; }
    public required string CustomerId { get; init; }
    public DateTime Start { get; set; }
    public int Hours { get; set; }
    public int Attendees { get; init; }
    public BookingStatus Status { get; set; } = BookingStatus.Confirmed;

    /// <summary>Booking price after discounts, excluding VAT.</summary>
    public decimal Price { get; set; }

    public decimal CancellationFee { get; set; }
    public DateTime CreatedAt { get; init; }
    public DateTime? CancelledAt { get; set; }

    public DateTime End => Start.AddHours(Hours);

    public bool Overlaps(DateTime start, DateTime end) => Start < end && start < End;
}
