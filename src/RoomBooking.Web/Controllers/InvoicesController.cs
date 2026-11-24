using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoomBooking.Core;
using RoomBooking.Core.Services;

namespace RoomBooking.Web.Controllers;

[Authorize]
public sealed class InvoicesController(InvoiceService invoices) : AppController
{
    public IActionResult Index() => View(invoices.GetVisibleTo(CurrentUserId, IsAdmin));

    public IActionResult Details(string id)
    {
        try
        {
            return View(invoices.Get(id, CurrentUserId, IsAdmin));
        }
        catch (BusinessRuleException error)
        {
            Response.StatusCode = StatusCodes.Status404NotFound;
            return View("NotFound", error.Message);
        }
    }
}
