using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace RoomBooking.Web.Controllers;

public abstract class AppController : Controller
{
    protected string CurrentUserId =>
        User.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? throw new InvalidOperationException("No signed-in user.");

    protected bool IsAdmin => User.IsInRole("Admin");

    protected void Success(string message) => TempData["Success"] = message;

    protected void Error(string message) => TempData["Error"] = message;
}
