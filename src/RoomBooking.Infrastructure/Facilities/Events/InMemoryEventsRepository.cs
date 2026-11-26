using RoomBooking.Core.Facilities.Events;
using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Infrastructure.Facilities.Events;

public sealed class InMemoryEventsRepository : IEventsRepository, IResettable
{
    private readonly object _sync = new();
    private List<EventsItem> _items = [];
    private List<EventsOrder> _orders = [];
    private int _next;

    public InMemoryEventsRepository() => Reset();

    public void Reset()
    {
        lock (_sync)
        {
            _items = EventsSeed.Items();
            _orders = EventsSeed.Orders();
            _next = 1001 + _orders.Count;
        }
    }

    public IReadOnlyList<EventsItem> Items()
    {
        lock (_sync)
        {
            return _items.ToList();
        }
    }

    public EventsItem? Find(string itemId)
    {
        lock (_sync)
        {
            return _items.FirstOrDefault(item => item.Id == itemId);
        }
    }

    public IReadOnlyList<EventsOrder> OrdersFor(string customerId)
    {
        lock (_sync)
        {
            return _orders.Where(order => order.CustomerId == customerId).OrderBy(order => order.Id).ToList();
        }
    }

    public IReadOnlyList<EventsOrder> AllOrders()
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
            return $"EVE-{_next++}";
        }
    }

    public void Add(EventsOrder order)
    {
        lock (_sync)
        {
            _orders.Add(order);
        }
    }
}
