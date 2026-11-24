using System.Globalization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using RoomBooking.Core.Domain;
using RoomBooking.Core.Services;

namespace RoomBooking.Web.Models;

public sealed class LoginViewModel
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? ReturnUrl { get; set; }
}

public sealed class RoomFormViewModel
{
    public string? Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Capacity { get; set; } = 1;
    public decimal HourlyRate { get; set; } = 100m;
}

/// <summary>Date (yyyy-MM-dd) and start time (HH:mm) are strings so parsing is culture-invariant.</summary>
public class SlotForm
{
    public string Date { get; set; } = string.Empty;
    public string StartTime { get; set; } = "09:00";
    public int Hours { get; set; } = 1;

    public bool TryGetStart(out DateTime start) =>
        DateTime.TryParseExact(
            $"{Date} {StartTime}",
            "yyyy-MM-dd HH:mm",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out start);

    public static IReadOnlyList<string> StartTimes { get; } =
        Enumerable.Range(8, 14).Select(hour => $"{hour:00}:00").ToList();
}

public sealed class SearchViewModel : SlotForm
{
    public int Attendees { get; set; } = 1;
    public bool Submitted { get; set; }
    public string? Error { get; set; }
    [BindNever]
    public IReadOnlyList<SearchResult> Results { get; set; } = [];
}

public sealed record SearchResult(Room Room, PriceQuote Quote);

public sealed class CreateBookingForm : SlotForm
{
    public string RoomId { get; set; } = string.Empty;
    public int Attendees { get; set; } = 1;
}

public sealed class RescheduleViewModel : SlotForm
{
    [BindNever]
    public Booking? Booking { get; set; }
    [BindNever]
    public Room? Room { get; set; }
}

public sealed record BookingRow(Booking Booking, Room? Room, User? Customer);

public sealed record BookingDetailsViewModel(
    Booking Booking,
    Room? Room,
    User? Customer,
    IReadOnlyList<Invoice> Invoices,
    bool CanChange);

public sealed record CancelViewModel(Booking Booking, Room? Room, decimal Fee, decimal FeeVat);
