using RoomBooking.Core.Facilities.Cleaning;
using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Infrastructure.Facilities.Cleaning;

public sealed class InMemoryCleaningRepository : ICleaningRepository, IResettable
{
    private readonly object _sync = new();
    private List<CleaningItem> _items = [];
    private List<CleaningOrder> _orders = [];
    private int _next;

    public InMemoryCleaningRepository() => Reset();

    public void Reset()
    {
        lock (_sync)
        {
            _items = CleaningSeed.Items();
            _orders = CleaningSeed.Orders();
            _next = 1001 + _orders.Count;
        }
    }

    public IReadOnlyList<CleaningItem> Items()
    {
        lock (_sync)
        {
            return _items.ToList();
        }
    }

    public CleaningItem? Find(string itemId)
    {
        lock (_sync)
        {
            return _items.FirstOrDefault(item => item.Id == itemId);
        }
    }

    public IReadOnlyList<CleaningOrder> OrdersFor(string customerId)
    {
        lock (_sync)
        {
            return _orders.Where(order => order.CustomerId == customerId).OrderBy(order => order.Id).ToList();
        }
    }

    public IReadOnlyList<CleaningOrder> AllOrders()
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
            return $"CLE-{_next++}";
        }
    }

    public void Add(CleaningOrder order)
    {
        lock (_sync)
        {
            _orders.Add(order);
        }
    }
}
