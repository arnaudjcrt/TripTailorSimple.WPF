using TripTailorSimple.WPF.Models;

namespace TripTailorSimple.WPF.Services.Interfaces;

public interface IDestinationService
{
    List<Destination> GetDestinations();
}
