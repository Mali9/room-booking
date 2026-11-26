using RoomBooking.Core.Facilities.Pantry;
using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Infrastructure.Facilities.Pantry;

public sealed class InMemoryPantryRepository : IPantryRepository, IResettable
{
    private readonly object _sync = new();
    private List<PantryItem> _items = [];
    private List<PantryOrder> _orders = [];
    private int _next;

    public InMemoryPantryRepository() => Reset();

    public void Reset()
    {
        lock (_sync)
        {
            _items = PantrySeed.Items();
            _orders = PantrySeed.Orders();
            _next = 1001 + _orders.Count;
        }
    }

    public IReadOnlyList<PantryItem> Items()
    {
        lock (_sync)
        {
            return _items.ToList();
        }
    }

    public PantryItem? Find(string itemId)
    {
        lock (_sync)
        {
            return _items.FirstOrDefault(item => item.Id == itemId);
        }
    }

    public IReadOnlyList<PantryOrder> OrdersFor(string customerId)
    {
        lock (_sync)
        {
            return _orders.Where(order => order.CustomerId == customerId).OrderBy(order => order.Id).ToList();
        }
    }

    public IReadOnlyList<PantryOrder> AllOrders()
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
            return $"PAN-{_next++}";
        }
    }

    public void Add(PantryOrder order)
    {
        lock (_sync)
        {
            _orders.Add(order);
        }
    }
}
