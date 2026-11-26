using RoomBooking.Core.Facilities.Courier;
using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Infrastructure.Facilities.Courier;

public sealed class InMemoryCourierRepository : ICourierRepository, IResettable
{
    private readonly object _sync = new();
    private List<CourierItem> _items = [];
    private List<CourierOrder> _orders = [];
    private int _next;

    public InMemoryCourierRepository() => Reset();

    public void Reset()
    {
        lock (_sync)
        {
            _items = CourierSeed.Items();
            _orders = CourierSeed.Orders();
            _next = 1001 + _orders.Count;
        }
    }

    public IReadOnlyList<CourierItem> Items()
    {
        lock (_sync)
        {
            return _items.ToList();
        }
    }

    public CourierItem? Find(string itemId)
    {
        lock (_sync)
        {
            return _items.FirstOrDefault(item => item.Id == itemId);
        }
    }

    public IReadOnlyList<CourierOrder> OrdersFor(string customerId)
    {
        lock (_sync)
        {
            return _orders.Where(order => order.CustomerId == customerId).OrderBy(order => order.Id).ToList();
        }
    }

    public IReadOnlyList<CourierOrder> AllOrders()
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
            return $"COU-{_next++}";
        }
    }

    public void Add(CourierOrder order)
    {
        lock (_sync)
        {
            _orders.Add(order);
        }
    }
}
