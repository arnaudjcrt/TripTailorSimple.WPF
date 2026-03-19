using System.Net.Http;
using System.Text.Json;
using TripTailorSimple.WPF.Models;
using TripTailorSimple.WPF.Services.Interfaces;

namespace TripTailorSimple.WPF.Services;

public class WeatherService : IWeatherService
{
    private readonly HttpClient _httpClient = new();

    public async Task<double> GetAverageTemperatureAsync(double latitude, double longitude)
    {
        var forecast = await GetForecastAsync(latitude, longitude, 3);
        if (forecast.Count == 0)
        {
            return 0;
        }

        return Math.Round(forecast.Average(x => (x.TempMin + x.TempMax) / 2.0), 1);
    }

    public async Task<List<DailyForecast>> GetForecastAsync(double latitude, double longitude, int days)
    {
        try
        {
            string url = $"https://api.open-meteo.com/v1/forecast?latitude={latitude}&longitude={longitude}&daily=temperature_2m_max,temperature_2m_min&forecast_days={Math.Clamp(days, 1, 14)}&timezone=auto";
            string json = await _httpClient.GetStringAsync(url);

            using JsonDocument doc = JsonDocument.Parse(json);
            var daily = doc.RootElement.GetProperty("daily");

            var dates = daily.GetProperty("time").EnumerateArray().Select(x => x.GetString() ?? string.Empty).ToList();
            var maxTemps = daily.GetProperty("temperature_2m_max").EnumerateArray().Select(x => x.GetDouble()).ToList();
            var minTemps = daily.GetProperty("temperature_2m_min").EnumerateArray().Select(x => x.GetDouble()).ToList();

            var result = new List<DailyForecast>();
            for (int i = 0; i < dates.Count && i < maxTemps.Count && i < minTemps.Count; i++)
            {
                result.Add(new DailyForecast
                {
                    Date = dates[i],
                    TempMin = Math.Round(minTemps[i], 1),
                    TempMax = Math.Round(maxTemps[i], 1)
                });
            }

            return result;
        }
        catch
        {
            return new List<DailyForecast>();
        }
    }
}
