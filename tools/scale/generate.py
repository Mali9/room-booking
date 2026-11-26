"""Grow RoomBooking into a large application for change-impact performance tests.

Adds 40 "facility" modules on top of the room-booking app. Every module is real, runnable
code: a domain, a pricing policy, a validator, a service, a report, an in-memory repository
with seed data, an MVC controller with four Razor pages, and xUnit tests. Modules share two
hub classes (FacilityTax and FacilityAudit), and every fourth module reads a fee from the
previous module's policy, so changes travel across modules.

For each module it also writes 10 browser test cases with exact expected values, the code each
test case runs (its links), and, for every change scenario, which test cases must rerun and
which are expected to fail. Expected values are computed here with the same arithmetic as the
C# code (decimal, rounded half away from zero).

Usage (from the repository root):
    python tools/scale/generate.py modules            # write the modules, docs and test cases
    python tools/scale/generate.py scenario <name>    # apply one change scenario to the tree
    python tools/scale/generate.py list               # list scenarios
"""

from __future__ import annotations

import json
import shutil
import sys
from dataclasses import dataclass
from decimal import ROUND_HALF_UP, Decimal
from pathlib import Path

ROOT = Path(__file__).resolve().parents[2]
CORE = ROOT / "src/RoomBooking.Core/Facilities"
INFRA = ROOT / "src/RoomBooking.Infrastructure/Facilities"
WEB = ROOT / "src/RoomBooking.Web"
TESTS = ROOT / "tests/RoomBooking.Tests/Facilities"
DOCS = ROOT / "docs/scale"

NAMES = [
    "Catering", "Parking", "Equipment", "Visitors", "Cleaning", "Maintenance", "Security",
    "Lockers", "Desks", "Events", "Printing", "Shuttle", "Lounge", "Gym", "Childcare",
    "Laundry", "Mailroom", "Storage", "Wellness", "Library", "Labs", "Studio", "Kitchen",
    "Garden", "Charging", "Bikes", "Courier", "Translation", "ItSupport", "Furniture",
    "Signage", "Photography", "Recording", "Training", "Concierge", "Pantry", "Uniforms",
    "Keys", "Badges", "Archive",
]
VAT = Decimal("0.15")
NEW_VAT = Decimal("0.16")
# The first test case IDs are TC-1001 ... TC-1400, ten per module.
AI_SAMPLE = range(30, 35)  # modules left unlinked so the AI links them on first use
DELETED = ["Lockers", "Keys", "Uniforms"]
WIDE = range(10, 40)  # modules edited by the wide refactor scenario
SMALL = "Parking"  # own pricing multiplier changes
CROSS = "Equipment"  # its handling fee changes; Visitors reads it


def money(value: Decimal) -> Decimal:
    return value.quantize(Decimal("0.01"), rounding=ROUND_HALF_UP)


def fmt(value: Decimal) -> str:
    return f"{money(value):,.2f} SAR"


@dataclass
class Module:
    index: int
    name: str
    code: str

    @property
    def multiplier(self) -> Decimal:
        return [Decimal("1.00"), Decimal("1.10"), Decimal("1.20"), Decimal("1.25"), Decimal("1.50")][
            self.index % 5
        ]

    @property
    def bulk_threshold(self) -> int:
        return 5 + self.index % 4

    @property
    def bulk_rate(self) -> Decimal:
        return [Decimal("0.05"), Decimal("0.10"), Decimal("0.15")][self.index % 3]

    @property
    def max_quantity(self) -> int:
        return 20 + (self.index % 5) * 5

    @property
    def handling_fee(self) -> Decimal:
        return Decimal("10.00") + Decimal("2.50") * (self.index % 7)

    @property
    def dependency(self) -> Module | None:
        return MODULES[self.index - 1] if self.index % 4 == 3 else None

    def items(self) -> list[tuple[str, str, int, int]]:
        """(id, name, unit price, capacity)."""
        return [
            (f"{self.code}-A", f"{self.name} Standard", 40 + 5 * self.index, 50),
            (f"{self.code}-B", f"{self.name} Premium", 120 + 3 * self.index, 30),
            (f"{self.code}-C", f"{self.name} Limited", 75 + self.index, 3),
        ]

    def quote(self, key: str, quantity: int, *, vat=VAT, multiplier=None, fee=None):
        _, _, unit, _ = self.items()[{"A": 0, "B": 1, "C": 2}[key]]
        base = Decimal(unit) * quantity * (multiplier if multiplier is not None else self.multiplier)
        discount = money(base * self.bulk_rate) if quantity >= self.bulk_threshold else Decimal(0)
        handling = Decimal(0)
        if self.dependency is not None:
            handling = fee if fee is not None else self.dependency.handling_fee
        subtotal = base - discount + handling
        total = money(subtotal * (1 + vat))
        return {"base": money(base), "discount": discount, "fee": handling, "total": total}


def make_modules() -> list[Module]:
    modules, used = [], set()
    for index, name in enumerate(NAMES):
        code = name[:3].upper()
        while code in used:
            code = name[:2].upper() + str(index % 10)
        used.add(code)
        modules.append(Module(index, name, code))
    return modules


MODULES = make_modules()


def write(path: Path, text: str) -> None:
    path.parent.mkdir(parents=True, exist_ok=True)
    path.write_text(text.lstrip("\n"), encoding="utf-8")


def fill(template: str, m: Module, **extra: str) -> str:
    values = {
        "__M__": m.name,
        "__PFX__": m.code,
        "__MULT__": f"{m.multiplier}m",
        "__THRESHOLD__": str(m.bulk_threshold),
        "__RATE__": f"{m.bulk_rate}m",
        "__MAX__": str(m.max_quantity),
        "__FEE__": f"{m.handling_fee}m",
        **extra,
    }
    for key, value in values.items():
        template = template.replace(key, value)
    return template


