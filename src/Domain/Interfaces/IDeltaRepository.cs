using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IDeltaRepository
    {        
        Task AddAsync (Delta process);
        Task<IEnumerable<Delta>> GetAllAsync ();
    }
}