using RoomBooking.Core.Facilities.Furniture;
using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Infrastructure.Facilities.Furniture;

public sealed class InMemoryFurnitureRepository : IFurnitureRepository, IResettable
{
    private readonly object _sync = new();
    private List<FurnitureItem> _items = [];
    private List<FurnitureOrder> _orders = [];
    private int _next;

    public InMemoryFurnitureRepository() => Reset();

    public void Reset()
    {
        lock (_sync)
        {
            _items = FurnitureSeed.Items();
            _orders = FurnitureSeed.Orders();
            _next = 1001 + _orders.Count;
        }
    }

    public IReadOnlyList<FurnitureItem> Items()
    {
        lock (_sync)
        {
            return _items.ToList();
        }
    }

    public FurnitureItem? Find(string itemId)
    {
        lock (_sync)
        {
            return _items.FirstOrDefault(item => item.Id == itemId);
        }
    }

    public IReadOnlyList<FurnitureOrder> OrdersFor(string customerId)
    {
        lock (_sync)
        {
            return _orders.Where(order => order.CustomerId == customerId).OrderBy(order => order.Id).ToList();
        }
    }

    public IReadOnlyList<FurnitureOrder> AllOrders()
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
            return $"FUR-{_next++}";
        }
    }

    public void Add(FurnitureOrder order)
    {
        lock (_sync)
        {
            _orders.Add(order);
        }
    }
}
