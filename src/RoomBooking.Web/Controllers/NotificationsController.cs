using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoomBooking.Core.Services;

namespace RoomBooking.Web.Controllers;

[Authorize]
public sealed class NotificationsController(NotificationService notifications) : AppController
{
    public IActionResult Index()
    {
        var items = notifications.GetForUser(CurrentUserId);
        // Snapshot "unread" before marking, so the page can highlight new items.
        ViewBag.UnreadIds = items.Where(item => !item.IsRead).Select(item => item.Id).ToHashSet();
        notifications.MarkAllRead(CurrentUserId);
        return View(items);
    }
}
