namespace TripTailorSimple.WPF.Models;

public class DailyForecast
{
    public string Date { get; set; } = string.Empty;
    public double TempMin { get; set; }
    public double TempMax { get; set; }
    public string Summary => $"{TempMin:0.#}°C / {TempMax:0.#}°C";
}
