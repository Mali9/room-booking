namespace RoomBooking.Core.Facilities.Kitchen;

public interface IKitchenRepository
{
    IReadOnlyList<KitchenItem> Items();

    KitchenItem? Find(string itemId);

    IReadOnlyList<KitchenOrder> OrdersFor(string customerId);

    IReadOnlyList<KitchenOrder> AllOrders();

    string NextOrderId();

    void Add(KitchenOrder order);
}
