namespace TripTailorSimple.WPF.Models;

public class SearchCriteria
{
    public int Budget { get; set; } = 1000;
    public int DurationDays { get; set; } = 5;
    public string Season { get; set; } = "Été";
    public string Climate { get; set; } = "Tempéré";
    public string TravelStyle { get; set; } = "Confort";
    public string TripType { get; set; } = "City Break";
    public bool IncludeFlight { get; set; } = true;
    public bool IncludeHotel { get; set; } = true;
    public bool IncludeActivities { get; set; } = true;
    public IReadOnlyList<string> Regions { get; set; } = Array.Empty<string>();
}
