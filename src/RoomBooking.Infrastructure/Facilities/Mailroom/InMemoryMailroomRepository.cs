using RoomBooking.Core.Facilities.Mailroom;
using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Infrastructure.Facilities.Mailroom;

public sealed class InMemoryMailroomRepository : IMailroomRepository, IResettable
{
    private readonly object _sync = new();
    private List<MailroomItem> _items = [];
    private List<MailroomOrder> _orders = [];
    private int _next;

    public InMemoryMailroomRepository() => Reset();

    public void Reset()
    {
        lock (_sync)
        {
            _items = MailroomSeed.Items();
            _orders = MailroomSeed.Orders();
            _next = 1001 + _orders.Count;
        }
    }

    public IReadOnlyList<MailroomItem> Items()
    {
        lock (_sync)
        {
            return _items.ToList();
        }
    }

    public MailroomItem? Find(string itemId)
    {
        lock (_sync)
        {
            return _items.FirstOrDefault(item => item.Id == itemId);
        }
    }

    public IReadOnlyList<MailroomOrder> OrdersFor(string customerId)
    {
        lock (_sync)
        {
            return _orders.Where(order => order.CustomerId == customerId).OrderBy(order => order.Id).ToList();
        }
    }

    public IReadOnlyList<MailroomOrder> AllOrders()
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
            return $"MA6-{_next++}";
        }
    }

    public void Add(MailroomOrder order)
    {
        lock (_sync)
        {
            _orders.Add(order);
        }
    }
}
