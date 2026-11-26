using RoomBooking.Core.Facilities.Wellness;
using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Infrastructure.Facilities.Wellness;

public sealed class InMemoryWellnessRepository : IWellnessRepository, IResettable
{
    private readonly object _sync = new();
    private List<WellnessItem> _items = [];
    private List<WellnessOrder> _orders = [];
    private int _next;

    public InMemoryWellnessRepository() => Reset();

    public void Reset()
    {
        lock (_sync)
        {
            _items = WellnessSeed.Items();
            _orders = WellnessSeed.Orders();
            _next = 1001 + _orders.Count;
        }
    }

    public IReadOnlyList<WellnessItem> Items()
    {
        lock (_sync)
        {
            return _items.ToList();
        }
    }

    public WellnessItem? Find(string itemId)
    {
        lock (_sync)
        {
            return _items.FirstOrDefault(item => item.Id == itemId);
        }
    }

    public IReadOnlyList<WellnessOrder> OrdersFor(string customerId)
    {
        lock (_sync)
        {
            var mine = _orders.Where(order => order.CustomerId == customerId);
            return [.. mine.OrderBy(order => order.Id)];
        }
    }

    public IReadOnlyList<WellnessOrder> AllOrders()
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
            return $"WEL-{_next++}";
        }
    }

    public void Add(WellnessOrder order)
    {
        lock (_sync)
        {
            _orders.Add(order);
        }
    }
}
