namespace RoomBooking.Core.Facilities.Photography;

public interface IPhotographyRepository
{
    IReadOnlyList<PhotographyItem> Items();

    PhotographyItem? Find(string itemId);

    IReadOnlyList<PhotographyOrder> OrdersFor(string customerId);

    IReadOnlyList<PhotographyOrder> AllOrders();

    string NextOrderId();

    void Add(PhotographyOrder order);
}
