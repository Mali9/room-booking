using RoomBooking.Core.Facilities.Studio;
using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Infrastructure.Facilities.Studio;

public sealed class InMemoryStudioRepository : IStudioRepository, IResettable
{
    private readonly object _sync = new();
    private List<StudioItem> _items = [];
    private List<StudioOrder> _orders = [];
    private int _next;

    public InMemoryStudioRepository() => Reset();

    public void Reset()
    {
        lock (_sync)
        {
            _items = StudioSeed.Items();
            _orders = StudioSeed.Orders();
            _next = 1001 + _orders.Count;
        }
    }

    public IReadOnlyList<StudioItem> Items()
    {
        lock (_sync)
        {
            return _items.ToList();
        }
    }

    public StudioItem? Find(string itemId)
    {
        lock (_sync)
        {
            return _items.FirstOrDefault(item => item.Id == itemId);
        }
    }

    public IReadOnlyList<StudioOrder> OrdersFor(string customerId)
    {
        lock (_sync)
        {
            var mine = _orders.Where(order => order.CustomerId == customerId);
            return [.. mine.OrderBy(order => order.Id)];
        }
    }

    public IReadOnlyList<StudioOrder> AllOrders()
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
            return $"STU-{_next++}";
        }
    }

    public void Add(StudioOrder order)
    {
        lock (_sync)
        {
            _orders.Add(order);
        }
    }
}
