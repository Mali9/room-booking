namespace RoomBooking.Core.Facilities.Wellness;

public interface IWellnessRepository
{
    IReadOnlyList<WellnessItem> Items();

    WellnessItem? Find(string itemId);

    IReadOnlyList<WellnessOrder> OrdersFor(string customerId);

    IReadOnlyList<WellnessOrder> AllOrders();

    string NextOrderId();

    void Add(WellnessOrder order);
}
