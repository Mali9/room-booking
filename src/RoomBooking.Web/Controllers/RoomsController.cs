using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoomBooking.Core;
using RoomBooking.Core.Services;
using RoomBooking.Web.Models;

namespace RoomBooking.Web.Controllers;

[Authorize]
public sealed class RoomsController(RoomService rooms) : AppController
{
    public IActionResult Index() => View(rooms.GetCatalogue(includeInactive: IsAdmin));

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult Create() => View("Edit", new RoomFormViewModel());

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult Create(RoomFormViewModel model)
    {
        try
        {
            var room = rooms.Create(model.Name, model.Capacity, model.HourlyRate);
            Success($"Room {room.Name} created with ID {room.Id}.");
            return RedirectToAction(nameof(Index));
        }
        catch (BusinessRuleException error)
        {
            ModelState.AddModelError(string.Empty, error.Message);
            return View("Edit", model);
        }
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult Edit(string id)
    {
        try
        {
            var room = rooms.Get(id);
            return View(new RoomFormViewModel
            {
                Id = room.Id,
                Name = room.Name,
                Capacity = room.Capacity,
                HourlyRate = room.HourlyRate,
            });
        }
        catch (BusinessRuleException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult Edit(string id, RoomFormViewModel model)
    {
        try
        {
            var room = rooms.Update(id, model.Name, model.Capacity, model.HourlyRate);
            Success($"Room {room.Name} updated.");
            return RedirectToAction(nameof(Index));
        }
        catch (BusinessRuleException error)
        {
            model.Id = id;
            ModelState.AddModelError(string.Empty, error.Message);
            return View(model);
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult SetActive(string id, bool active)
    {
        try
        {
            var room = rooms.SetActive(id, active);
            Success(active ? $"Room {room.Name} activated." : $"Room {room.Name} deactivated.");
        }
        catch (BusinessRuleException error)
        {
            Error(error.Message);
        }

        return RedirectToAction(nameof(Index));
    }
}
