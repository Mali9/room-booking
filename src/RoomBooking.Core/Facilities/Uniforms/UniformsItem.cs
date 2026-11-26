namespace RoomBooking.Core.Facilities.Uniforms;

/// <summary>Something customers can order from Uniforms.</summary>
public sealed class UniformsItem
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public decimal UnitPrice { get; init; }
    public int Capacity { get; init; }
}
