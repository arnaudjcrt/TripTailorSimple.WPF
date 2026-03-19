using TripTailorSimple.WPF.Services.Interfaces;

namespace TripTailorSimple.WPF.Services;

public class WikipediaService : IContentService
{
    public string BuildWikipediaLink(string city)
    {
        return $"https://fr.wikipedia.org/wiki/{Uri.EscapeDataString(city)}";
    }
}
