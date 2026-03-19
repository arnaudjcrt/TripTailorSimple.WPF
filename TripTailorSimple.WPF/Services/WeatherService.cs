using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using TripTailorSimple.WPF.Models;
using TripTailorSimple.WPF.Services.Interfaces;

namespace TripTailorSimple.WPF.Services;

public class WeatherService : IWeatherService
{
    private readonly HttpClient _httpClient;

    public WeatherService()
    {
        _httpClient = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(12)
        };

        _httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("TripTailorSimple", "1.0"));
    }

    public async Task<double> GetAverageTemperatureAsync(double latitude, double longitude)
    {
        var forecast = await GetForecastAsync(latitude, longitude, 3);
        if (forecast.Count == 0)
        {
            return BuildFallbackTemperature(latitude);
        }

        return Math.Round(forecast.Average(x => (x.TempMin + x.TempMax) / 2.0), 1);
    }

    public async Task<List<DailyForecast>> GetForecastAsync(double latitude, double longitude, int days)
    {
        int safeDays = Math.Clamp(days, 1, 14);
        string lat = latitude.ToString(CultureInfo.InvariantCulture);
        string lon = longitude.ToString(CultureInfo.InvariantCulture);

        try
        {
            string url = $"https://api.open-meteo.com/v1/forecast?latitude={lat}&longitude={lon}&daily=temperature_2m_max,temperature_2m_min&forecast_days={safeDays}&timezone=auto";
            string json = await _httpClient.GetStringAsync(url);

            using JsonDocument doc = JsonDocument.Parse(json);
            if (!doc.RootElement.TryGetProperty("daily", out JsonElement daily))
            {
                return BuildFallbackForecast(latitude, safeDays);
            }

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

            return result.Count > 0 ? result : BuildFallbackForecast(latitude, safeDays);
        }
        catch
        {
            return BuildFallbackForecast(latitude, safeDays);
        }
    }

    private static List<DailyForecast> BuildFallbackForecast(double latitude, int days)
    {
        double baseTemp = BuildFallbackTemperature(latitude);
        var forecasts = new List<DailyForecast>();

        for (int i = 0; i < days; i++)
        {
            DateTime day = DateTime.UtcNow.Date.AddDays(i);
            double shift = (i % 3) - 1; // -1,0,1
            forecasts.Add(new DailyForecast
            {
                Date = day.ToString("yyyy-MM-dd"),
                TempMin = Math.Round(baseTemp - 4 + shift, 1),
                TempMax = Math.Round(baseTemp + 4 + shift, 1)
            });
        }

        return forecasts;
    }

    private static double BuildFallbackTemperature(double latitude)
    {
        // Approximation simple basée sur la latitude quand l'API est indisponible.
        double absLat = Math.Abs(latitude);
        return absLat switch
        {
            < 15 => 29,
            < 30 => 24,
            < 45 => 18,
            < 60 => 11,
            _ => 4
        };
    }
}
