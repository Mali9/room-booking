namespace RoomBooking.Core.Facilities.Shared;

/// <summary>Every facility and the page that opens it.</summary>
public static class FacilityCatalog
{
    public static IReadOnlyList<(string Name, string Path)> All { get; } =
    [
        ("Catering", "/Catering"),
        ("Parking", "/Parking"),
        ("Equipment", "/Equipment"),
        ("Visitors", "/Visitors"),
        ("Cleaning", "/Cleaning"),
        ("Maintenance", "/Maintenance"),
        ("Security", "/Security"),
        ("Lockers", "/Lockers"),
        ("Desks", "/Desks"),
        ("Events", "/Events"),
        ("Printing", "/Printing"),
        ("Shuttle", "/Shuttle"),
        ("Lounge", "/Lounge"),
        ("Gym", "/Gym"),
        ("Childcare", "/Childcare"),
        ("Laundry", "/Laundry"),
        ("Mailroom", "/Mailroom"),
        ("Storage", "/Storage"),
        ("Wellness", "/Wellness"),
        ("Library", "/Library"),
        ("Labs", "/Labs"),
        ("Studio", "/Studio"),
        ("Kitchen", "/Kitchen"),
        ("Garden", "/Garden"),
        ("Charging", "/Charging"),
        ("Bikes", "/Bikes"),
        ("Courier", "/Courier"),
        ("Translation", "/Translation"),
        ("ItSupport", "/ItSupport"),
        ("Furniture", "/Furniture"),
        ("Signage", "/Signage"),
        ("Photography", "/Photography"),
        ("Recording", "/Recording"),
        ("Training", "/Training"),
        ("Concierge", "/Concierge"),
        ("Pantry", "/Pantry"),
        ("Uniforms", "/Uniforms"),
        ("Keys", "/Keys"),
        ("Badges", "/Badges"),
        ("Archive", "/Archive"),
    ];
}
