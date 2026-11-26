using RoomBooking.Core.Facilities.Laundry;
using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Infrastructure.Facilities.Laundry;

public sealed class InMemoryLaundryRepository : ILaundryRepository, IResettable
{
    private readonly object _sync = new();
    private List<LaundryItem> _items = [];
    private List<LaundryOrder> _orders = [];
    private int _next;

    public InMemoryLaundryRepository() => Reset();

    public void Reset()
    {
        lock (_sync)
        {
            _items = LaundrySeed.Items();
            _orders = LaundrySeed.Orders();
            _next = 1001 + _orders.Count;
        }
    }

    public IReadOnlyList<LaundryItem> Items()
    {
        lock (_sync)
        {
            return _items.ToList();
        }
    }

    public LaundryItem? Find(string itemId)
    {
        lock (_sync)
        {
            return _items.FirstOrDefault(item => item.Id == itemId);
        }
    }

    public IReadOnlyList<LaundryOrder> OrdersFor(string customerId)
    {
        lock (_sync)
        {
            return _orders.Where(order => order.CustomerId == customerId).OrderBy(order => order.Id).ToList();
        }
    }

    public IReadOnlyList<LaundryOrder> AllOrders()
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
            return $"LAU-{_next++}";
        }
    }

    public void Add(LaundryOrder order)
    {
        lock (_sync)
        {
            _orders.Add(order);
        }
    }
}
