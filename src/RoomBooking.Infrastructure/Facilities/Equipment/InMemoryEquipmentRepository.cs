using RoomBooking.Core.Facilities.Equipment;
using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Infrastructure.Facilities.Equipment;

public sealed class InMemoryEquipmentRepository : IEquipmentRepository, IResettable
{
    private readonly object _sync = new();
    private List<EquipmentItem> _items = [];
    private List<EquipmentOrder> _orders = [];
    private int _next;

    public InMemoryEquipmentRepository() => Reset();

    public void Reset()
    {
        lock (_sync)
        {
            _items = EquipmentSeed.Items();
            _orders = EquipmentSeed.Orders();
            _next = 1001 + _orders.Count;
        }
    }

    public IReadOnlyList<EquipmentItem> Items()
    {
        lock (_sync)
        {
            return _items.ToList();
        }
    }

    public EquipmentItem? Find(string itemId)
    {
        lock (_sync)
        {
            return _items.FirstOrDefault(item => item.Id == itemId);
        }
    }

    public IReadOnlyList<EquipmentOrder> OrdersFor(string customerId)
    {
        lock (_sync)
        {
            return _orders.Where(order => order.CustomerId == customerId).OrderBy(order => order.Id).ToList();
        }
    }

    public IReadOnlyList<EquipmentOrder> AllOrders()
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
            return $"EQU-{_next++}";
        }
    }

    public void Add(EquipmentOrder order)
    {
        lock (_sync)
        {
            _orders.Add(order);
        }
    }
}
