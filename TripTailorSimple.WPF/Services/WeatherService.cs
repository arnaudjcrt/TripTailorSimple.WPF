using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TripTailorSimple.WPF.Services;
public class WeatherService
{
    private readonly HttpClient _httpClient = new();

    public async Task<double> GetAverageTemperatureAsync(double latitude, double longitude)
    {
        try
        {
            string url =
                $"https://api.open-meteo.com/v1/forecast?latitude={latitude}&longitude={longitude}&daily=temperature_2m_max,temperature_2m_min&forecast_days=3&timezone=auto";

            string json = await _httpClient.GetStringAsync(url);

            using JsonDocument doc = JsonDocument.Parse(json);

            var daily = doc.RootElement.GetProperty("daily");

            var maxTemps = daily.GetProperty("temperature_2m_max")
                                .EnumerateArray()
                                .Select(x => x.GetDouble())
                                .ToList();

            var minTemps = daily.GetProperty("temperature_2m_min")
                                .EnumerateArray()
                                .Select(x => x.GetDouble())
                                .ToList();

            if (maxTemps.Count == 0 || minTemps.Count == 0)
                return 0;

            double average = maxTemps.Zip(minTemps, (max, min) => (max + min) / 2.0).Average();

            return Math.Round(average, 1);
        }
        catch
        {
            return 0;
        }
    }
}