# ------------------------------------------------------------------------------ C# sources

SHARED = {
    "Shared/IResettable.cs": """
namespace RoomBooking.Core.Facilities.Shared;

/// <summary>A store that returns to its seed on test reset.</summary>
public interface IResettable
{
    void Reset();
}
""",
    "Shared/Money.cs": """
namespace RoomBooking.Core.Facilities.Shared;

/// <summary>Money rounding shared by every facility: two decimals, half away from zero.</summary>
public static class FacilityMoney
{
    public static decimal Round(decimal amount) =>
        Math.Round(amount, 2, MidpointRounding.AwayFromZero);
}
""",
    "Shared/FacilityTax.cs": """
namespace RoomBooking.Core.Facilities.Shared;

/// <summary>VAT added to every facility price. Every facility pricing class uses it.</summary>
public sealed class FacilityTax
{
    public decimal VatRate { get; init; } = 0.15m;

    public decimal Apply(decimal subtotal) => FacilityMoney.Round(subtotal * (1 + VatRate));
}
""",
    "Shared/FacilityAudit.cs": """
namespace RoomBooking.Core.Facilities.Shared;

/// <summary>Records what happened in every facility, for operations.</summary>
public sealed class FacilityAudit
{
    private readonly List<string> _entries = [];

    public IReadOnlyList<string> Entries => _entries;

    public void Record(string entry)
    {
        lock (_entries)
        {
            _entries.Add(entry);
        }
    }
}
""",
    "Shared/FacilityRuleException.cs": """
namespace RoomBooking.Core.Facilities.Shared;

/// <summary>A facility rule rejected the request. The message is safe to show.</summary>
public sealed class FacilityRuleException(string message) : Exception(message);
""",
}

CORE_FILES = {
    "__M__Item.cs": """
namespace RoomBooking.Core.Facilities.__M__;

/// <summary>Something customers can order from __M__.</summary>
public sealed class __M__Item
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public decimal UnitPrice { get; init; }
    public int Capacity { get; init; }
}
""",
    "__M__Order.cs": """
namespace RoomBooking.Core.Facilities.__M__;

/// <summary>One __M__ order placed by a customer.</summary>
public sealed class __M__Order
{
    public required string Id { get; init; }
    public required string CustomerId { get; init; }
    public required string ItemId { get; init; }
    public int Quantity { get; init; }
    public decimal Total { get; init; }
}
""",
    "__M__Quote.cs": """
namespace RoomBooking.Core.Facilities.__M__;

/// <summary>The price of an order before it is placed.</summary>
public sealed record __M__Quote(decimal Base, decimal Discount, decimal HandlingFee, decimal Total);
""",
    "__M__Policy.cs": """
namespace RoomBooking.Core.Facilities.__M__;

/// <summary>Pricing and quantity rules of __M__.</summary>
public sealed class __M__Policy
{
    /// <summary>Unit prices are multiplied by this.</summary>
    public decimal Multiplier { get; init; } = __MULT__;

    /// <summary>Orders of at least this many units get the bulk discount.</summary>
    public int BulkThreshold { get; init; } = __THRESHOLD__;

    public decimal BulkDiscountRate { get; init; } = __RATE__;

    public int MaxQuantity { get; init; } = __MAX__;

    /// <summary>Fee other facilities add when an order involves __M__.</summary>
    public decimal HandlingFee { get; init; } = __FEE__;
}
""",
    "I__M__Repository.cs": """
namespace RoomBooking.Core.Facilities.__M__;

public interface I__M__Repository
{
    IReadOnlyList<__M__Item> Items();

    __M__Item? Find(string itemId);

    IReadOnlyList<__M__Order> OrdersFor(string customerId);

    IReadOnlyList<__M__Order> AllOrders();

    string NextOrderId();

    void Add(__M__Order order);
}
""",
    "__M__Validator.cs": """
using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.__M__;

/// <summary>Quantity rules of __M__ orders.</summary>
public sealed class __M__Validator(__M__Policy policy)
{
    public void Validate(__M__Item item, int quantity)
    {
        if (quantity < 1)
        {
            throw new FacilityRuleException("Quantity must be at least 1.");
        }

        if (quantity > policy.MaxQuantity)
        {
            throw new FacilityRuleException($"Quantity cannot exceed {policy.MaxQuantity}.");
        }

        if (quantity > item.Capacity)
        {
            throw new FacilityRuleException($"Only {item.Capacity} available for {item.Name}.");
        }
    }
}
""",
    "__M__Messages.cs": """
using RoomBooking.Core.Services;

namespace RoomBooking.Core.Facilities.__M__;

/// <summary>Texts shown to customers of __M__.</summary>
public static class __M__Messages
{
    public static string OrderPlaced(string orderId, decimal total) =>
        $"Order {orderId} placed. Total: {Text.Money(total)}.";
}
""",
    "__M__Service.cs": """
using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.__M__;

/// <summary>Lists, quotes and places __M__ orders.</summary>
public sealed class __M__Service(
    I__M__Repository repository,
    __M__Validator validator,
    __M__Pricing pricing,
    FacilityAudit audit)
{
    public IReadOnlyList<__M__Item> ListItems() => repository.Items();

    public (__M__Item Item, __M__Quote Quote) Quote(string itemId, int quantity)
    {
        var item = Find(itemId);
        validator.Validate(item, quantity);
        return (item, pricing.Quote(item, quantity));
    }

    public string Place(string customerId, string itemId, int quantity)
    {
        var item = Find(itemId);
        validator.Validate(item, quantity);
        var quote = pricing.Quote(item, quantity);
        var order = new __M__Order
        {
            Id = repository.NextOrderId(),
            CustomerId = customerId,
            ItemId = item.Id,
            Quantity = quantity,
            Total = quote.Total,
        };
        repository.Add(order);
        audit.Record($"__M__ order {order.Id} by {customerId}");
        return __M__Messages.OrderPlaced(order.Id, order.Total);
    }

    public IReadOnlyList<__M__Order> GetOrders(string customerId) => repository.OrdersFor(customerId);

    public __M__Item Find(string itemId) =>
        repository.Find(itemId) ?? throw new FacilityRuleException($"Item {itemId} was not found.");
}
""",
    "__M__Report.cs": """
namespace RoomBooking.Core.Facilities.__M__;

/// <summary>Revenue of __M__, priced with today's rules.</summary>
public sealed class __M__Report(I__M__Repository repository, __M__Pricing pricing)
{
    public (int Orders, decimal Revenue) Summarize()
    {
        var orders = repository.AllOrders();
        var revenue = 0m;
        foreach (var order in orders)
        {
            var item = repository.Find(order.ItemId);
            if (item is not null)
            {
                revenue += pricing.Quote(item, order.Quantity).Total;
            }
        }

        return (orders.Count, revenue);
    }
}
""",
}

