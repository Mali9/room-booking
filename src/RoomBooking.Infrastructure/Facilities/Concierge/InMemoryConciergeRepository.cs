using RoomBooking.Core.Facilities.Concierge;
using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Infrastructure.Facilities.Concierge;

public sealed class InMemoryConciergeRepository : IConciergeRepository, IResettable
{
    private readonly object _sync = new();
    private List<ConciergeItem> _items = [];
    private List<ConciergeOrder> _orders = [];
    private int _next;

    public InMemoryConciergeRepository() => Reset();

    public void Reset()
    {
        lock (_sync)
        {
            _items = ConciergeSeed.Items();
            _orders = ConciergeSeed.Orders();
            _next = 1001 + _orders.Count;
        }
    }

    public IReadOnlyList<ConciergeItem> Items()
    {
        lock (_sync)
        {
            return _items.ToList();
        }
    }

    public ConciergeItem? Find(string itemId)
    {
        lock (_sync)
        {
            return _items.FirstOrDefault(item => item.Id == itemId);
        }
    }

    public IReadOnlyList<ConciergeOrder> OrdersFor(string customerId)
    {
        lock (_sync)
        {
            return _orders.Where(order => order.CustomerId == customerId).OrderBy(order => order.Id).ToList();
        }
    }

    public IReadOnlyList<ConciergeOrder> AllOrders()
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
            return $"CON-{_next++}";
        }
    }

    public void Add(ConciergeOrder order)
    {
        lock (_sync)
        {
            _orders.Add(order);
        }
    }
}
