using RoomBooking.Core.Facilities.Catering;
using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Infrastructure.Facilities.Catering;

public sealed class InMemoryCateringRepository : ICateringRepository, IResettable
{
    private readonly object _sync = new();
    private List<CateringItem> _items = [];
    private List<CateringOrder> _orders = [];
    private int _next;

    public InMemoryCateringRepository() => Reset();

    public void Reset()
    {
        lock (_sync)
        {
            _items = CateringSeed.Items();
            _orders = CateringSeed.Orders();
            _next = 1001 + _orders.Count;
        }
    }

    public IReadOnlyList<CateringItem> Items()
    {
        lock (_sync)
        {
            return _items.ToList();
        }
    }

    public CateringItem? Find(string itemId)
    {
        lock (_sync)
        {
            return _items.FirstOrDefault(item => item.Id == itemId);
        }
    }

    public IReadOnlyList<CateringOrder> OrdersFor(string customerId)
    {
        lock (_sync)
        {
            return _orders.Where(order => order.CustomerId == customerId).OrderBy(order => order.Id).ToList();
        }
    }

    public IReadOnlyList<CateringOrder> AllOrders()
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
            return $"CAT-{_next++}";
        }
    }

    public void Add(CateringOrder order)
    {
        lock (_sync)
        {
            _orders.Add(order);
        }
    }
}