PRICING = """
using RoomBooking.Core.Facilities.Shared;
__DEP_USING__
namespace RoomBooking.Core.Facilities.__M__;

/// <summary>Prices __M__ orders: multiplier, bulk discount__DEP_DOC__, then VAT.</summary>
public sealed class __M__Pricing(__M__Policy policy, FacilityTax tax__DEP_PARAM__)
{
    public __M__Quote Quote(__M__Item item, int quantity)
    {
        var baseAmount = item.UnitPrice * quantity * policy.Multiplier;
        var discount = quantity >= policy.BulkThreshold
            ? FacilityMoney.Round(baseAmount * policy.BulkDiscountRate)
            : 0m;
        var handlingFee = __FEE_EXPR__;
        var total = tax.Apply(baseAmount - discount + handlingFee);
        return new __M__Quote(FacilityMoney.Round(baseAmount), discount, handlingFee, total);
    }
}
"""

INFRA_FILES = {
    "__M__Seed.cs": """
using RoomBooking.Core.Facilities.__M__;

namespace RoomBooking.Infrastructure.Facilities.__M__;

/// <summary>Seed data of __M__: three items and two orders of Customer Sara.</summary>
public static class __M__Seed
{
    public static List<__M__Item> Items() =>
    [
__ITEMS__
    ];

    public static List<__M__Order> Orders() =>
    [
__ORDERS__
    ];
}
""",
    "InMemory__M__Repository.cs": """
using RoomBooking.Core.Facilities.__M__;
using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Infrastructure.Facilities.__M__;

public sealed class InMemory__M__Repository : I__M__Repository, IResettable
{
    private readonly object _sync = new();
    private List<__M__Item> _items = [];
    private List<__M__Order> _orders = [];
    private int _next;

    public InMemory__M__Repository() => Reset();

    public void Reset()
    {
        lock (_sync)
        {
            _items = __M__Seed.Items();
            _orders = __M__Seed.Orders();
            _next = 1001 + _orders.Count;
        }
    }

    public IReadOnlyList<__M__Item> Items()
    {
        lock (_sync)
        {
            return _items.ToList();
        }
    }

    public __M__Item? Find(string itemId)
    {
        lock (_sync)
        {
            return _items.FirstOrDefault(item => item.Id == itemId);
        }
    }

    public IReadOnlyList<__M__Order> OrdersFor(string customerId)
    {
        lock (_sync)
        {
            return _orders.Where(order => order.CustomerId == customerId).OrderBy(order => order.Id).ToList();
        }
    }

    public IReadOnlyList<__M__Order> AllOrders()
    {
        lock (_sync)
        {
            return _orders.ToList();
        }
    }

    public string NextOrderId()
    {
        lock (_sync)
        {
            return $"__PFX__-{_next++}";
        }
    }

    public void Add(__M__Order order)
    {
        lock (_sync)
        {
            _orders.Add(order);
        }
    }
}
""",
}

CONTROLLER = """
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoomBooking.Core.Facilities.__M__;
using RoomBooking.Core.Facilities.Shared;
using RoomBooking.Web.Models.Facilities;

namespace RoomBooking.Web.Controllers.Facilities;

[Authorize]
public sealed class __M__Controller(__M__Service service, __M__Report report) : AppController
{
    public IActionResult Index() => View(service.ListItems());

    [HttpGet]
    public IActionResult Quote(string itemId, int quantity = 1)
    {
        var model = new __M__QuoteViewModel { ItemId = itemId, Quantity = quantity };
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
        return View(new __M__ReportViewModel { Orders = orders, Revenue = revenue });
    }
}
"""

MODELS = """
using RoomBooking.Core.Facilities.__M__;

namespace RoomBooking.Web.Models.Facilities;

public sealed class __M__QuoteViewModel
{
    public required string ItemId { get; init; }
    public int Quantity { get; init; }
    public __M__Item? Item { get; set; }
    public __M__Quote? Quote { get; set; }
    public string? Error { get; set; }
}

public sealed class __M__ReportViewModel
{
    public int Orders { get; init; }
    public decimal Revenue { get; init; }
}
"""

