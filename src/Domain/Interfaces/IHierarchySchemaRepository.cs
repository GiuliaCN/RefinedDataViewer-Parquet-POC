using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IHierarchySchemaRepository
    {        
        Task<IEnumerable<HierarchySchema>> GetAllAsync();
    }
}