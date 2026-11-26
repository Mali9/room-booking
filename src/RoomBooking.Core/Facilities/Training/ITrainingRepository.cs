namespace RoomBooking.Core.Facilities.Training;

public interface ITrainingRepository
{
    IReadOnlyList<TrainingItem> Items();

    TrainingItem? Find(string itemId);

    IReadOnlyList<TrainingOrder> OrdersFor(string customerId);

    IReadOnlyList<TrainingOrder> AllOrders();

    string NextOrderId();

    void Add(TrainingOrder order);
}
