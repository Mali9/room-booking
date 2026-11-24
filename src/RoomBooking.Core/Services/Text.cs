using System.Globalization;

namespace RoomBooking.Core.Services;

/// <summary>Invariant formatting shared by user-facing messages and pages.</summary>
public static class Text
{
    public static string Money(decimal amount) =>
        amount.ToString("N2", CultureInfo.InvariantCulture) + " SAR";

    public static string Date(DateTime value) =>
        value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

    public static string Time(DateTime value) =>
        value.ToString("HH:mm", CultureInfo.InvariantCulture);

    public static string DateTime(DateTime value) =>
        value.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
}
