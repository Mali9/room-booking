using Microsoft.AspNetCore.Mvc;

namespace RoomBooking.Web.Controllers;

/// <summary>A plain-text help page at /Help. Nothing else in the app links to it yet.</summary>
public sealed class HelpController : Controller
{
    public IActionResult Index() =>
        Content(
            "RoomBooking help\n\n"
            + "Search: pick a date, start time, duration and attendees, then choose a room.\n"
            + "Cancel: open a booking and choose Cancel. Late cancellations have a fee.\n"
            + "Invoices: every booking has an invoice under Invoices.",
            "text/plain");
}
