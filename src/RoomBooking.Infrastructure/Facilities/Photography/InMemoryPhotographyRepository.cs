using RoomBooking.Core.Facilities.Photography;
using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Infrastructure.Facilities.Photography;

public sealed class InMemoryPhotographyRepository : IPhotographyRepository, IResettable
{
    private readonly object _sync = new();
    private List<PhotographyItem> _items = [];
    private List<PhotographyOrder> _orders = [];
    private int _next;

    public InMemoryPhotographyRepository() => Reset();

    public void Reset()
    {
        lock (_sync)
        {
            _items = PhotographySeed.Items();
            _orders = PhotographySeed.Orders();
            _next = 1001 + _orders.Count;
        }
    }

    public IReadOnlyList<PhotographyItem> Items()
    {
        lock (_sync)
        {
            return _items.ToList();
        }
    }

    public PhotographyItem? Find(string itemId)
    {
        lock (_sync)
        {
            return _items.FirstOrDefault(item => item.Id == itemId);
        }
    }

    public IReadOnlyList<PhotographyOrder> OrdersFor(string customerId)
    {
        lock (_sync)
        {
            var mine = _orders.Where(order => order.CustomerId == customerId);
            return [.. mine.OrderBy(order => order.Id)];
        }
    }

    public IReadOnlyList<PhotographyOrder> AllOrders()
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
            return $"PHO-{_next++}";
        }
    }

    public void Add(PhotographyOrder order)
    {
        lock (_sync)
        {
            _orders.Add(order);
        }
    }
}
