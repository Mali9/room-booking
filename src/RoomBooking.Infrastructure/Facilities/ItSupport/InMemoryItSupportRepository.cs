using RoomBooking.Core.Facilities.ItSupport;
using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Infrastructure.Facilities.ItSupport;

public sealed class InMemoryItSupportRepository : IItSupportRepository, IResettable
{
    private readonly object _sync = new();
    private List<ItSupportItem> _items = [];
    private List<ItSupportOrder> _orders = [];
    private int _next;

    public InMemoryItSupportRepository() => Reset();

    public void Reset()
    {
        lock (_sync)
        {
            _items = ItSupportSeed.Items();
            _orders = ItSupportSeed.Orders();
            _next = 1001 + _orders.Count;
        }
    }

    public IReadOnlyList<ItSupportItem> Items()
    {
        lock (_sync)
        {
            return _items.ToList();
        }
    }

    public ItSupportItem? Find(string itemId)
    {
        lock (_sync)
        {
            return _items.FirstOrDefault(item => item.Id == itemId);
        }
    }

    public IReadOnlyList<ItSupportOrder> OrdersFor(string customerId)
    {
        lock (_sync)
        {
            var mine = _orders.Where(order => order.CustomerId == customerId);
            return [.. mine.OrderBy(order => order.Id)];
        }
    }

    public IReadOnlyList<ItSupportOrder> AllOrders()
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
            return $"ITS-{_next++}";
        }
    }

    public void Add(ItSupportOrder order)
    {
        lock (_sync)
        {
            _orders.Add(order);
        }
    }
}
