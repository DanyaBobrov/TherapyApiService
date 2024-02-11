using TherapyApiService.Models;
using TherapyApiService.Repositories.Interfaces;

namespace TherapyApiService.Repositories.Implementations
{
    public class InjectionRepository : IInjectionRepository
    {
        public InjectionRepository()
        {

        }

        public async Task AddAsync(Injection injection)
        {
            throw new NotImplementedException();
        }

        public async Task<Injection> FindAsync(EntityId id)
        {
            throw new NotImplementedException();
        }
    }
}