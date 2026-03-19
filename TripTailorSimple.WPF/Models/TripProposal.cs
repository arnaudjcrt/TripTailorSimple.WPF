namespace TripTailorSimple.WPF.Models;

public class TripProposal
{
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Climate { get; set; } = "Tempéré";
    public string ImageUrl { get; set; } = "https://picsum.photos/seed/triptailor/600/350";

    public int EstimatedFlightPrice { get; set; }
    public int EstimatedHotelPrice { get; set; }
    public int EstimatedActivityPrice { get; set; }
    public int TotalPrice => EstimatedFlightPrice + EstimatedHotelPrice + EstimatedActivityPrice;

    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double AverageTemperature { get; set; }

    public List<string> PaidActivities { get; set; } = new();
    public List<string> FreeActivities { get; set; } = new();
    public List<ActivityPlanItem> SuggestedPlan { get; set; } = new();

    public string FlightLink { get; set; } = string.Empty;
    public string HotelLink { get; set; } = string.Empty;
    public string ActivitiesLink { get; set; } = string.Empty;
    public string WikipediaLink { get; set; } = string.Empty;
}
