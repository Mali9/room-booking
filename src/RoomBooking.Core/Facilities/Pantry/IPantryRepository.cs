namespace RoomBooking.Core.Facilities.Pantry;

public interface IPantryRepository
{
    IReadOnlyList<PantryItem> Items();

    PantryItem? Find(string itemId);

    IReadOnlyList<PantryOrder> OrdersFor(string customerId);

    IReadOnlyList<PantryOrder> AllOrders();

    string NextOrderId();

    void Add(PantryOrder order);
}
