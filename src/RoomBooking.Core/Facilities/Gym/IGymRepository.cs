namespace RoomBooking.Core.Facilities.Gym;

public interface IGymRepository
{
    IReadOnlyList<GymItem> Items();

    GymItem? Find(string itemId);

    IReadOnlyList<GymOrder> OrdersFor(string customerId);

    IReadOnlyList<GymOrder> AllOrders();

    string NextOrderId();

    void Add(GymOrder order);
}
