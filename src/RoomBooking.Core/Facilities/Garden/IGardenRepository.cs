namespace RoomBooking.Core.Facilities.Garden;

public interface IGardenRepository
{
    IReadOnlyList<GardenItem> Items();

    GardenItem? Find(string itemId);

    IReadOnlyList<GardenOrder> OrdersFor(string customerId);

    IReadOnlyList<GardenOrder> AllOrders();

    string NextOrderId();

    void Add(GardenOrder order);
}
