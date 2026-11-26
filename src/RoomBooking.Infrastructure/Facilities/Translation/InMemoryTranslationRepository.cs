using RoomBooking.Core.Facilities.Translation;
using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Infrastructure.Facilities.Translation;

public sealed class InMemoryTranslationRepository : ITranslationRepository, IResettable
{
    private readonly object _sync = new();
    private List<TranslationItem> _items = [];
    private List<TranslationOrder> _orders = [];
    private int _next;

    public InMemoryTranslationRepository() => Reset();

    public void Reset()
    {
        lock (_sync)
        {
            _items = TranslationSeed.Items();
            _orders = TranslationSeed.Orders();
            _next = 1001 + _orders.Count;
        }
    }

    public IReadOnlyList<TranslationItem> Items()
    {
        lock (_sync)
        {
            return _items.ToList();
        }
    }

    public TranslationItem? Find(string itemId)
    {
        lock (_sync)
        {
            return _items.FirstOrDefault(item => item.Id == itemId);
        }
    }

    public IReadOnlyList<TranslationOrder> OrdersFor(string customerId)
    {
        lock (_sync)
        {
            return _orders.Where(order => order.CustomerId == customerId).OrderBy(order => order.Id).ToList();
        }
    }

    public IReadOnlyList<TranslationOrder> AllOrders()
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
            return $"TRA-{_next++}";
        }
    }

    public void Add(TranslationOrder order)
    {
        lock (_sync)
        {
            _orders.Add(order);
        }
    }
}
