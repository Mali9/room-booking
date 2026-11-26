using RoomBooking.Core.Facilities.Printing;
using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Infrastructure.Facilities.Printing;

public sealed class InMemoryPrintingRepository : IPrintingRepository, IResettable
{
    private readonly object _sync = new();
    private List<PrintingItem> _items = [];
    private List<PrintingOrder> _orders = [];
    private int _next;

    public InMemoryPrintingRepository() => Reset();

    public void Reset()
    {
        lock (_sync)
        {
            _items = PrintingSeed.Items();
            _orders = PrintingSeed.Orders();
            _next = 1001 + _orders.Count;
        }
    }

    public IReadOnlyList<PrintingItem> Items()
    {
        lock (_sync)
        {
            return _items.ToList();
        }
    }

    public PrintingItem? Find(string itemId)
    {
        lock (_sync)
        {
            return _items.FirstOrDefault(item => item.Id == itemId);
        }
    }

    public IReadOnlyList<PrintingOrder> OrdersFor(string customerId)
    {
        lock (_sync)
        {
            return _orders.Where(order => order.CustomerId == customerId).OrderBy(order => order.Id).ToList();
        }
    }

    public IReadOnlyList<PrintingOrder> AllOrders()
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
            return $"PRI-{_next++}";
        }
    }

    public void Add(PrintingOrder order)
    {
        lock (_sync)
        {
            _orders.Add(order);
        }
    }
}
