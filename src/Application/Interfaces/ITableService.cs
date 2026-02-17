using Application.DTOs;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface ITableService
    {
        Task<IEnumerable<TableViewLine>> GetTableViewAsync(string filter, int filterValue);
        void ApplyDeltasInList(List<TableItemChange> listAllTableItemChange, IEnumerable<Delta> allDeltas);
    }
}