using RoomBooking.Core.Facilities.Desks;
using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Infrastructure.Facilities.Desks;

public sealed class InMemoryDesksRepository : IDesksRepository, IResettable
{
    private readonly object _sync = new();
    private List<DesksItem> _items = [];
    private List<DesksOrder> _orders = [];
    private int _next;

    public InMemoryDesksRepository() => Reset();

    public void Reset()
    {
        lock (_sync)
        {
            _items = DesksSeed.Items();
            _orders = DesksSeed.Orders();
            _next = 1001 + _orders.Count;
        }
    }

    public IReadOnlyList<DesksItem> Items()
    {
        lock (_sync)
        {
            return _items.ToList();
        }
    }

    public DesksItem? Find(string itemId)
    {
        lock (_sync)
        {
            return _items.FirstOrDefault(item => item.Id == itemId);
        }
    }

    public IReadOnlyList<DesksOrder> OrdersFor(string customerId)
    {
        lock (_sync)
        {
            return _orders.Where(order => order.CustomerId == customerId).OrderBy(order => order.Id).ToList();
        }
    }

    public IReadOnlyList<DesksOrder> AllOrders()
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
            return $"DES-{_next++}";
        }
    }

    public void Add(DesksOrder order)
    {
        lock (_sync)
        {
            _orders.Add(order);
        }
    }
}
