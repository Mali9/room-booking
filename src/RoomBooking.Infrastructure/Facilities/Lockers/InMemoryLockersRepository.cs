using RoomBooking.Core.Facilities.Lockers;
using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Infrastructure.Facilities.Lockers;

public sealed class InMemoryLockersRepository : ILockersRepository, IResettable
{
    private readonly object _sync = new();
    private List<LockersItem> _items = [];
    private List<LockersOrder> _orders = [];
    private int _next;

    public InMemoryLockersRepository() => Reset();

    public void Reset()
    {
        lock (_sync)
        {
            _items = LockersSeed.Items();
            _orders = LockersSeed.Orders();
            _next = 1001 + _orders.Count;
        }
    }

    public IReadOnlyList<LockersItem> Items()
    {
        lock (_sync)
        {
            return _items.ToList();
        }
    }

    public LockersItem? Find(string itemId)
    {
        lock (_sync)
        {
            return _items.FirstOrDefault(item => item.Id == itemId);
        }
    }

    public IReadOnlyList<LockersOrder> OrdersFor(string customerId)
    {
        lock (_sync)
        {
            return _orders.Where(order => order.CustomerId == customerId).OrderBy(order => order.Id).ToList();
        }
    }

    public IReadOnlyList<LockersOrder> AllOrders()
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
            return $"LOC-{_next++}";
        }
    }

    public void Add(LockersOrder order)
    {
        lock (_sync)
        {
            _orders.Add(order);
        }
    }
}
