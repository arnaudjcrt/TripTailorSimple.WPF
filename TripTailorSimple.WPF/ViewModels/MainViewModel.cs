using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TripTailorSimple.WPF.Models;
using TripTailorSimple.WPF.Services;

namespace TripTailorSimple.WPF.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    private readonly DestinationService _destinationService;
    private readonly WeatherService _weatherService;

    public ObservableCollection<TripResult> Results { get; set; } = new();

    public List<string> Regions { get; } = new() { "Toutes", "Europe", "Afrique", "Asie", "Amérique" };

    private string _selectedRegion = "Toutes";
    public string SelectedRegion
    {
        get => _selectedRegion;
        set
        {
            _selectedRegion = value;
            OnPropertyChanged();
        }
    }

    private int _budget = 1000;
    public int Budget
    {
        get => _budget;
        set
        {
            _budget = value;
            OnPropertyChanged();
        }
    }

    public ICommand SearchCommand { get; }

    public MainViewModel()
    {
        _destinationService = new DestinationService();
        _weatherService = new WeatherService();

        SearchCommand = new RelayCommand(async () => await SearchAsync());
    }

    private async Task SearchAsync()
    {
        Results.Clear();

        List<Destination> destinations = _destinationService.GetDestinations();

        if (SelectedRegion != "Toutes")
        {
            destinations = destinations.Where(d => d.Region == SelectedRegion).ToList();
        }

        destinations = destinations.Where(d => d.EstimatedPrice <= Budget).ToList();

        foreach (var destination in destinations)
        {
            double temp = await _weatherService.GetAverageTemperatureAsync(destination.Latitude, destination.Longitude);

            Results.Add(new TripResult
            {
                City = destination.City,
                Country = destination.Country,
                Region = destination.Region,
                Description = destination.Description,
                EstimatedPrice = destination.EstimatedPrice,
                AverageTemperature = temp
            });
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}