VIEWS = {
    "Index.cshtml": """
@using RoomBooking.Core.Facilities.__M__
@model IReadOnlyList<__M__Item>
@{
    ViewData["Title"] = "__M__";
}
<h1>__M__</h1>
<p><a href="/__M__/Orders">My __M__ orders</a> · <a href="/__M__/Report">__M__ report</a></p>
<table id="items">
    <thead>
    <tr><th>Item</th><th>Unit price</th><th>Available</th><th></th></tr>
    </thead>
    <tbody>
    @foreach (var item in Model)
    {
        <tr>
            <td>@item.Name</td>
            <td>@Text.Money(item.UnitPrice)</td>
            <td>@item.Capacity</td>
            <td><a href="/__M__/Quote?itemId=@item.Id&quantity=1">Order</a></td>
        </tr>
    }
    </tbody>
</table>
""",
    "Quote.cshtml": """
@using RoomBooking.Web.Models.Facilities
@model __M__QuoteViewModel
@{
    ViewData["Title"] = "__M__ quote";
}
<h1>__M__ quote</h1>
@if (Model.Error is not null)
{
    <div class="alert error" role="alert">@Model.Error</div>
}
@if (Model.Item is not null)
{
    <p>Item: <strong>@Model.Item.Name</strong></p>
}
@if (Model.Quote is not null)
{
    <dl id="quote">
        <dt>Quantity</dt><dd>@Model.Quantity</dd>
        <dt>Base</dt><dd>@Text.Money(Model.Quote.Base)</dd>
        <dt>Discount</dt><dd>@Text.Money(Model.Quote.Discount)</dd>
        @if (Model.Quote.HandlingFee > 0)
        {
            <dt>Handling fee</dt><dd>@Text.Money(Model.Quote.HandlingFee)</dd>
        }
        <dt>Total (incl. VAT)</dt><dd>@Text.Money(Model.Quote.Total)</dd>
    </dl>
}
<form method="post" asp-controller="__M__" asp-action="Place">
    <input type="hidden" name="itemId" value="@Model.ItemId" />
    <label for="quantity">Quantity</label>
    <input id="quantity" name="quantity" type="number" value="@Model.Quantity" />
    <button type="submit">Place order</button>
</form>
""",
    "Orders.cshtml": """
@using RoomBooking.Core.Facilities.__M__
@model IReadOnlyList<__M__Order>
@{
    ViewData["Title"] = "My __M__ orders";
}
<h1>My __M__ orders</h1>
@if (Model.Count == 0)
{
    <p>No orders yet.</p>
}
else
{
    <table id="orders">
        <thead>
        <tr><th>Order</th><th>Item</th><th>Quantity</th><th>Total (incl. VAT)</th></tr>
        </thead>
        <tbody>
        @foreach (var order in Model)
        {
            <tr>
                <td>@order.Id</td>
                <td>@order.ItemId</td>
                <td>@order.Quantity</td>
                <td>@Text.Money(order.Total)</td>
            </tr>
        }
        </tbody>
    </table>
}
""",
    "Report.cshtml": """
@using RoomBooking.Web.Models.Facilities
@model __M__ReportViewModel
@{
    ViewData["Title"] = "__M__ report";
}
<h1>__M__ report</h1>
<p id="summary">Orders: @Model.Orders. Revenue: @Text.Money(Model.Revenue).</p>
""",
}

UNIT_TEST = """
using RoomBooking.Core.Facilities.__M__;
using RoomBooking.Core.Facilities.Shared;
__DEP_USING__
using Xunit;

namespace RoomBooking.Tests.Facilities;

public sealed class __M__Tests
{
    private static __M__Pricing Pricing() => new(new __M__Policy(), new FacilityTax()__DEP_ARG__);

    private static readonly __M__Item Premium = new()
    {
        Id = "__PFX__-B", Name = "__M__ Premium", UnitPrice = __UNIT_B__m, Capacity = 30,
    };

    [Fact]
    [Trait("TestCase", "__TC_QUOTE__")]
    public void Premium_quote_adds_vat() =>
        Assert.Equal(__TOTAL_B1__m, Pricing().Quote(Premium, 1).Total);

    [Fact]
    [Trait("TestCase", "__TC_ZERO__")]
    public void Zero_quantity_is_rejected()
    {
        var error = Assert.Throws<FacilityRuleException>(
            () => new __M__Validator(new __M__Policy()).Validate(Premium, 0));
        Assert.Equal("Quantity must be at least 1.", error.Message);
    }
}
"""


def tc(module: Module, number: int) -> str:
    return f"TC-{1000 + module.index * 10 + number:04d}"


def write_module(m: Module) -> None:
    base = CORE / m.name
    for name, template in CORE_FILES.items():
        write(base / name.replace("__M__", m.name), fill(template, m))
    dep = m.dependency
    write(
        base / f"{m.name}Pricing.cs",
        fill(
            PRICING,
            m,
            __DEP_USING__=f"using RoomBooking.Core.Facilities.{dep.name};\n" if dep else "",
            __DEP_DOC__=f", the {dep.name} handling fee" if dep else "",
            __DEP_PARAM__=f", {dep.name}Policy {dep.name[0].lower()}{dep.name[1:]}Policy" if dep else "",
            __FEE_EXPR__=f"{dep.name[0].lower()}{dep.name[1:]}Policy.HandlingFee" if dep else "0m",
        ),
    )
    items = "\n".join(
        f'        new() {{ Id = "{item_id}", Name = "{name}", UnitPrice = {unit}m, Capacity = {cap} }},'
        for item_id, name, unit, cap in m.items()
    )
    seeded = [("A", 2), ("B", 1)]
    orders = "\n".join(
        f'        new() {{ Id = "{m.code}-{1001 + i}", CustomerId = "U-2", ItemId = "{m.code}-{key}", '
        f'Quantity = {qty}, Total = {m.quote(key, qty)["total"]}m }},'
        for i, (key, qty) in enumerate(seeded)
    )
    for name, template in INFRA_FILES.items():
        write(
            INFRA / m.name / name.replace("__M__", m.name),
            fill(template, m, __ITEMS__=items, __ORDERS__=orders),
        )
    write(WEB / f"Controllers/Facilities/{m.name}Controller.cs", fill(CONTROLLER, m))
    write(WEB / f"Models/Facilities/{m.name}Models.cs", fill(MODELS, m))
    for name, template in VIEWS.items():
        write(WEB / f"Views/{m.name}/{name}", fill(template, m))
    write(
        TESTS / f"{m.name}Tests.cs",
        fill(
            UNIT_TEST,
            m,
            __DEP_USING__=f"using RoomBooking.Core.Facilities.{dep.name};" if dep else "",
            __DEP_ARG__=f", new {dep.name}Policy()" if dep else "",
            __UNIT_B__=str(m.items()[1][2]),
            __TOTAL_B1__=str(m.quote("B", 1)["total"]),
            __TC_QUOTE__=tc(m, 2),
            __TC_ZERO__=tc(m, 5),
        ),
    )


