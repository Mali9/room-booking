using RoomBooking.Core.Facilities.Keys;
using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Infrastructure.Facilities.Keys;

public sealed class InMemoryKeysRepository : IKeysRepository, IResettable
{
    private readonly object _sync = new();
    private List<KeysItem> _items = [];
    private List<KeysOrder> _orders = [];
    private int _next;

    public InMemoryKeysRepository() => Reset();

    public void Reset()
    {
        lock (_sync)
        {
            _items = KeysSeed.Items();
            _orders = KeysSeed.Orders();
            _next = 1001 + _orders.Count;
        }
    }

    public IReadOnlyList<KeysItem> Items()
    {
        lock (_sync)
        {
            return _items.ToList();
        }
    }

    public KeysItem? Find(string itemId)
    {
        lock (_sync)
        {
            return _items.FirstOrDefault(item => item.Id == itemId);
        }
    }

    public IReadOnlyList<KeysOrder> OrdersFor(string customerId)
    {
        lock (_sync)
        {
            return _orders.Where(order => order.CustomerId == customerId).OrderBy(order => order.Id).ToList();
        }
    }

    public IReadOnlyList<KeysOrder> AllOrders()
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
            return $"KEY-{_next++}";
        }
    }

    public void Add(KeysOrder order)
    {
        lock (_sync)
        {
            _orders.Add(order);
        }
    }
}
