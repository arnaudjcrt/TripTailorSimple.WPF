using System.IO;
using System.Text.Json;
using TripTailorSimple.WPF.Models;
using TripTailorSimple.WPF.Services.Interfaces;

namespace TripTailorSimple.WPF.Services;

public class DestinationService : IDestinationService
{
    public List<Destination> GetDestinations()
    {
        string path = Path.Combine(AppContext.BaseDirectory, "Data", "destinations.json");

        if (!File.Exists(path))
            return new List<Destination>();

        string json = File.ReadAllText(path);

        return JsonSerializer.Deserialize<List<Destination>>(json) ?? new List<Destination>();
    }
}
