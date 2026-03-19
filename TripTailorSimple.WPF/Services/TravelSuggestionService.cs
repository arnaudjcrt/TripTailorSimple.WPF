using TripTailorSimple.WPF.Models;
using TripTailorSimple.WPF.Services.Interfaces;

namespace TripTailorSimple.WPF.Services;

public class TravelSuggestionService : ITravelSuggestionService
{
    private static readonly string[] PaidMorningTemplates =
    {
        "Visite guidée premium du centre-ville",
        "Session food tour avec guide local",
        "Entrée coupe-file pour un monument iconique",
        "Excursion photo accompagnée"
    };

    private static readonly string[] PaidAfternoonTemplates =
    {
        "Atelier local (artisanat ou cuisine)",
        "Excursion en bateau / transport panoramique",
        "Pass musées avec audio-guide",
        "Tour street-art et quartiers créatifs"
    };

    private static readonly string[] PaidEveningTemplates =
    {
        "Dîner signature + vue panoramique",
        "Spectacle culturel en soirée",
        "Croisière nocturne ou rooftop experience",
        "Expérience gastronomique typique"
    };

    private static readonly string[] FreeMorningTemplates =
    {
        "Balade photo au lever du soleil",
        "Parcs et jardins emblématiques",
        "Marché local et cafés de quartier",
        "Circuit architecture à pied"
    };

    private static readonly string[] FreeAfternoonTemplates =
    {
        "Quartiers historiques et ruelles cachées",
        "Musées gratuits / galeries publiques",
        "Parcours street art et places locales",
        "Point de vue nature accessible à pied"
    };

    private static readonly string[] FreeEveningTemplates =
    {
        "Promenade sunset sur les quais",
        "Ambiance locale dans une place animée",
        "Coucher de soleil au meilleur viewpoint",
        "Concert gratuit / animation urbaine"
    };

    public List<string> BuildPaidActivities(string city, string tripType)
    {
        return new List<string>
        {
            $"{tripType} à {city} : {PaidMorningTemplates[0]}",
            $"{tripType} à {city} : {PaidAfternoonTemplates[1]}",
            $"{tripType} à {city} : {PaidEveningTemplates[2]}"
        };
    }

    public List<string> BuildFreeActivities(string city)
    {
        return new List<string>
        {
            $"{city} : {FreeMorningTemplates[0]}",
            $"{city} : {FreeAfternoonTemplates[2]}",
            $"{city} : {FreeEveningTemplates[1]}"
        };
    }

    public List<ActivityPlanItem> BuildPlan(string city, int durationDays)
    {
        var plan = new List<ActivityPlanItem>();
        int totalDays = Math.Max(durationDays, 1);

        int seed = Math.Abs(city.GetHashCode());

        for (int day = 1; day <= totalDays; day++)
        {
            int dayOffset = seed + day * 7;
            string morning = day % 2 == 0
                ? $"{city} : {FreeMorningTemplates[dayOffset % FreeMorningTemplates.Length]}"
                : $"{city} : {PaidMorningTemplates[dayOffset % PaidMorningTemplates.Length]}";

            string afternoon = day % 3 == 0
                ? $"{city} : {FreeAfternoonTemplates[dayOffset % FreeAfternoonTemplates.Length]}"
                : $"{city} : {PaidAfternoonTemplates[dayOffset % PaidAfternoonTemplates.Length]}";

            string evening = day % 2 == 0
                ? $"{city} : {PaidEveningTemplates[dayOffset % PaidEveningTemplates.Length]}"
                : $"{city} : {FreeEveningTemplates[dayOffset % FreeEveningTemplates.Length]}";

            plan.Add(new ActivityPlanItem
            {
                DayNumber = day,
                Morning = morning,
                Afternoon = afternoon,
                Evening = evening
            });
        }

        return plan;
    }
}
