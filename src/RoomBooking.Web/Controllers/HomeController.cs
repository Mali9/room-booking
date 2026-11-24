using Microsoft.AspNetCore.Mvc;

namespace RoomBooking.Web.Controllers;

public sealed class HomeController : AppController
{
    public IActionResult Index() => View();

    public IActionResult Error() => View();
}
