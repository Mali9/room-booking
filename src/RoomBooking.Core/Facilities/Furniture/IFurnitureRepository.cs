namespace RoomBooking.Core.Facilities.Furniture;

public interface IFurnitureRepository
{
    IReadOnlyList<FurnitureItem> Items();

    FurnitureItem? Find(string itemId);

    IReadOnlyList<FurnitureOrder> OrdersFor(string customerId);

    IReadOnlyList<FurnitureOrder> AllOrders();

    string NextOrderId();

    void Add(FurnitureOrder order);
}
