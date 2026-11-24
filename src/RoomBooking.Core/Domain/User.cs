namespace RoomBooking.Core.Domain;

public enum UserRole
{
    Customer,
    Admin,
}

public sealed class User
{
    public required string Id { get; init; }
    public required string Email { get; init; }
    public required string DisplayName { get; init; }
    public required string PasswordHash { get; init; }
    public UserRole Role { get; init; }
}
