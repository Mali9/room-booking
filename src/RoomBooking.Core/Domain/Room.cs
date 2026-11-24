namespace RoomBooking.Core.Domain;

public sealed class Room
{
    public required string Id { get; init; }
    public required string Name { get; set; }
    public int Capacity { get; set; }
    public decimal HourlyRate { get; set; }
    public bool IsActive { get; set; } = true;
}
