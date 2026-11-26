using RoomBooking.Core.Facilities.Maintenance;
using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Infrastructure.Facilities.Maintenance;

public sealed class InMemoryMaintenanceRepository : IMaintenanceRepository, IResettable
{
    private readonly object _sync = new();
    private List<MaintenanceItem> _items = [];
    private List<MaintenanceOrder> _orders = [];
    private int _next;

    public InMemoryMaintenanceRepository() => Reset();

    public void Reset()
    {
        lock (_sync)
        {
            _items = MaintenanceSeed.Items();
            _orders = MaintenanceSeed.Orders();
            _next = 1001 + _orders.Count;
        }
    }

    public IReadOnlyList<MaintenanceItem> Items()
    {
        lock (_sync)
        {
            return _items.ToList();
        }
    }

    public MaintenanceItem? Find(string itemId)
    {
        lock (_sync)
        {
            return _items.FirstOrDefault(item => item.Id == itemId);
        }
    }

    public IReadOnlyList<MaintenanceOrder> OrdersFor(string customerId)
    {
        lock (_sync)
        {
            return _orders.Where(order => order.CustomerId == customerId).OrderBy(order => order.Id).ToList();
        }
    }

    public IReadOnlyList<MaintenanceOrder> AllOrders()
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
            return $"MAI-{_next++}";
        }
    }

    public void Add(MaintenanceOrder order)
    {
        lock (_sync)
        {
            _orders.Add(order);
        }
    }
}
