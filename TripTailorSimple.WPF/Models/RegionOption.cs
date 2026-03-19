using TripTailorSimple.WPF.ViewModels.Base;

namespace TripTailorSimple.WPF.Models;

public class RegionOption : ViewModelBase
{
    public string Name { get; set; } = string.Empty;

    private bool _isSelected;
    public bool IsSelected
    {
        get => _isSelected;
        set => SetProperty(ref _isSelected, value);
    }
}
