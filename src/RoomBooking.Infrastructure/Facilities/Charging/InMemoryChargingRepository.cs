using RoomBooking.Core.Facilities.Charging;
using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Infrastructure.Facilities.Charging;

public sealed class InMemoryChargingRepository : IChargingRepository, IResettable
{
    private readonly object _sync = new();
    private List<ChargingItem> _items = [];
    private List<ChargingOrder> _orders = [];
    private int _next;

    public InMemoryChargingRepository() => Reset();

    public void Reset()
    {
        lock (_sync)
        {
            _items = ChargingSeed.Items();
            _orders = ChargingSeed.Orders();
            _next = 1001 + _orders.Count;
        }
    }

    public IReadOnlyList<ChargingItem> Items()
    {
        lock (_sync)
        {
            return _items.ToList();
        }
    }

    public ChargingItem? Find(string itemId)
    {
        lock (_sync)
        {
            return _items.FirstOrDefault(item => item.Id == itemId);
        }
    }

    public IReadOnlyList<ChargingOrder> OrdersFor(string customerId)
    {
        lock (_sync)
        {
            return _orders.Where(order => order.CustomerId == customerId).OrderBy(order => order.Id).ToList();
        }
    }

    public IReadOnlyList<ChargingOrder> AllOrders()
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
            return $"CHA-{_next++}";
        }
    }

    public void Add(ChargingOrder order)
    {
        lock (_sync)
        {
            _orders.Add(order);
        }
    }
}