def write_shared(modules: list[Module]) -> None:
    for name, text in SHARED.items():
        write(CORE / name, text)
    registrations = []
    for m in modules:
        registrations += [
            f"        services.AddSingleton<RoomBooking.Core.Facilities.{m.name}.{m.name}Policy>();",
            f"        services.AddSingleton<RoomBooking.Core.Facilities.{m.name}.{m.name}Pricing>();",
            f"        services.AddSingleton<RoomBooking.Core.Facilities.{m.name}.{m.name}Validator>();",
            f"        services.AddSingleton<RoomBooking.Core.Facilities.{m.name}.{m.name}Service>();",
            f"        services.AddSingleton<RoomBooking.Core.Facilities.{m.name}.{m.name}Report>();",
            f"        services.AddSingleton<RoomBooking.Infrastructure.Facilities.{m.name}.InMemory{m.name}Repository>();",
            f"        services.AddSingleton<RoomBooking.Core.Facilities.{m.name}.I{m.name}Repository>("
            f"provider => provider.GetRequiredService<RoomBooking.Infrastructure.Facilities.{m.name}.InMemory{m.name}Repository>());",
            f"        services.AddSingleton<IResettable>("
            f"provider => provider.GetRequiredService<RoomBooking.Infrastructure.Facilities.{m.name}.InMemory{m.name}Repository>());",
        ]
    write(
        WEB / "FacilityRegistration.cs",
        "using RoomBooking.Core.Facilities.Shared;\n\nnamespace RoomBooking.Web;\n\n"
        "/// <summary>Registers every facility module. Generated by tools/scale/generate.py.</summary>\n"
        "public static class FacilityRegistration\n{\n"
        "    public static IServiceCollection AddFacilities(this IServiceCollection services)\n    {\n"
        "        services.AddSingleton<FacilityTax>();\n"
        "        services.AddSingleton<FacilityAudit>();\n"
        + "\n".join(registrations)
        + "\n        return services;\n    }\n}\n",
    )
    catalog = "\n".join(f'        ("{m.name}", "/{m.name}"),' for m in modules)
    write(
        CORE / "Shared/FacilityCatalog.cs",
        "namespace RoomBooking.Core.Facilities.Shared;\n\n"
        "/// <summary>Every facility and the page that opens it.</summary>\n"
        "public static class FacilityCatalog\n{\n"
        "    public static IReadOnlyList<(string Name, string Path)> All { get; } =\n    [\n"
        + catalog
        + "\n    ];\n}\n",
    )
    write(
        WEB / "Controllers/Facilities/FacilitiesController.cs",
        """
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Web.Controllers.Facilities;

[Authorize]
public sealed class FacilitiesController : AppController
{
    public IActionResult Index() => View(FacilityCatalog.All);
}
""",
    )
    write(
        WEB / "Views/Facilities/Index.cshtml",
        """
@model IReadOnlyList<(string Name, string Path)>
@{
    ViewData["Title"] = "Facilities";
}
<h1>Facilities</h1>
<ul id="facilities">
    @foreach (var facility in Model)
    {
        <li><a href="@facility.Path">@facility.Name</a></li>
    }
</ul>
""",
    )


def wire_app() -> None:
    """Hook the modules into the existing app: DI, test reset and the navigation."""

    registration = WEB / "ServiceRegistration.cs"
    text = registration.read_text(encoding="utf-8")
    if "AddFacilities()" not in text:
        text = text.replace(
            "        services.AddSingleton<AuthService>();\n",
            "        services.AddSingleton<AuthService>();\n        services.AddFacilities();\n",
        )
        registration.write_text(text, encoding="utf-8")
    dev = WEB / "Controllers/DevController.cs"
    text = dev.read_text(encoding="utf-8")
    if "IResettable" not in text:
        text = text.replace(
            "using RoomBooking.Infrastructure;",
            "using RoomBooking.Core.Facilities.Shared;\nusing RoomBooking.Infrastructure;",
        )
        text = text.replace(
            "public sealed class DevController(InMemoryDataStore store, IConfiguration configuration)",
            "public sealed class DevController(\n    InMemoryDataStore store,\n"
            "    IEnumerable<IResettable> facilities,\n    IConfiguration configuration)",
        )
        text = text.replace(
            "        store.Reset();\n",
            "        store.Reset();\n        foreach (var facility in facilities)\n        {\n"
            "            facility.Reset();\n        }\n\n",
        )
        dev.write_text(text, encoding="utf-8")
    layout = WEB / "Views/Shared/_Layout.cshtml"
    text = layout.read_text(encoding="utf-8")
    if 'href="/Facilities"' not in text:
        text = text.replace(
            '            <a href="/Invoices">Invoices</a>\n',
            '            <a href="/Invoices">Invoices</a>\n            <a href="/Facilities">Facilities</a>\n',
        )
        layout.write_text(text, encoding="utf-8")


# ------------------------------------------------------------------------------ test cases


