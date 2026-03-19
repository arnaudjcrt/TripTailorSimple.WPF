using System.Collections.ObjectModel;
using System.Windows.Input;
using TripTailorSimple.WPF.Models;
using TripTailorSimple.WPF.ViewModels.Base;

namespace TripTailorSimple.WPF.ViewModels;

public class ResultsViewModel : ViewModelBase
{
    public ObservableCollection<DestinationCardViewModel> Results { get; } = new();

    public ICommand OpenDetailCommand { get; }
    public ICommand BackToSearchCommand { get; }

    public event Action<TripProposal>? DetailRequested;
    public event Action? BackRequested;

    public ResultsViewModel()
    {
        OpenDetailCommand = new RelayCommand(param =>
        {
            if (param is DestinationCardViewModel card)
            {
                DetailRequested?.Invoke(card.Proposal);
            }
        });

        BackToSearchCommand = new RelayCommand(() => BackRequested?.Invoke());
    }

    public bool HasResults => Results.Count > 0;

    public void SetResults(IEnumerable<TripProposal> proposals)
    {
        Results.Clear();
        foreach (var proposal in proposals)
        {
            Results.Add(new DestinationCardViewModel(proposal));
        }

        OnPropertyChanged(nameof(HasResults));
    }
}
