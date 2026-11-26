using RoomBooking.Core.Facilities.Visitors;
using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Infrastructure.Facilities.Visitors;

public sealed class InMemoryVisitorsRepository : IVisitorsRepository, IResettable
{
    private readonly object _sync = new();
    private List<VisitorsItem> _items = [];
    private List<VisitorsOrder> _orders = [];
    private int _next;

    public InMemoryVisitorsRepository() => Reset();

    public void Reset()
    {
        lock (_sync)
        {
            _items = VisitorsSeed.Items();
            _orders = VisitorsSeed.Orders();
            _next = 1001 + _orders.Count;
        }
    }

    public IReadOnlyList<VisitorsItem> Items()
    {
        lock (_sync)
        {
            return _items.ToList();
        }
    }

    public VisitorsItem? Find(string itemId)
    {
        lock (_sync)
        {
            return _items.FirstOrDefault(item => item.Id == itemId);
        }
    }

    public IReadOnlyList<VisitorsOrder> OrdersFor(string customerId)
    {
        lock (_sync)
        {
            return _orders.Where(order => order.CustomerId == customerId).OrderBy(order => order.Id).ToList();
        }
    }

    public IReadOnlyList<VisitorsOrder> AllOrders()
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
            return $"VIS-{_next++}";
        }
    }

    public void Add(VisitorsOrder order)
    {
        lock (_sync)
        {
            _orders.Add(order);
        }
    }
}
