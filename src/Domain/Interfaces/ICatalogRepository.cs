using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ICatalogRepository
    {        
        Task<IEnumerable<Catalog>> GetAllAsync();
    }
}