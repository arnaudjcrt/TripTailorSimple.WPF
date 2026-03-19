using TripTailorSimple.WPF.Models;

namespace TripTailorSimple.WPF.Services.Interfaces;

public interface ITripSearchService
{
    Task<List<TripProposal>> SearchAsync(SearchCriteria criteria);
}
