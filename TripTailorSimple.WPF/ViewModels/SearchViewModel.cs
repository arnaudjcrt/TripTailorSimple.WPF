using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
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
        new RegionOption { Name = "Amérique" },
        new RegionOption { Name = "Océanie" }
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

    private string _quickRequest = string.Empty;
    public string QuickRequest
    {
        get => _quickRequest;
        set => SetProperty(ref _quickRequest, value);
    }

    public ICommand SelectStyleCommand { get; }
    public ICommand ExtractCriteriaCommand { get; }

    public SearchViewModel()
    {
        SelectStyleCommand = new RelayCommand(parameter =>
        {
            if (parameter is string style)
            {
                SelectedTravelStyle = style;
            }
        });

        ExtractCriteriaCommand = new RelayCommand(ExtractFromQuickRequest);
    }

    private void ExtractFromQuickRequest()
    {
        if (string.IsNullOrWhiteSpace(QuickRequest))
        {
            return;
        }

        string text = QuickRequest.ToLowerInvariant();

        var budgetMatch = Regex.Match(text, @"(\d{3,5})\s*€?");
        if (budgetMatch.Success && int.TryParse(budgetMatch.Groups[1].Value, out int parsedBudget))
        {
            Budget = Math.Clamp(parsedBudget, 200, 20000);
        }

        var daysMatch = Regex.Match(text, @"(\d{1,2})\s*(jour|jours|j)");
        if (daysMatch.Success && int.TryParse(daysMatch.Groups[1].Value, out int parsedDays))
        {
            DurationDays = Math.Clamp(parsedDays, 1, 21);
        }

        if (text.Contains("froid")) SelectedClimate = "Froid";
        else if (text.Contains("chaud") || text.Contains("soleil")) SelectedClimate = "Chaud";
        else if (text.Contains("temp") || text.Contains("doux")) SelectedClimate = "Tempéré";

        if (text.Contains("eco") || text.Contains("pas cher") || text.Contains("économie")) SelectedTravelStyle = "Économie";
        else if (text.Contains("luxe") || text.Contains("premium")) SelectedTravelStyle = "Luxe";
        else if (text.Contains("confort")) SelectedTravelStyle = "Confort";

        if (text.Contains("nature")) SelectedTripType = "Nature";
        else if (text.Contains("culture")) SelectedTripType = "Culture";
        else if (text.Contains("detente") || text.Contains("détente") || text.Contains("relax")) SelectedTripType = "Détente";
        else if (text.Contains("city") || text.Contains("ville")) SelectedTripType = "City Break";

        if (text.Contains("été")) SelectedSeason = "Été";
        else if (text.Contains("hiver")) SelectedSeason = "Hiver";
        else if (text.Contains("printemps")) SelectedSeason = "Printemps";
        else if (text.Contains("automne")) SelectedSeason = "Automne";

        if (text.Contains("oceanie") || text.Contains("océanie")) SelectOnlyRegion("Océanie");
        else if (text.Contains("europe")) SelectOnlyRegion("Europe");
        else if (text.Contains("asie")) SelectOnlyRegion("Asie");
        else if (text.Contains("afrique")) SelectOnlyRegion("Afrique");
        else if (text.Contains("amerique") || text.Contains("amérique")) SelectOnlyRegion("Amérique");
    }

    private void SelectOnlyRegion(string regionName)
    {
        foreach (var region in Regions)
        {
            region.IsSelected = region.Name == regionName;
        }
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
