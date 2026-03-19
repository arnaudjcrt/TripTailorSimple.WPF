using TripTailorSimple.WPF.Models;
using TripTailorSimple.WPF.Services.Interfaces;

namespace TripTailorSimple.WPF.Services;

public class TripSearchService : ITripSearchService
{
    private readonly IDestinationService _destinationService;
    private readonly IWeatherService _weatherService;
    private readonly ITravelSuggestionService _travelSuggestionService;
    private readonly IContentService _contentService;

    public TripSearchService(
        IDestinationService destinationService,
        IWeatherService weatherService,
        ITravelSuggestionService travelSuggestionService,
        IContentService contentService)
    {
        _destinationService = destinationService;
        _weatherService = weatherService;
        _travelSuggestionService = travelSuggestionService;
        _contentService = contentService;
    }

    public async Task<List<TripProposal>> SearchAsync(SearchCriteria criteria)
    {
        var destinations = _destinationService.GetDestinations().AsEnumerable();

        if (criteria.Regions.Count > 0)
        {
            destinations = destinations.Where(d => criteria.Regions.Contains(d.Region));
        }

        var results = new List<TripProposal>();
        foreach (var destination in destinations)
        {
            int basePrice = destination.EstimatedPrice;
            int styleMultiplier = criteria.TravelStyle switch
            {
                "Économie" => 85,
                "Luxe" => 145,
                _ => 100
            };

            int flight = criteria.IncludeFlight ? (int)Math.Round(basePrice * 0.35 * styleMultiplier / 100.0) : 0;
            int hotel = criteria.IncludeHotel ? (int)Math.Round(basePrice * 0.45 * styleMultiplier / 100.0 * Math.Max(criteria.DurationDays, 1) / 5.0) : 0;
            int activities = criteria.IncludeActivities ? (int)Math.Round(basePrice * 0.20 * styleMultiplier / 100.0) : 0;
            int total = flight + hotel + activities;

            if (total > criteria.Budget)
            {
                continue;
            }

            var temperature = await _weatherService.GetAverageTemperatureAsync(destination.Latitude, destination.Longitude);
            if (!MatchesClimate(criteria.Climate, temperature))
            {
                continue;
            }

            results.Add(new TripProposal
            {
                City = destination.City,
                Country = destination.Country,
                Region = destination.Region,
                Description = destination.Description,
                Climate = ClassifyClimate(temperature),
                Latitude = destination.Latitude,
                Longitude = destination.Longitude,
                AverageTemperature = temperature,
                EstimatedFlightPrice = flight,
                EstimatedHotelPrice = hotel,
                EstimatedActivityPrice = activities,
                ImageUrl = $"https://picsum.photos/seed/{Uri.EscapeDataString(destination.City)}/600/350",
                PaidActivities = _travelSuggestionService.BuildPaidActivities(destination.City, criteria.TripType),
                FreeActivities = _travelSuggestionService.BuildFreeActivities(destination.City),
                SuggestedPlan = _travelSuggestionService.BuildPlan(destination.City, criteria.DurationDays),
                FlightLink = $"https://www.google.com/travel/flights?q=vol%20pour%20{Uri.EscapeDataString(destination.City)}",
                HotelLink = $"https://www.booking.com/searchresults.fr.html?ss={Uri.EscapeDataString(destination.City)}",
                ActivitiesLink = $"https://www.getyourguide.com/s/?q={Uri.EscapeDataString(destination.City)}",
                WikipediaLink = _contentService.BuildWikipediaLink(destination.City)
            });
        }

        return results.OrderBy(x => x.TotalPrice).ToList();
    }

    private static bool MatchesClimate(string requestedClimate, double averageTemp)
    {
        if (string.IsNullOrWhiteSpace(requestedClimate) || requestedClimate == "Tous")
        {
            return true;
        }

        return requestedClimate switch
        {
            "Froid" => averageTemp < 12,
            "Tempéré" => averageTemp >= 12 && averageTemp <= 24,
            "Chaud" => averageTemp > 24,
            _ => true
        };
    }

    private static string ClassifyClimate(double averageTemp)
    {
        return averageTemp switch
        {
            < 12 => "Froid",
            <= 24 => "Tempéré",
            _ => "Chaud"
        };
    }
}
