using RoomBooking.Core.Facilities.Signage;
using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Infrastructure.Facilities.Signage;

public sealed class InMemorySignageRepository : ISignageRepository, IResettable
{
    private readonly object _sync = new();
    private List<SignageItem> _items = [];
    private List<SignageOrder> _orders = [];
    private int _next;

    public InMemorySignageRepository() => Reset();

    public void Reset()
    {
        lock (_sync)
        {
            _items = SignageSeed.Items();
            _orders = SignageSeed.Orders();
            _next = 1001 + _orders.Count;
        }
    }

    public IReadOnlyList<SignageItem> Items()
    {
        lock (_sync)
        {
            return _items.ToList();
        }
    }

    public SignageItem? Find(string itemId)
    {
        lock (_sync)
        {
            return _items.FirstOrDefault(item => item.Id == itemId);
        }
    }

    public IReadOnlyList<SignageOrder> OrdersFor(string customerId)
    {
        lock (_sync)
        {
            return _orders.Where(order => order.CustomerId == customerId).OrderBy(order => order.Id).ToList();
        }
    }

    public IReadOnlyList<SignageOrder> AllOrders()
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
            return $"SIG-{_next++}";
        }
    }

    public void Add(SignageOrder order)
    {
        lock (_sync)
        {
            _orders.Add(order);
        }
    }
}
