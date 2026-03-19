using TripTailorSimple.WPF.Models;

namespace TripTailorSimple.WPF.Services.Interfaces;

public interface IWeatherService
{
    Task<double> GetAverageTemperatureAsync(double latitude, double longitude);
    Task<List<DailyForecast>> GetForecastAsync(double latitude, double longitude, int days);
}
