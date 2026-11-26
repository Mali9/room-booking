namespace RoomBooking.Core.Facilities.Studio;

public interface IStudioRepository
{
    IReadOnlyList<StudioItem> Items();

    StudioItem? Find(string itemId);

    IReadOnlyList<StudioOrder> OrdersFor(string customerId);

    IReadOnlyList<StudioOrder> AllOrders();

    string NextOrderId();

    void Add(StudioOrder order);
}
