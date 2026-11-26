using RoomBooking.Core.Facilities.Uniforms;
using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Infrastructure.Facilities.Uniforms;

public sealed class InMemoryUniformsRepository : IUniformsRepository, IResettable
{
    private readonly object _sync = new();
    private List<UniformsItem> _items = [];
    private List<UniformsOrder> _orders = [];
    private int _next;

    public InMemoryUniformsRepository() => Reset();

    public void Reset()
    {
        lock (_sync)
        {
            _items = UniformsSeed.Items();
            _orders = UniformsSeed.Orders();
            _next = 1001 + _orders.Count;
        }
    }

    public IReadOnlyList<UniformsItem> Items()
    {
        lock (_sync)
        {
            return _items.ToList();
        }
    }

    public UniformsItem? Find(string itemId)
    {
        lock (_sync)
        {
            return _items.FirstOrDefault(item => item.Id == itemId);
        }
    }

    public IReadOnlyList<UniformsOrder> OrdersFor(string customerId)
    {
        lock (_sync)
        {
            var mine = _orders.Where(order => order.CustomerId == customerId);
            return [.. mine.OrderBy(order => order.Id)];
        }
    }

    public IReadOnlyList<UniformsOrder> AllOrders()
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
            return $"UNI-{_next++}";
        }
    }

    public void Add(UniformsOrder order)
    {
        lock (_sync)
        {
            _orders.Add(order);
        }
    }
}
