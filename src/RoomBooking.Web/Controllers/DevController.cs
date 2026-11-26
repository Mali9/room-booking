using Microsoft.AspNetCore.Mvc;
using RoomBooking.Core.Facilities.Shared;
using RoomBooking.Infrastructure;

namespace RoomBooking.Web.Controllers;

/// <summary>
/// Test-environment helper: resets all data to the documented seed.
/// Enabled only when Testing:EnableReset is true. Never enable it in production.
/// </summary>
public sealed class DevController(
    InMemoryDataStore store,
    IEnumerable<IResettable> facilities,
    IConfiguration configuration) : AppController
{
    private bool Enabled => configuration.GetValue<bool>("Testing:EnableReset");

    [HttpGet]
    public IActionResult Index() => Enabled ? View() : NotFound();

    [HttpPost]
    [IgnoreAntiforgeryToken]
    public IActionResult Reset()
    {
        if (!Enabled)
        {
            return NotFound();
        }

        store.Reset();
        foreach (var facility in facilities)
        {
            facility.Reset();
        }

        Success("Test data has been reset.");
        return RedirectToAction(nameof(Index));
    }
}
