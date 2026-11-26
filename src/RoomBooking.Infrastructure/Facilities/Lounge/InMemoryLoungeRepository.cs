using RoomBooking.Core.Facilities.Lounge;
using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Infrastructure.Facilities.Lounge;

public sealed class InMemoryLoungeRepository : ILoungeRepository, IResettable
{
    private readonly object _sync = new();
    private List<LoungeItem> _items = [];
    private List<LoungeOrder> _orders = [];
    private int _next;

    public InMemoryLoungeRepository() => Reset();

    public void Reset()
    {
        lock (_sync)
        {
            _items = LoungeSeed.Items();
            _orders = LoungeSeed.Orders();
            _next = 1001 + _orders.Count;
        }
    }

    public IReadOnlyList<LoungeItem> Items()
    {
        lock (_sync)
        {
            return _items.ToList();
        }
    }

    public LoungeItem? Find(string itemId)
    {
        lock (_sync)
        {
            return _items.FirstOrDefault(item => item.Id == itemId);
        }
    }

    public IReadOnlyList<LoungeOrder> OrdersFor(string customerId)
    {
        lock (_sync)
        {
            var mine = _orders.Where(order => order.CustomerId == customerId);
            return [.. mine.OrderBy(order => order.Id)];
        }
    }

    public IReadOnlyList<LoungeOrder> AllOrders()
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
            return $"LOU-{_next++}";
        }
    }

    public void Add(LoungeOrder order)
    {
        lock (_sync)
        {
            _orders.Add(order);
        }
    }
}
