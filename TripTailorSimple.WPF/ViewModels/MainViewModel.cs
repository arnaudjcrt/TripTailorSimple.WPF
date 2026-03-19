using System.Windows.Input;
using TripTailorSimple.WPF.Models;
using TripTailorSimple.WPF.Services.Interfaces;
using TripTailorSimple.WPF.ViewModels.Base;

namespace TripTailorSimple.WPF.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly ITripSearchService _tripSearchService;
    private readonly TripDetailViewModel _detailViewModel;

    public SearchViewModel SearchViewModel { get; }
    public ResultsViewModel ResultsViewModel { get; }

    private ViewModelBase _currentPageViewModel;
    public ViewModelBase CurrentPageViewModel
    {
        get => _currentPageViewModel;
        set => SetProperty(ref _currentPageViewModel, value);
    }

    private bool _isLoading;
    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    public string StatusMessage => IsLoading ? "Recherche en cours..." : string.Empty;

    public ICommand SearchCommand { get; }

    public MainViewModel(ITripSearchService tripSearchService, TripDetailViewModel detailViewModel)
    {
        _tripSearchService = tripSearchService;
        _detailViewModel = detailViewModel;

        SearchViewModel = new SearchViewModel();
        ResultsViewModel = new ResultsViewModel();

        _currentPageViewModel = SearchViewModel;

        SearchCommand = new RelayCommand(async () => await ExecuteSearchAsync(), () => !IsLoading);

        ResultsViewModel.DetailRequested += async proposal => await OpenDetailAsync(proposal);
        ResultsViewModel.BackRequested += () => CurrentPageViewModel = SearchViewModel;
        _detailViewModel.BackRequested += () => CurrentPageViewModel = ResultsViewModel;
    }

    private async Task ExecuteSearchAsync()
    {
        IsLoading = true;
        OnPropertyChanged(nameof(StatusMessage));

        try
        {
            SearchCriteria criteria = SearchViewModel.BuildCriteria();
            var proposals = await _tripSearchService.SearchAsync(criteria);
            ResultsViewModel.SetResults(proposals);
            CurrentPageViewModel = ResultsViewModel;
        }
        finally
        {
            IsLoading = false;
            OnPropertyChanged(nameof(StatusMessage));
        }
    }

    private async Task OpenDetailAsync(TripProposal proposal)
    {
        await _detailViewModel.LoadAsync(proposal);
        CurrentPageViewModel = _detailViewModel;
    }
}
