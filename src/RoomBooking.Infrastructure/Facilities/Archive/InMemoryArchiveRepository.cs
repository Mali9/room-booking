using RoomBooking.Core.Facilities.Archive;
using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Infrastructure.Facilities.Archive;

public sealed class InMemoryArchiveRepository : IArchiveRepository, IResettable
{
    private readonly object _sync = new();
    private List<ArchiveItem> _items = [];
    private List<ArchiveOrder> _orders = [];
    private int _next;

    public InMemoryArchiveRepository() => Reset();

    public void Reset()
    {
        lock (_sync)
        {
            _items = ArchiveSeed.Items();
            _orders = ArchiveSeed.Orders();
            _next = 1001 + _orders.Count;
        }
    }

    public IReadOnlyList<ArchiveItem> Items()
    {
        lock (_sync)
        {
            return _items.ToList();
        }
    }

    public ArchiveItem? Find(string itemId)
    {
        lock (_sync)
        {
            return _items.FirstOrDefault(item => item.Id == itemId);
        }
    }

    public IReadOnlyList<ArchiveOrder> OrdersFor(string customerId)
    {
        lock (_sync)
        {
            var mine = _orders.Where(order => order.CustomerId == customerId);
            return [.. mine.OrderBy(order => order.Id)];
        }
    }

    public IReadOnlyList<ArchiveOrder> AllOrders()
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
            return $"ARC-{_next++}";
        }
    }

    public void Add(ArchiveOrder order)
    {
        lock (_sync)
        {
            _orders.Add(order);
        }
    }
}
