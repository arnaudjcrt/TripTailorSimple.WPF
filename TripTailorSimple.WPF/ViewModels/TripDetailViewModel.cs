using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using TripTailorSimple.WPF.Models;
using TripTailorSimple.WPF.Services.Interfaces;
using TripTailorSimple.WPF.ViewModels.Base;

namespace TripTailorSimple.WPF.ViewModels;

public class TripDetailViewModel : ViewModelBase
{
    private readonly IWeatherService _weatherService;
    private TripProposal? _proposal;

    public TripProposal? Proposal
    {
        get => _proposal;
        private set => SetProperty(ref _proposal, value);
    }

    public ObservableCollection<DailyForecast> Forecast { get; } = new();
    public ICommand BackToResultsCommand { get; }
    public ICommand OpenLinkCommand { get; }

    public event Action? BackRequested;

    public TripDetailViewModel(IWeatherService weatherService)
    {
        _weatherService = weatherService;
        BackToResultsCommand = new RelayCommand(() => BackRequested?.Invoke());
        OpenLinkCommand = new RelayCommand(param =>
        {
            if (param is string url && !string.IsNullOrWhiteSpace(url))
            {
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
        });
    }

    public async Task LoadAsync(TripProposal proposal)
    {
        Proposal = proposal;
        Forecast.Clear();

        var daily = await _weatherService.GetForecastAsync(proposal.Latitude, proposal.Longitude, 5);
        foreach (var item in daily)
        {
            Forecast.Add(item);
        }
    }
}
