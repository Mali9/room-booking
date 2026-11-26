using RoomBooking.Core.Facilities.Storage;
using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Infrastructure.Facilities.Storage;

public sealed class InMemoryStorageRepository : IStorageRepository, IResettable
{
    private readonly object _sync = new();
    private List<StorageItem> _items = [];
    private List<StorageOrder> _orders = [];
    private int _next;

    public InMemoryStorageRepository() => Reset();

    public void Reset()
    {
        lock (_sync)
        {
            _items = StorageSeed.Items();
            _orders = StorageSeed.Orders();
            _next = 1001 + _orders.Count;
        }
    }

    public IReadOnlyList<StorageItem> Items()
    {
        lock (_sync)
        {
            return _items.ToList();
        }
    }

    public StorageItem? Find(string itemId)
    {
        lock (_sync)
        {
            return _items.FirstOrDefault(item => item.Id == itemId);
        }
    }

    public IReadOnlyList<StorageOrder> OrdersFor(string customerId)
    {
        lock (_sync)
        {
            var mine = _orders.Where(order => order.CustomerId == customerId);
            return [.. mine.OrderBy(order => order.Id)];
        }
    }

    public IReadOnlyList<StorageOrder> AllOrders()
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
            return $"STO-{_next++}";
        }
    }

    public void Add(StorageOrder order)
    {
        lock (_sync)
        {
            _orders.Add(order);
        }
    }
}
