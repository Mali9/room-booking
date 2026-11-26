using RoomBooking.Core.Facilities.Training;
using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Infrastructure.Facilities.Training;

public sealed class InMemoryTrainingRepository : ITrainingRepository, IResettable
{
    private readonly object _sync = new();
    private List<TrainingItem> _items = [];
    private List<TrainingOrder> _orders = [];
    private int _next;

    public InMemoryTrainingRepository() => Reset();

    public void Reset()
    {
        lock (_sync)
        {
            _items = TrainingSeed.Items();
            _orders = TrainingSeed.Orders();
            _next = 1001 + _orders.Count;
        }
    }

    public IReadOnlyList<TrainingItem> Items()
    {
        lock (_sync)
        {
            return _items.ToList();
        }
    }

    public TrainingItem? Find(string itemId)
    {
        lock (_sync)
        {
            return _items.FirstOrDefault(item => item.Id == itemId);
        }
    }

    public IReadOnlyList<TrainingOrder> OrdersFor(string customerId)
    {
        lock (_sync)
        {
            var mine = _orders.Where(order => order.CustomerId == customerId);
            return [.. mine.OrderBy(order => order.Id)];
        }
    }

    public IReadOnlyList<TrainingOrder> AllOrders()
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
            return $"TR3-{_next++}";
        }
    }

    public void Add(TrainingOrder order)
    {
        lock (_sync)
        {
            _orders.Add(order);
        }
    }
}
