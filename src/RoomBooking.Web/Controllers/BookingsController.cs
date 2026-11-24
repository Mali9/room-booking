using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoomBooking.Core;
using RoomBooking.Core.Abstractions;
using RoomBooking.Core.Services;
using RoomBooking.Web.Models;

namespace RoomBooking.Web.Controllers;

[Authorize]
public sealed class BookingsController(
    BookingService bookings,
    AvailabilityService availability,
    PricingService pricing,
    InvoiceService invoices,
    IRoomRepository rooms,
    IUserRepository users) : AppController
{
    public IActionResult Index()
    {
        var rows = bookings
            .GetVisibleTo(CurrentUserId, IsAdmin)
            .Select(booking => new BookingRow(
                booking,
                rooms.GetById(booking.RoomId),
                users.GetById(booking.CustomerId)))
            .ToList();
        return View(rows);
    }

    [HttpGet]
    [Authorize(Roles = "Customer")]
    public IActionResult Search(SearchViewModel model)
    {
        if (!model.Submitted)
        {
            return View(model);
        }

        if (!model.TryGetStart(out var start))
        {
            model.Error = "Enter a valid date and start time.";
            return View(model);
        }

        try
        {
            model.Results = availability
                .FindAvailableRooms(start, model.Hours, model.Attendees)
                .Select(room => new SearchResult(room, pricing.Quote(room, start, model.Hours)))
                .ToList();
        }
        catch (BusinessRuleException error)
        {
            model.Error = error.Message;
        }

        return View(model);
    }

    [HttpPost]
    [Authorize(Roles = "Customer")]
    public IActionResult Create(CreateBookingForm form)
    {
        if (!form.TryGetStart(out var start))
        {
            Error("Enter a valid date and start time.");
            return RedirectToAction(nameof(Search));
        }

        try
        {
            var booking = bookings.Create(new BookingRequest(
                CurrentUserId, form.RoomId, start, form.Hours, form.Attendees));
            Success($"Booking {booking.Id} confirmed.");
            return RedirectToAction(nameof(Details), new { id = booking.Id });
        }
        catch (BusinessRuleException error)
        {
            Error(error.Message);
            return RedirectToAction(nameof(Search), new
            {
                form.Date,
                form.StartTime,
                form.Hours,
                form.Attendees,
                Submitted = true,
            });
        }
    }

    public IActionResult Details(string id)
    {
        try
        {
            var booking = bookings.Get(id, CurrentUserId, IsAdmin);
            var related = invoices
                .GetVisibleTo(CurrentUserId, IsAdmin)
                .Where(invoice => invoice.BookingId == booking.Id)
                .ToList();
            return View(new BookingDetailsViewModel(
                booking,
                rooms.GetById(booking.RoomId),
                users.GetById(booking.CustomerId),
                related,
                bookings.CanChange(booking)));
        }
        catch (BusinessRuleException error)
        {
            return NotFoundPage(error.Message);
        }
    }

    [HttpGet]
    public IActionResult Cancel(string id)
    {
        try
        {
            var booking = bookings.Get(id, CurrentUserId, IsAdmin);
            var fee = bookings.PreviewCancellationFee(id, CurrentUserId, IsAdmin);
            return View(new CancelViewModel(
                booking, rooms.GetById(booking.RoomId), fee, pricing.CalculateVat(fee)));
        }
        catch (BusinessRuleException error)
        {
            Error(error.Message);
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [ActionName("Cancel")]
    public IActionResult CancelConfirmed(string id)
    {
        try
        {
            var booking = bookings.Cancel(id, CurrentUserId, IsAdmin);
            Success($"Booking {booking.Id} cancelled. Cancellation fee: {Text.Money(booking.CancellationFee)}.");
            return RedirectToAction(nameof(Details), new { id });
        }
        catch (BusinessRuleException error)
        {
            Error(error.Message);
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpGet]
    public IActionResult Reschedule(string id)
    {
        try
        {
            var booking = bookings.Get(id, CurrentUserId, IsAdmin);
            return View(new RescheduleViewModel
            {
                Booking = booking,
                Room = rooms.GetById(booking.RoomId),
                Date = Text.Date(booking.Start),
                StartTime = Text.Time(booking.Start),
                Hours = booking.Hours,
            });
        }
        catch (BusinessRuleException error)
        {
            return NotFoundPage(error.Message);
        }
    }

    [HttpPost]
    public IActionResult Reschedule(string id, RescheduleViewModel model)
    {
        try
        {
            if (!model.TryGetStart(out var start))
            {
                throw new BusinessRuleException(ErrorCodes.InvalidStartTime, "Enter a valid date and start time.");
            }

            var booking = bookings.Reschedule(id, CurrentUserId, IsAdmin, start, model.Hours);
            Success($"Booking {booking.Id} rescheduled. New price: {Text.Money(booking.Price)}.");
            return RedirectToAction(nameof(Details), new { id });
        }
        catch (BusinessRuleException error)
        {
            try
            {
                model.Booking = bookings.Get(id, CurrentUserId, IsAdmin);
                model.Room = rooms.GetById(model.Booking.RoomId);
            }
            catch (BusinessRuleException)
            {
                return NotFoundPage("Booking not found.");
            }

            ModelState.AddModelError(string.Empty, error.Message);
            return View(model);
        }
    }

    private ViewResult NotFoundPage(string message)
    {
        Response.StatusCode = StatusCodes.Status404NotFound;
        return View("NotFound", message);
    }
}
