using RoomBooking.Core.Facilities.Gym;
using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Infrastructure.Facilities.Gym;

public sealed class InMemoryGymRepository : IGymRepository, IResettable
{
    private readonly object _sync = new();
    private List<GymItem> _items = [];
    private List<GymOrder> _orders = [];
    private int _next;

    public InMemoryGymRepository() => Reset();

    public void Reset()
    {
        lock (_sync)
        {
            _items = GymSeed.Items();
            _orders = GymSeed.Orders();
            _next = 1001 + _orders.Count;
        }
    }

    public IReadOnlyList<GymItem> Items()
    {
        lock (_sync)
        {
            return _items.ToList();
        }
    }

    public GymItem? Find(string itemId)
    {
        lock (_sync)
        {
            return _items.FirstOrDefault(item => item.Id == itemId);
        }
    }

    public IReadOnlyList<GymOrder> OrdersFor(string customerId)
    {
        lock (_sync)
        {
            return _orders.Where(order => order.CustomerId == customerId).OrderBy(order => order.Id).ToList();
        }
    }

    public IReadOnlyList<GymOrder> AllOrders()
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
            return $"GYM-{_next++}";
        }
    }

    public void Add(GymOrder order)
    {
        lock (_sync)
        {
            _orders.Add(order);
        }
    }
}