def test_cases(m: Module) -> list[dict]:
    a, b, c = m.items()
    quote_b1 = m.quote("B", 1)
    bulk = m.quote("A", m.bulk_threshold)
    place_a2 = m.quote("A", 2)
    report_revenue = m.quote("A", 2)["total"] + m.quote("B", 1)["total"]
    new_id = f"{m.code}-1003"
    C, S, P = f"{m.name}Controller", f"{m.name}Service", f"{m.name}Pricing"
    fee_line = (
        f" Handling fee is {fmt(quote_b1['fee'])}." if quote_b1["fee"] else ""
    )

    def case(n, title, steps, touchpoints, kind="Functional"):
        return {
            "id": tc(m, n),
            "title": f"{m.name}: {title}",
            "description": f"{m.name} facility. {title}.",
            "requirements": [],
            "persona": "Customer Sara",
            "start_unauthenticated": False,
            "priority": "Medium",
            "type": kind,
            "preconditions": ["Signed in as Customer Sara."],
            "steps": steps,
            "code_touchpoints": touchpoints,
            "module": m.name,
            "automated_test": None,
        }

    open_quote = lambda key, q: {  # noqa: E731
        "action": f"Open /{m.name}/Quote?itemId={m.code}-{key}&quantity={q}.",
        "expected": f"The {m.name} quote page opens.",
    }
    enter_place = lambda q, expected: {  # noqa: E731
        "action": f"Enter {q} in 'Quantity' and select 'Place order'.",
        "expected": expected,
    }
    return [
        case(1, "items list shows prices and availability", [
            {"action": f"Open /{m.name}.",
             "expected": f"The items table lists {a[1]} at {fmt(Decimal(a[2]))} with {a[3]} available, "
                         f"{b[1]} at {fmt(Decimal(b[2]))} with {b[3]} available and {c[1]} at "
                         f"{fmt(Decimal(c[2]))} with {c[3]} available."},
        ], [f"{C}.Index", f"{S}.ListItems", f"InMemory{m.name}Repository.Items"]),
        case(2, "quote for one premium item", [
            {"action": f"Open /{m.name}/Quote?itemId={m.code}-B&quantity=1.",
             "expected": f"The quote shows Base {fmt(quote_b1['base'])}, Discount 0.00 SAR "
                         f"and Total (incl. VAT) {fmt(quote_b1['total'])}.{fee_line}"},
        ], [f"{C}.Quote", f"{S}.Quote", f"{P}.Quote", f"{m.name}Policy.Multiplier", "FacilityTax.Apply"]),
        case(3, "bulk quote applies the discount", [
            {"action": f"Open /{m.name}/Quote?itemId={m.code}-A&quantity={m.bulk_threshold}.",
             "expected": f"The quote shows Base {fmt(bulk['base'])}, Discount {fmt(bulk['discount'])} "
                         f"and Total (incl. VAT) {fmt(bulk['total'])}."},
        ], [f"{C}.Quote", f"{S}.Quote", f"{P}.Quote", f"{m.name}Policy.BulkDiscountRate",
            "FacilityTax.Apply"]),
        case(4, "placing an order confirms the total", [
            open_quote("A", 2),
            enter_place(2, f"The message 'Order {new_id} placed. Total: {fmt(place_a2['total'])}.' is shown."),
        ], [f"{C}.Quote", f"{C}.Place", f"{S}.Place", f"{P}.Quote", f"{m.name}Messages.OrderPlaced",
            "FacilityAudit.Record"]),
        case(5, "zero quantity is rejected", [
            open_quote("A", 1),
            enter_place(0, "The message 'Quantity must be at least 1.' is shown."),
        ], [f"{C}.Quote", f"{C}.Place", f"{S}.Place", f"{m.name}Validator.Validate"]),
        case(6, "quantity above the maximum is rejected", [
            open_quote("A", 1),
            enter_place(m.max_quantity + 1,
                        f"The message 'Quantity cannot exceed {m.max_quantity}.' is shown."),
        ], [f"{C}.Quote", f"{C}.Place", f"{S}.Place", f"{m.name}Validator.Validate",
            f"{m.name}Policy.MaxQuantity"]),
        case(7, "quantity above availability is rejected", [
            open_quote("C", 1),
            enter_place(4, f"The message 'Only 3 available for {c[1]}.' is shown."),
        ], [f"{C}.Quote", f"{C}.Place", f"{S}.Place", f"{m.name}Validator.Validate"]),
        case(8, "a new order appears in my orders", [
            open_quote("B", 1),
            enter_place(1, f"My {m.name} orders page opens."),
            {"action": "Read the orders table.",
             "expected": f"It lists {new_id} for {m.code}-B, quantity 1, total {fmt(quote_b1['total'])}."},
        ], [f"{C}.Quote", f"{C}.Place", f"{S}.Place", f"{C}.Orders", f"{S}.GetOrders"]),
        case(9, "report shows revenue at today's prices", [
            {"action": f"Open /{m.name}/Report.",
             "expected": f"The page shows 'Orders: 2. Revenue: {fmt(report_revenue)}.'"},
        ], [f"{C}.Report", f"{m.name}Report.Summarize", f"{P}.Quote"]),
        case(10, "my orders lists the seeded orders", [
            {"action": f"Open /{m.name}/Orders.",
             "expected": f"The orders table lists {m.code}-1001 for {m.code}-A with quantity 2 and "
                         f"{m.code}-1002 for {m.code}-B with quantity 1."},
        ], [f"{C}.Orders", f"{S}.GetOrders", f"InMemory{m.name}Repository.OrdersFor"]),
    ]


# ------------------------------------------------------------------------------ scenarios
# Each scenario is applied on top of the modules commit, on its own branch.


@dataclass
class Scenario:
    key: str
    branch: str
    commit: str
    description: str


