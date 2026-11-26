using RoomBooking.Core.Facilities.Library;
using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Infrastructure.Facilities.Library;

public sealed class InMemoryLibraryRepository : ILibraryRepository, IResettable
{
    private readonly object _sync = new();
    private List<LibraryItem> _items = [];
    private List<LibraryOrder> _orders = [];
    private int _next;

    public InMemoryLibraryRepository() => Reset();

    public void Reset()
    {
        lock (_sync)
        {
            _items = LibrarySeed.Items();
            _orders = LibrarySeed.Orders();
            _next = 1001 + _orders.Count;
        }
    }

    public IReadOnlyList<LibraryItem> Items()
    {
        lock (_sync)
        {
            return _items.ToList();
        }
    }

    public LibraryItem? Find(string itemId)
    {
        lock (_sync)
        {
            return _items.FirstOrDefault(item => item.Id == itemId);
        }
    }

    public IReadOnlyList<LibraryOrder> OrdersFor(string customerId)
    {
        lock (_sync)
        {
            var mine = _orders.Where(order => order.CustomerId == customerId);
            return [.. mine.OrderBy(order => order.Id)];
        }
    }

    public IReadOnlyList<LibraryOrder> AllOrders()
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
            return $"LIB-{_next++}";
        }
    }

    public void Add(LibraryOrder order)
    {
        lock (_sync)
        {
            _orders.Add(order);
        }
    }
}
