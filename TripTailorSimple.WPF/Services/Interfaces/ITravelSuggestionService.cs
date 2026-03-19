using TripTailorSimple.WPF.Models;

namespace TripTailorSimple.WPF.Services.Interfaces;

public interface ITravelSuggestionService
{
    List<string> BuildPaidActivities(string city, string tripType);
    List<string> BuildFreeActivities(string city);
    List<ActivityPlanItem> BuildPlan(string city, int durationDays);
}
