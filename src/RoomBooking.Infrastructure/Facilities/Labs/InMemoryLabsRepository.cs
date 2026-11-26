using RoomBooking.Core.Facilities.Labs;
using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Infrastructure.Facilities.Labs;

public sealed class InMemoryLabsRepository : ILabsRepository, IResettable
{
    private readonly object _sync = new();
    private List<LabsItem> _items = [];
    private List<LabsOrder> _orders = [];
    private int _next;

    public InMemoryLabsRepository() => Reset();

    public void Reset()
    {
        lock (_sync)
        {
            _items = LabsSeed.Items();
            _orders = LabsSeed.Orders();
            _next = 1001 + _orders.Count;
        }
    }

    public IReadOnlyList<LabsItem> Items()
    {
        lock (_sync)
        {
            return _items.ToList();
        }
    }

    public LabsItem? Find(string itemId)
    {
        lock (_sync)
        {
            return _items.FirstOrDefault(item => item.Id == itemId);
        }
    }

    public IReadOnlyList<LabsOrder> OrdersFor(string customerId)
    {
        lock (_sync)
        {
            return _orders.Where(order => order.CustomerId == customerId).OrderBy(order => order.Id).ToList();
        }
    }

    public IReadOnlyList<LabsOrder> AllOrders()
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
            return $"LAB-{_next++}";
        }
    }

    public void Add(LabsOrder order)
    {
        lock (_sync)
        {
            _orders.Add(order);
        }
    }
}
