using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class TableService(ITableRepository tableRepository, IDeltaRepository deltaRepository) : ITableService
    {
        private readonly ITableRepository _tableRepository = tableRepository;
        private readonly IDeltaRepository _deltaRepository = deltaRepository;
        public async Task<IEnumerable<TableViewLine>> GetTableViewAsync(string filter, int filterValue)
        {
            List<TableItemChange> listAllTableItemChange = (await _tableRepository.GetTableItemChangeAsync()).ToList();
            var allDeltas = await _deltaRepository.GetAllAsync();
            ApplyDeltasInList(listAllTableItemChange,allDeltas);
            return ApplyViewFilter(listAllTableItemChange, filter, filterValue);
        }
        public void ApplyDeltasInList(List<TableItemChange> listAllTableItemChange, IEnumerable<Delta> allDeltas)
        {
            foreach (Delta delta in allDeltas)
            {
                List<TableItemChange> modifiedSegment = new();
                if (FilterAccessors.TryGetValue(delta.Filter, out var acessor))
                {
                    modifiedSegment = listAllTableItemChange.Where(x => acessor(x) == delta.FilterValue && x.GroupKey == delta.GroupKey).ToList();        
                }
                else
                {
                    modifiedSegment = listAllTableItemChange.Where(x => x.GroupKey == delta.GroupKey).ToList();                    
                }
                var totalSum = modifiedSegment.Sum(x => x.ChangedSumValue);
                foreach(var item in modifiedSegment)
                {
                    double factor = item.ChangedSumValue / totalSum;
                    item.ChangedSumValue += factor * delta.Value;
                }
            }
        }
        private static IEnumerable<TableViewLine> ApplyViewFilter(IEnumerable<TableItemChange> list, string filter, int filterValue)
        {
            // If it has filter
            if (FilterAccessors.TryGetValue(filter, out var acessor))
            {
                return list
                    .Where(x => acessor(x) == filterValue)
                    .GroupBy(x => x.GroupKey)
                    .Select(g => new TableViewLine
                    {
                        GroupKey = g.Key,
                        ChangedSumValue = g.Sum(x => x.ChangedSumValue),
                        OriginalSumValue = g.Sum(x => x.OriginalSumValue)
                    });
            }

            return list
                .GroupBy(x => x.GroupKey)
                .Select(g => new TableViewLine
                {
                    GroupKey = g.Key,
                    ChangedSumValue = g.Sum(x => x.ChangedSumValue),
                    OriginalSumValue = g.Sum(x => x.OriginalSumValue)
                });
        }
        private static readonly Dictionary<string, Func<TableItemChange, int>> FilterAccessors = new()
        {
            { "SKU", x => x.SKU},
            { "Category", x => x.Category}
        };
    }
}