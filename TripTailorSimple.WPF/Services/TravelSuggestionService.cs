using TripTailorSimple.WPF.Models;
using TripTailorSimple.WPF.Services.Interfaces;

namespace TripTailorSimple.WPF.Services;

public class TravelSuggestionService : ITravelSuggestionService
{
    public List<string> BuildPaidActivities(string city, string tripType)
    {
        return new List<string>
        {
            $"Visite guidée {tripType.ToLower()} à {city}",
            "Pass musées et monuments",
            "Excursion locale d'une demi-journée"
        };
    }

    public List<string> BuildFreeActivities(string city)
    {
        return new List<string>
        {
            $"Balade dans les quartiers historiques de {city}",
            "Parcs, jardins et points de vue",
            "Marchés locaux et ruelles typiques"
        };
    }

    public List<ActivityPlanItem> BuildPlan(string city, int durationDays)
    {
        var plan = new List<ActivityPlanItem>();
        for (int day = 1; day <= Math.Max(durationDays, 1); day++)
        {
            plan.Add(new ActivityPlanItem
            {
                DayNumber = day,
                Morning = $"Café et visite d'un site phare de {city}",
                Afternoon = "Découverte culturelle et quartier local",
                Evening = "Dîner + promenade panoramique"
            });
        }

        return plan;
    }
}
