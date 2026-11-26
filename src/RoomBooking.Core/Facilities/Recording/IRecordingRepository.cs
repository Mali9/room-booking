namespace RoomBooking.Core.Facilities.Recording;

public interface IRecordingRepository
{
    IReadOnlyList<RecordingItem> Items();

    RecordingItem? Find(string itemId);

    IReadOnlyList<RecordingOrder> OrdersFor(string customerId);

    IReadOnlyList<RecordingOrder> AllOrders();

    string NextOrderId();

    void Add(RecordingOrder order);
}
