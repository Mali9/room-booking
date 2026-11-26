using RoomBooking.Core.Facilities.Recording;
using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Infrastructure.Facilities.Recording;

public sealed class InMemoryRecordingRepository : IRecordingRepository, IResettable
{
    private readonly object _sync = new();
    private List<RecordingItem> _items = [];
    private List<RecordingOrder> _orders = [];
    private int _next;

    public InMemoryRecordingRepository() => Reset();

    public void Reset()
    {
        lock (_sync)
        {
            _items = RecordingSeed.Items();
            _orders = RecordingSeed.Orders();
            _next = 1001 + _orders.Count;
        }
    }

    public IReadOnlyList<RecordingItem> Items()
    {
        lock (_sync)
        {
            return _items.ToList();
        }
    }

    public RecordingItem? Find(string itemId)
    {
        lock (_sync)
        {
            return _items.FirstOrDefault(item => item.Id == itemId);
        }
    }

    public IReadOnlyList<RecordingOrder> OrdersFor(string customerId)
    {
        lock (_sync)
        {
            var mine = _orders.Where(order => order.CustomerId == customerId);
            return [.. mine.OrderBy(order => order.Id)];
        }
    }

    public IReadOnlyList<RecordingOrder> AllOrders()
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
            return $"REC-{_next++}";
        }
    }

    public void Add(RecordingOrder order)
    {
        lock (_sync)
        {
            _orders.Add(order);
        }
    }
}
