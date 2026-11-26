namespace RoomBooking.Core.Facilities.Parking;

public interface IParkingRepository
{
    IReadOnlyList<ParkingItem> Items();

    ParkingItem? Find(string itemId);

    IReadOnlyList<ParkingOrder> OrdersFor(string customerId);

    IReadOnlyList<ParkingOrder> AllOrders();

    string NextOrderId();

    void Add(ParkingOrder order);
}
