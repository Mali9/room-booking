using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Web.Controllers.Facilities;

[Authorize]
public sealed class FacilitiesController : AppController
{
    public IActionResult Index() => View(FacilityCatalog.All);
}
