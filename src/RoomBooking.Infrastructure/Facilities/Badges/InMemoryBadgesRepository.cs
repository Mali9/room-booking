using RoomBooking.Core.Facilities.Badges;
using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Infrastructure.Facilities.Badges;

public sealed class InMemoryBadgesRepository : IBadgesRepository, IResettable
{
    private readonly object _sync = new();
    private List<BadgesItem> _items = [];
    private List<BadgesOrder> _orders = [];
    private int _next;

    public InMemoryBadgesRepository() => Reset();

    public void Reset()
    {
        lock (_sync)
        {
            _items = BadgesSeed.Items();
            _orders = BadgesSeed.Orders();
            _next = 1001 + _orders.Count;
        }
    }

    public IReadOnlyList<BadgesItem> Items()
    {
        lock (_sync)
        {
            return _items.ToList();
        }
    }

    public BadgesItem? Find(string itemId)
    {
        lock (_sync)
        {
            return _items.FirstOrDefault(item => item.Id == itemId);
        }
    }

    public IReadOnlyList<BadgesOrder> OrdersFor(string customerId)
    {
        lock (_sync)
        {
            var mine = _orders.Where(order => order.CustomerId == customerId);
            return [.. mine.OrderBy(order => order.Id)];
        }
    }

    public IReadOnlyList<BadgesOrder> AllOrders()
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
            return $"BAD-{_next++}";
        }
    }

    public void Add(BadgesOrder order)
    {
        lock (_sync)
        {
            _orders.Add(order);
        }
    }
}
