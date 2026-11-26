using RoomBooking.Core.Facilities.Childcare;
using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Infrastructure.Facilities.Childcare;

public sealed class InMemoryChildcareRepository : IChildcareRepository, IResettable
{
    private readonly object _sync = new();
    private List<ChildcareItem> _items = [];
    private List<ChildcareOrder> _orders = [];
    private int _next;

    public InMemoryChildcareRepository() => Reset();

    public void Reset()
    {
        lock (_sync)
        {
            _items = ChildcareSeed.Items();
            _orders = ChildcareSeed.Orders();
            _next = 1001 + _orders.Count;
        }
    }

    public IReadOnlyList<ChildcareItem> Items()
    {
        lock (_sync)
        {
            return _items.ToList();
        }
    }

    public ChildcareItem? Find(string itemId)
    {
        lock (_sync)
        {
            return _items.FirstOrDefault(item => item.Id == itemId);
        }
    }

    public IReadOnlyList<ChildcareOrder> OrdersFor(string customerId)
    {
        lock (_sync)
        {
            var mine = _orders.Where(order => order.CustomerId == customerId);
            return [.. mine.OrderBy(order => order.Id)];
        }
    }

    public IReadOnlyList<ChildcareOrder> AllOrders()
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
            return $"CHI-{_next++}";
        }
    }

    public void Add(ChildcareOrder order)
    {
        lock (_sync)
        {
            _orders.Add(order);
        }
    }
}
