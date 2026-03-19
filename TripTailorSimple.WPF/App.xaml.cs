using System.Windows;
using TripTailorSimple.WPF.Services;
using TripTailorSimple.WPF.Services.Interfaces;
using TripTailorSimple.WPF.ViewModels;
using TripTailorSimple.WPF.Views;

namespace TripTailorSimple.WPF;

public partial class App : Application
{
    private void Application_Startup(object sender, StartupEventArgs e)
    {
        IDestinationService destinationService = new DestinationService();
        IWeatherService weatherService = new WeatherService();
        ITravelSuggestionService suggestionService = new TravelSuggestionService();
        IContentService contentService = new WikipediaService();

        ITripSearchService tripSearchService = new TripSearchService(
            destinationService,
            weatherService,
            suggestionService,
            contentService);

        var detailViewModel = new TripDetailViewModel(weatherService);
        var mainViewModel = new MainViewModel(tripSearchService, detailViewModel);

        var window = new MainWindow
        {
            DataContext = mainViewModel
        };

        window.Show();
    }
}