SCENARIOS = [
    Scenario("hub", "scale/hub-vat-16", "change(scale): HUB - FacilityTax VAT 15% to 16% (every facility price; 40 unit tests updated)",
             "One line in a class every facility uses: the VAT rate goes from 15% to 16%."),
    Scenario("wide", "scale/wide-refactor-150-files",
             "change(scale): WIDE - edit 150 files in 30 modules (messages, validators, repositories, report pages, unit tests)",
             "A broad refactor: order message and validation wording, a repository rewrite with the same "
             "behavior, and report page headings, in 30 modules."),
    Scenario("small", "scale/small-parking-multiplier",
             "change(scale): SMALL - Parking price multiplier 1.10 to 1.30 (one module)",
             "One number in one module."),
    Scenario("cross", "scale/cross-equipment-fee",
             "change(scale): CROSS - Equipment handling fee 15.00 to 25.00 (read by Visitors)",
             "A fee in one module that another module adds to its prices."),
    Scenario("delete", "scale/delete-3-modules",
             "change(scale): DELETE - remove Lockers, Keys and Uniforms modules (57 files)",
             "Three modules removed entirely."),
]


def by_name(name: str) -> Module:
    return next(m for m in MODULES if m.name == name)


def replace(path: Path, old: str, new: str) -> None:
    text = path.read_text(encoding="utf-8")
    if old not in text:
        raise SystemExit(f"{path}: text not found: {old[:60]}")
    path.write_text(text.replace(old, new), encoding="utf-8")


def apply_scenario(key: str) -> None:
    if key == "hub":
        replace(CORE / "Shared/FacilityTax.cs", "VatRate { get; init; } = 0.15m", "VatRate { get; init; } = 0.16m")
    elif key == "wide":
        for m in (MODULES[i] for i in WIDE):
            replace(CORE / m.name / f"{m.name}Messages.cs", "placed. Total:", "confirmed. Total:")
            replace(CORE / m.name / f"{m.name}Validator.cs", "Quantity must be at least 1.",
                    "Quantity must be 1 or more.")
            replace(INFRA / m.name / f"InMemory{m.name}Repository.cs",
                    "return _orders.Where(order => order.CustomerId == customerId).OrderBy(order => order.Id).ToList();",
                    "var mine = _orders.Where(order => order.CustomerId == customerId);\n"
                    "            return [.. mine.OrderBy(order => order.Id)];")
            replace(WEB / f"Views/{m.name}/Report.cshtml", f"<h1>{m.name} report</h1>",
                    f"<h1>{m.name} revenue report</h1>")
            replace(TESTS / f"{m.name}Tests.cs", '"Quantity must be at least 1."', '"Quantity must be 1 or more."')
    elif key == "small":
        m = by_name(SMALL)
        replace(CORE / m.name / f"{m.name}Policy.cs", f"Multiplier {{ get; init; }} = {m.multiplier}m",
                "Multiplier { get; init; } = 1.30m")
        _fix_unit_test(m, multiplier=Decimal("1.30"))
    elif key == "cross":
        m = by_name(CROSS)
        replace(CORE / m.name / f"{m.name}Policy.cs", f"HandlingFee {{ get; init; }} = {m.handling_fee}m",
                "HandlingFee { get; init; } = 25.00m")
        dependent = next(x for x in MODULES if x.dependency is m)
        _fix_unit_test(dependent, fee=Decimal("25.00"))
    elif key == "delete":
        for name in DELETED:
            for folder in (CORE / name, INFRA / name, WEB / f"Views/{name}"):
                shutil.rmtree(folder)
            for file in (WEB / f"Controllers/Facilities/{name}Controller.cs",
                         WEB / f"Models/Facilities/{name}Models.cs", TESTS / f"{name}Tests.cs"):
                file.unlink()
        keep = [m for m in MODULES if m.name not in DELETED]
        for dependent in (m for m in keep if m.dependency and m.dependency.name in DELETED):
            raise SystemExit(f"{dependent.name} depends on a deleted module")
        write_shared(keep)
    if key == "hub":
        for m in MODULES:
            _fix_unit_test(m, vat=NEW_VAT)


def _fix_unit_test(m: Module, **changes) -> None:
    old = str(m.quote("B", 1)["total"])
    new = str(m.quote("B", 1, **changes)["total"])
    if old != new:
        replace(TESTS / f"{m.name}Tests.cs", f"Assert.Equal({old}m,", f"Assert.Equal({new}m,")


# ------------------------------------------------------------------------------ expectations
# A small model of the generated call graph: which members each member calls or reads.


def edges(modules: list[Module]) -> dict[str, set[str]]:
    graph: dict[str, set[str]] = {
        "FacilityTax.Apply": {"FacilityTax.VatRate", "FacilityMoney.Round"},
    }
    for m in modules:
        M, C, S, P, R = m.name, f"{m.name}Controller", f"{m.name}Service", f"{m.name}Pricing", f"{m.name}Report"
        repo = f"InMemory{M}Repository"
        reads = {f"{M}Policy.Multiplier", f"{M}Policy.BulkThreshold", f"{M}Policy.BulkDiscountRate",
                 "FacilityTax.Apply", "FacilityMoney.Round"}
        if m.dependency:
            reads.add(f"{m.dependency.name}Policy.HandlingFee")
        graph.update({
            f"{C}.Index": {f"{S}.ListItems"},
            f"{C}.Quote": {f"{S}.Quote", f"{S}.ListItems"},
            f"{C}.Place": {f"{S}.Place"},
            f"{C}.Orders": {f"{S}.GetOrders"},
            f"{C}.Report": {f"{R}.Summarize"},
            f"{S}.ListItems": {f"{repo}.Items"},
            f"{S}.Quote": {f"{S}.Find", f"{M}Validator.Validate", f"{P}.Quote"},
            f"{S}.Place": {f"{S}.Find", f"{M}Validator.Validate", f"{P}.Quote", f"{repo}.NextOrderId",
                           f"{repo}.Add", "FacilityAudit.Record", f"{M}Messages.OrderPlaced"},
            f"{S}.GetOrders": {f"{repo}.OrdersFor"},
            f"{S}.Find": {f"{repo}.Find"},
            f"{R}.Summarize": {f"{repo}.AllOrders", f"{repo}.Find", f"{P}.Quote"},
            f"{P}.Quote": reads,
            f"{M}Validator.Validate": {f"{M}Policy.MaxQuantity"},
        })
    return graph


