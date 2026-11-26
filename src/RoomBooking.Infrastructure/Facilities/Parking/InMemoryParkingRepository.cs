using RoomBooking.Core.Facilities.Parking;
using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Infrastructure.Facilities.Parking;

public sealed class InMemoryParkingRepository : IParkingRepository, IResettable
{
    private readonly object _sync = new();
    private List<ParkingItem> _items = [];
    private List<ParkingOrder> _orders = [];
    private int _next;

    public InMemoryParkingRepository() => Reset();

    public void Reset()
    {
        lock (_sync)
        {
            _items = ParkingSeed.Items();
            _orders = ParkingSeed.Orders();
            _next = 1001 + _orders.Count;
        }
    }

    public IReadOnlyList<ParkingItem> Items()
    {
        lock (_sync)
        {
            return _items.ToList();
        }
    }

    public ParkingItem? Find(string itemId)
    {
        lock (_sync)
        {
            return _items.FirstOrDefault(item => item.Id == itemId);
        }
    }

    public IReadOnlyList<ParkingOrder> OrdersFor(string customerId)
    {
        lock (_sync)
        {
            return _orders.Where(order => order.CustomerId == customerId).OrderBy(order => order.Id).ToList();
        }
    }

    public IReadOnlyList<ParkingOrder> AllOrders()
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
            return $"PAR-{_next++}";
        }
    }

    public void Add(ParkingOrder order)
    {
        lock (_sync)
        {
            _orders.Add(order);
        }
    }
}
