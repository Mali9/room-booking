using RoomBooking.Core.Facilities.Training;

namespace RoomBooking.Infrastructure.Facilities.Training;

/// <summary>Seed data of Training: three items and two orders of Customer Sara.</summary>
public static class TrainingSeed
{
    public static List<TrainingItem> Items() =>
    [
        new() { Id = "TR3-A", Name = "Training Standard", UnitPrice = 205m, Capacity = 50 },
        new() { Id = "TR3-B", Name = "Training Premium", UnitPrice = 219m, Capacity = 30 },
        new() { Id = "TR3-C", Name = "Training Limited", UnitPrice = 108m, Capacity = 3 },
    ];

    public static List<TrainingOrder> Orders() =>
    [
        new() { Id = "TR3-1001", CustomerId = "U-2", ItemId = "TR3-A", Quantity = 2, Total = 589.38m },
        new() { Id = "TR3-1002", CustomerId = "U-2", ItemId = "TR3-B", Quantity = 1, Total = 314.81m },
    ];
}
