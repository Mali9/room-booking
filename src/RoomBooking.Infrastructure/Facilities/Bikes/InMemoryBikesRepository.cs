using RoomBooking.Core.Facilities.Bikes;
using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Infrastructure.Facilities.Bikes;

public sealed class InMemoryBikesRepository : IBikesRepository, IResettable
{
    private readonly object _sync = new();
    private List<BikesItem> _items = [];
    private List<BikesOrder> _orders = [];
    private int _next;

    public InMemoryBikesRepository() => Reset();

    public void Reset()
    {
        lock (_sync)
        {
            _items = BikesSeed.Items();
            _orders = BikesSeed.Orders();
            _next = 1001 + _orders.Count;
        }
    }

    public IReadOnlyList<BikesItem> Items()
    {
        lock (_sync)
        {
            return _items.ToList();
        }
    }

    public BikesItem? Find(string itemId)
    {
        lock (_sync)
        {
            return _items.FirstOrDefault(item => item.Id == itemId);
        }
    }

    public IReadOnlyList<BikesOrder> OrdersFor(string customerId)
    {
        lock (_sync)
        {
            return _orders.Where(order => order.CustomerId == customerId).OrderBy(order => order.Id).ToList();
        }
    }

    public IReadOnlyList<BikesOrder> AllOrders()
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
            return $"BIK-{_next++}";
        }
    }

    public void Add(BikesOrder order)
    {
        lock (_sync)
        {
            _orders.Add(order);
        }
    }
}
