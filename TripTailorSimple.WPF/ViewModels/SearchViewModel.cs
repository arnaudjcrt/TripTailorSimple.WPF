using System.Collections.ObjectModel;
using System.Windows.Input;
using TripTailorSimple.WPF.Models;
using TripTailorSimple.WPF.ViewModels.Base;

namespace TripTailorSimple.WPF.ViewModels;

public class SearchViewModel : ViewModelBase
{
    public ObservableCollection<RegionOption> Regions { get; } = new()
    {
        new RegionOption { Name = "Europe", IsSelected = true },
        new RegionOption { Name = "Afrique" },
        new RegionOption { Name = "Asie" },
        new RegionOption { Name = "Amérique" }
    };

    public ObservableCollection<string> Seasons { get; } = new() { "Printemps", "Été", "Automne", "Hiver" };
    public ObservableCollection<string> Climates { get; } = new() { "Tous", "Froid", "Tempéré", "Chaud" };
    public ObservableCollection<string> TripTypes { get; } = new() { "City Break", "Nature", "Culture", "Détente" };

    private int _budget = 1500;
    public int Budget { get => _budget; set => SetProperty(ref _budget, value); }

    private int _durationDays = 5;
    public int DurationDays { get => _durationDays; set => SetProperty(ref _durationDays, value); }

    private string _selectedSeason = "Été";
    public string SelectedSeason { get => _selectedSeason; set => SetProperty(ref _selectedSeason, value); }

    private string _selectedClimate = "Tous";
    public string SelectedClimate { get => _selectedClimate; set => SetProperty(ref _selectedClimate, value); }

    private string _selectedTripType = "City Break";
    public string SelectedTripType { get => _selectedTripType; set => SetProperty(ref _selectedTripType, value); }

    private string _selectedTravelStyle = "Confort";
    public string SelectedTravelStyle { get => _selectedTravelStyle; set => SetProperty(ref _selectedTravelStyle, value); }

    private bool _includeFlight = true;
    public bool IncludeFlight { get => _includeFlight; set => SetProperty(ref _includeFlight, value); }

    private bool _includeHotel = true;
    public bool IncludeHotel { get => _includeHotel; set => SetProperty(ref _includeHotel, value); }

    private bool _includeActivities = true;
    public bool IncludeActivities { get => _includeActivities; set => SetProperty(ref _includeActivities, value); }

    public ICommand SelectStyleCommand { get; }

    public SearchViewModel()
    {
        SelectStyleCommand = new RelayCommand(parameter =>
        {
            if (parameter is string style)
            {
                SelectedTravelStyle = style;
            }
        });
    }

    public SearchCriteria BuildCriteria()
    {
        return new SearchCriteria
        {
            Budget = Budget,
            DurationDays = DurationDays,
            Season = SelectedSeason,
            Climate = SelectedClimate,
            TripType = SelectedTripType,
            TravelStyle = SelectedTravelStyle,
            IncludeFlight = IncludeFlight,
            IncludeHotel = IncludeHotel,
            IncludeActivities = IncludeActivities,
            Regions = Regions.Where(x => x.IsSelected).Select(x => x.Name).ToList()
        };
    }
}
