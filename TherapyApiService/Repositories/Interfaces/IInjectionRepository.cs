using TherapyApiService.Models;

namespace TherapyApiService.Repositories.Interfaces
{
    public interface IInjectionRepository
    {
        Task AddAsync(Injection injection);
        Task<Injection> FindAsync(EntityId id);
        Task<Injection[]> FindPlannedInjections();
    }
}