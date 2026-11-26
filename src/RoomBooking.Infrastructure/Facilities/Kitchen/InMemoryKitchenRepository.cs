using RoomBooking.Core.Facilities.Kitchen;
using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Infrastructure.Facilities.Kitchen;

public sealed class InMemoryKitchenRepository : IKitchenRepository, IResettable
{
    private readonly object _sync = new();
    private List<KitchenItem> _items = [];
    private List<KitchenOrder> _orders = [];
    private int _next;

    public InMemoryKitchenRepository() => Reset();

    public void Reset()
    {
        lock (_sync)
        {
            _items = KitchenSeed.Items();
            _orders = KitchenSeed.Orders();
            _next = 1001 + _orders.Count;
        }
    }

    public IReadOnlyList<KitchenItem> Items()
    {
        lock (_sync)
        {
            return _items.ToList();
        }
    }

    public KitchenItem? Find(string itemId)
    {
        lock (_sync)
        {
            return _items.FirstOrDefault(item => item.Id == itemId);
        }
    }

    public IReadOnlyList<KitchenOrder> OrdersFor(string customerId)
    {
        lock (_sync)
        {
            return _orders.Where(order => order.CustomerId == customerId).OrderBy(order => order.Id).ToList();
        }
    }

    public IReadOnlyList<KitchenOrder> AllOrders()
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
            return $"KIT-{_next++}";
        }
    }

    public void Add(KitchenOrder order)
    {
        lock (_sync)
        {
            _orders.Add(order);
        }
    }
}
