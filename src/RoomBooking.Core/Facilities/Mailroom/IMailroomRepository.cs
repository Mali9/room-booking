namespace RoomBooking.Core.Facilities.Mailroom;

public interface IMailroomRepository
{
    IReadOnlyList<MailroomItem> Items();

    MailroomItem? Find(string itemId);

    IReadOnlyList<MailroomOrder> OrdersFor(string customerId);

    IReadOnlyList<MailroomOrder> AllOrders();

    string NextOrderId();

    void Add(MailroomOrder order);
}
