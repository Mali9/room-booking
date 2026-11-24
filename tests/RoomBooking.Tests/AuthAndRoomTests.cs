using RoomBooking.Core;
using RoomBooking.Core.Domain;
using RoomBooking.Infrastructure;

namespace RoomBooking.Tests;

public sealed class AuthTests
{
    private readonly TestApp _app = new();

    [Fact]
    [Trait("TestCase", "TC-001")]
    [Trait("Requirement", "REQ-01")]
    public void Valid_customer_credentials_sign_in()
    {
        var user = _app.Auth.ValidateCredentials(SeedData.SaraEmail, SeedData.SaraPassword);

        Assert.NotNull(user);
        Assert.Equal("Sara Al-Harbi", user.DisplayName);
        Assert.Equal(UserRole.Customer, user.Role);
    }

    [Fact]
    [Trait("TestCase", "TC-002")]
    [Trait("Requirement", "REQ-01")]
    public void Wrong_password_is_rejected()
    {
        Assert.Null(_app.Auth.ValidateCredentials(SeedData.SaraEmail, "wrong-password"));
    }
}

public sealed class RoomTests
{
    private readonly TestApp _app = new();

    [Fact]
    [Trait("TestCase", "TC-004")]
    [Trait("Requirement", "REQ-02")]
    public void Customer_catalogue_hides_inactive_rooms()
    {
        var names = _app.RoomService.GetCatalogue(includeInactive: false).Select(room => room.Name);

        Assert.Equal(["Al Nakheel", "Al Waha", "Board Room"], names);
    }

    [Fact]
    [Trait("TestCase", "TC-005")]
    [Trait("Requirement", "REQ-02")]
    public void Admin_catalogue_includes_inactive_rooms()
    {
        var rooms = _app.RoomService.GetCatalogue(includeInactive: true);

        Assert.Equal(4, rooms.Count);
        Assert.False(rooms.Single(room => room.Name == "Focus Pod").IsActive);
    }

    [Fact]
    [Trait("TestCase", "TC-006")]
    [Trait("Requirement", "REQ-03")]
    public void Admin_creates_room_with_next_id()
    {
        var room = _app.RoomService.Create("Al Rawdah", 8, 150m);

        Assert.Equal("R-401", room.Id);
        Assert.True(room.IsActive);
        Assert.Contains(_app.RoomService.GetCatalogue(false), item => item.Name == "Al Rawdah");
    }

    [Fact]
    [Trait("TestCase", "TC-007")]
    [Trait("Requirement", "REQ-03")]
    public void Capacity_zero_is_rejected()
    {
        var error = Assert.Throws<BusinessRuleException>(() => _app.RoomService.Create("Al Rawdah", 0, 150m));

        Assert.Equal("Capacity must be between 1 and 50.", error.Message);
    }

    [Fact]
    [Trait("TestCase", "TC-008")]
    [Trait("Requirement", "REQ-03")]
    public void Duplicate_room_name_is_rejected()
    {
        var error = Assert.Throws<BusinessRuleException>(() => _app.RoomService.Create("Al Waha", 10, 150m));

        Assert.Equal("A room named 'Al Waha' already exists.", error.Message);
    }

    [Fact]
    [Trait("TestCase", "TC-009")]
    [Trait("Requirement", "REQ-03")]
    public void Admin_updates_hourly_rate()
    {
        _app.RoomService.Update("R-101", "Al Nakheel", 6, 120m);

        Assert.Equal(120m, _app.RoomService.Get("R-101").HourlyRate);
    }

    [Fact]
    [Trait("TestCase", "TC-010")]
    [Trait("Requirement", "REQ-04")]
    public void Room_with_upcoming_bookings_cannot_be_deactivated()
    {
        var error = Assert.Throws<BusinessRuleException>(() => _app.RoomService.SetActive("R-201", false));

        Assert.Equal("Room Board Room has upcoming bookings and cannot be deactivated.", error.Message);
        Assert.True(_app.RoomService.Get("R-201").IsActive);
    }

    [Fact]
    [Trait("TestCase", "TC-011")]
    [Trait("Requirement", "REQ-04")]
    public void Inactive_room_can_be_activated()
    {
        _app.RoomService.SetActive("R-301", true);

        Assert.True(_app.RoomService.Get("R-301").IsActive);
    }
}
