using RoomBooking.Core.Facilities.Security;
using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Infrastructure.Facilities.Security;

public sealed class InMemorySecurityRepository : ISecurityRepository, IResettable
{
    private readonly object _sync = new();
    private List<SecurityItem> _items = [];
    private List<SecurityOrder> _orders = [];
    private int _next;

    public InMemorySecurityRepository() => Reset();

    public void Reset()
    {
        lock (_sync)
        {
            _items = SecuritySeed.Items();
            _orders = SecuritySeed.Orders();
            _next = 1001 + _orders.Count;
        }
    }

    public IReadOnlyList<SecurityItem> Items()
    {
        lock (_sync)
        {
            return _items.ToList();
        }
    }

    public SecurityItem? Find(string itemId)
    {
        lock (_sync)
        {
            return _items.FirstOrDefault(item => item.Id == itemId);
        }
    }

    public IReadOnlyList<SecurityOrder> OrdersFor(string customerId)
    {
        lock (_sync)
        {
            return _orders.Where(order => order.CustomerId == customerId).OrderBy(order => order.Id).ToList();
        }
    }

    public IReadOnlyList<SecurityOrder> AllOrders()
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
            return $"SEC-{_next++}";
        }
    }

    public void Add(SecurityOrder order)
    {
        lock (_sync)
        {
            _orders.Add(order);
        }
    }
}
