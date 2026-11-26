using RoomBooking.Core.Facilities.Garden;
using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Infrastructure.Facilities.Garden;

public sealed class InMemoryGardenRepository : IGardenRepository, IResettable
{
    private readonly object _sync = new();
    private List<GardenItem> _items = [];
    private List<GardenOrder> _orders = [];
    private int _next;

    public InMemoryGardenRepository() => Reset();

    public void Reset()
    {
        lock (_sync)
        {
            _items = GardenSeed.Items();
            _orders = GardenSeed.Orders();
            _next = 1001 + _orders.Count;
        }
    }

    public IReadOnlyList<GardenItem> Items()
    {
        lock (_sync)
        {
            return _items.ToList();
        }
    }

    public GardenItem? Find(string itemId)
    {
        lock (_sync)
        {
            return _items.FirstOrDefault(item => item.Id == itemId);
        }
    }

    public IReadOnlyList<GardenOrder> OrdersFor(string customerId)
    {
        lock (_sync)
        {
            return _orders.Where(order => order.CustomerId == customerId).OrderBy(order => order.Id).ToList();
        }
    }

    public IReadOnlyList<GardenOrder> AllOrders()
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
            return $"GAR-{_next++}";
        }
    }

    public void Add(GardenOrder order)
    {
        lock (_sync)
        {
            _orders.Add(order);
        }
    }
}
