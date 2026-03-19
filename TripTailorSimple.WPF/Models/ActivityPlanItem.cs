namespace TripTailorSimple.WPF.Models;

public class ActivityPlanItem
{
    public int DayNumber { get; set; }
    public string Morning { get; set; } = string.Empty;
    public string Afternoon { get; set; } = string.Empty;
    public string Evening { get; set; } = string.Empty;
}
