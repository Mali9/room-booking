namespace RoomBooking.Core.Facilities.Uniforms;

public interface IUniformsRepository
{
    IReadOnlyList<UniformsItem> Items();

    UniformsItem? Find(string itemId);

    IReadOnlyList<UniformsOrder> OrdersFor(string customerId);

    IReadOnlyList<UniformsOrder> AllOrders();

    string NextOrderId();

    void Add(UniformsOrder order);
}
