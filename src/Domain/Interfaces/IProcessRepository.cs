using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IProcessRepository
    {
        Task AddAsync (Process process);
        Task StartAsync (Process process);
        Task EndAsync (Process process);
        Task<IEnumerable<Process>> GetAllAsync ();
    }
}