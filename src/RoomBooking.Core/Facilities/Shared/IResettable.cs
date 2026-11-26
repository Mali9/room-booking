namespace RoomBooking.Core.Facilities.Shared;

/// <summary>A store that returns to its seed on test reset.</summary>
public interface IResettable
{
    void Reset();
}
