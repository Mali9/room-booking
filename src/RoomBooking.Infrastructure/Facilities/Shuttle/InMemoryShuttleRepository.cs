using RoomBooking.Core.Facilities.Shuttle;
using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Infrastructure.Facilities.Shuttle;

public sealed class InMemoryShuttleRepository : IShuttleRepository, IResettable
{
    private readonly object _sync = new();
    private List<ShuttleItem> _items = [];
    private List<ShuttleOrder> _orders = [];
    private int _next;

    public InMemoryShuttleRepository() => Reset();

    public void Reset()
    {
        lock (_sync)
        {
            _items = ShuttleSeed.Items();
            _orders = ShuttleSeed.Orders();
            _next = 1001 + _orders.Count;
        }
    }

    public IReadOnlyList<ShuttleItem> Items()
    {
        lock (_sync)
        {
            return _items.ToList();
        }
    }

    public ShuttleItem? Find(string itemId)
    {
        lock (_sync)
        {
            return _items.FirstOrDefault(item => item.Id == itemId);
        }
    }

    public IReadOnlyList<ShuttleOrder> OrdersFor(string customerId)
    {
        lock (_sync)
        {
            return _orders.Where(order => order.CustomerId == customerId).OrderBy(order => order.Id).ToList();
        }
    }

    public IReadOnlyList<ShuttleOrder> AllOrders()
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
            return $"SHU-{_next++}";
        }
    }

    public void Add(ShuttleOrder order)
    {
        lock (_sync)
        {
            _orders.Add(order);
        }
    }
}
