using TripTailorSimple.WPF.Models;

namespace TripTailorSimple.WPF.ViewModels;

public class DestinationCardViewModel
{
    public DestinationCardViewModel(TripProposal proposal)
    {
        Proposal = proposal;
    }

    public TripProposal Proposal { get; }
    public string CityCountry => $"{Proposal.City}, {Proposal.Country}";
    public string Summary => Proposal.Description;
    public string Temperature => $"{Proposal.AverageTemperature:0.#}°C";
    public string Price => $"{Proposal.TotalPrice} €";
    public string Breakdown => $"Vol {Proposal.EstimatedFlightPrice}€ · Hôtel {Proposal.EstimatedHotelPrice}€ · Activités {Proposal.EstimatedActivityPrice}€";
    public string TravelBadge => Proposal.TotalPrice < 800 ? "Économie" : Proposal.TotalPrice > 1500 ? "Luxe" : "Confort";
}