def affected(graph: dict[str, set[str]], changed: set[str]) -> set[str]:
    callers: dict[str, set[str]] = {}
    for source, targets in graph.items():
        for target in targets:
            callers.setdefault(target, set()).add(source)
    seen, frontier = set(changed), list(changed)
    while frontier:
        for caller in callers.get(frontier.pop(), ()):
            if caller not in seen:
                seen.add(caller)
                frontier.append(caller)
    return seen


def changed_members(key: str) -> set[str]:
    if key == "hub":
        return {"FacilityTax.VatRate"}
    if key == "wide":
        out = set()
        for m in (MODULES[i] for i in WIDE):
            out |= {f"{m.name}Messages.OrderPlaced", f"{m.name}Validator.Validate",
                    f"InMemory{m.name}Repository.OrdersFor"}
        return out
    if key == "small":
        return {f"{SMALL}Policy.Multiplier"}
    if key == "cross":
        return {f"{CROSS}Policy.HandlingFee"}
    if key == "delete":
        out = set()
        for name in DELETED:
            m = by_name(name)
            out |= {k for k in edges([m]) if k.startswith((name, f"InMemory{name}"))}
            out |= {f"{name}Policy.Multiplier", f"{name}Policy.MaxQuantity"}
        return out
    raise SystemExit(key)


def expected_failures(key: str, m: Module, case_number: int) -> bool:
    """Whether this test case's written expectation stops being true after the change."""

    if key == "hub":
        return case_number in {2, 3, 4, 8, 9}
    if key == "wide":
        return m.index in WIDE and case_number in {4, 5}
    if key == "small":
        return m.name == SMALL and case_number in {2, 3, 4, 8, 9}
    if key == "cross":
        dependent = next(x for x in MODULES if x.dependency is by_name(CROSS))
        return m is dependent and case_number in {2, 3, 4, 8, 9}
    if key == "delete":
        return m.name in DELETED
    return False


def expectations() -> dict:
    graph = edges(MODULES)
    result = {}
    for s in SCENARIOS:
        hit = affected(graph, changed_members(s.key))
        must, fail = [], []
        for m in MODULES:
            for case in test_cases(m):
                number = int(case["id"][3:]) - 1000 - m.index * 10
                if any(touch in hit for touch in case["code_touchpoints"]):
                    must.append(case["id"])
                if expected_failures(s.key, m, number):
                    fail.append(case["id"])
        result[s.key] = {"branch": s.branch, "commit": s.commit, "description": s.description,
                         "must_rerun": must, "expected_to_fail": fail}
    return result


def write_docs() -> None:
    cases = [case for m in MODULES for case in test_cases(m)]
    data = json.loads((ROOT / "docs/test-cases.json").read_text(encoding="utf-8"))
    write(
        DOCS / "test-cases.json",
        json.dumps({
            "application": data["application"],
            "version": "scale-1",
            "base_url_note": data["base_url_note"],
            "common_preconditions": data["common_preconditions"],
            "reset_step": data["reset_step"],
            "ai_sample_modules": [MODULES[i].name for i in AI_SAMPLE],
            "test_cases": cases,
        }, indent=2) + "\n",
    )
    expected = expectations()
    write(DOCS / "expectations.json", json.dumps(expected, indent=2) + "\n")
    lines = [
        "# Large-scale change scenarios",
        "",
        "Generated by `tools/scale/generate.py`. The app has 40 facility modules (about 770 C# and Razor",
        "files) on top of RoomBooking, and 400 browser test cases (TC-1001 to TC-1400, ten per module).",
        "Every scenario is a branch from the modules commit on `main`. Analyze the modules commit,",
        "then compare it with the head of a scenario branch.",
        "",
        f"Test cases of {', '.join(MODULES[i].name for i in AI_SAMPLE)} are not pre-linked, so the AI",
        "links them on first use.",
        "",
    ]
    for s in SCENARIOS:
        e = expected[s.key]
        lines += [
            f"## {s.key}: {s.description}",
            "",
            f"Branch `{s.branch}`. Commit: {s.commit}",
            "",
            f"- Must rerun: {len(e['must_rerun'])} test cases",
            f"- Expected to fail: {len(e['expected_to_fail'])} test cases",
            "",
            "Expected to fail: " + (", ".join(e["expected_to_fail"]) or "none"),
            "",
        ]
    write(DOCS / "README.md", "\n".join(lines))


def main() -> None:
    command = sys.argv[1] if len(sys.argv) > 1 else ""
    if command == "modules":
        for m in MODULES:
            write_module(m)
        write_shared(MODULES)
        wire_app()
        write_docs()
        files = sum(1 for p in (ROOT / "src").rglob("*") if p.suffix in {".cs", ".cshtml"})
        print(f"Wrote {len(MODULES)} modules. The repository now has {files} C# and Razor source files.")
    elif command == "scenario":
        apply_scenario(sys.argv[2])
        print(f"Applied scenario {sys.argv[2]}.")
    elif command == "list":
        for s in SCENARIOS:
            print(s.key, s.branch, "|", s.commit)
    else:
        raise SystemExit(__doc__)


if __name__ == "__main__":
    main()
