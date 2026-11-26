namespace RoomBooking.Core.Facilities.Signage;

public interface ISignageRepository
{
    IReadOnlyList<SignageItem> Items();

    SignageItem? Find(string itemId);

    IReadOnlyList<SignageOrder> OrdersFor(string customerId);

    IReadOnlyList<SignageOrder> AllOrders();

    string NextOrderId();

    void Add(SignageOrder order);
}
