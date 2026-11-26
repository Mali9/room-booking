using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoomBooking.Core.Facilities.Lockers;
using RoomBooking.Core.Facilities.Shared;
using RoomBooking.Web.Models.Facilities;

namespace RoomBooking.Web.Controllers.Facilities;

[Authorize]
public sealed class LockersController(LockersService service, LockersReport report) : AppController
{
    public IActionResult Index() => View(service.ListItems());

    [HttpGet]
    public IActionResult Quote(string itemId, int quantity = 1)
    {
        var model = new LockersQuoteViewModel { ItemId = itemId, Quantity = quantity };
        try
        {
            (model.Item, model.Quote) = service.Quote(itemId, quantity);
        }
        catch (FacilityRuleException error)
        {
            model.Item = service.ListItems().FirstOrDefault(item => item.Id == itemId);
            model.Error = error.Message;
        }

        return View(model);
    }

    [HttpPost]
    public IActionResult Place(string itemId, int quantity)
    {
        try
        {
            Success(service.Place(CurrentUserId, itemId, quantity));
            return RedirectToAction(nameof(Orders));
        }
        catch (FacilityRuleException error)
        {
            Error(error.Message);
            return RedirectToAction(nameof(Quote), new { itemId, quantity });
        }
    }

    public IActionResult Orders() => View(service.GetOrders(CurrentUserId));

    public IActionResult Report()
    {
        var (orders, revenue) = report.Summarize();
        return View(new LockersReportViewModel { Orders = orders, Revenue = revenue });
    }
}
