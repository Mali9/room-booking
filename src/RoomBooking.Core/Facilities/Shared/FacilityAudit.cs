namespace RoomBooking.Core.Facilities.Shared;

/// <summary>Records what happened in every facility, for operations.</summary>
public sealed class FacilityAudit
{
    private readonly List<string> _entries = [];

    public IReadOnlyList<string> Entries => _entries;

    public void Record(string entry)
    {
        lock (_entries)
        {
            _entries.Add(entry);
        }
    }
}
