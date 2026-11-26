namespace RoomBooking.Core.Facilities.Shared;

/// <summary>A facility rule rejected the request. The message is safe to show.</summary>
public sealed class FacilityRuleException(string message) : Exception(message);
