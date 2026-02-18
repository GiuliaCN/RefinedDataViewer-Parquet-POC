using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DataViewerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TableController(IDeltaRepository deltaRepository, IHierarchySchemaRepository hierarchySchemaRepository, ITableService tableService) : ControllerBase
    {
        private readonly IDeltaRepository _deltaRepository = deltaRepository;
        private readonly IHierarchySchemaRepository _hierarchySchemaRepository = hierarchySchemaRepository;
        private readonly ITableService _tableService = tableService;

        [HttpGet]
        [EndpointSummary("List top 50 values available from a column of HierarchySchema. Options: 'IntermediateNode', 'AtomicEntity'")]
        [Route("/value-filters/{filter}")]
        public async Task<ActionResult> Get(string filter)
        {
            var fullHierarchySchema = await _hierarchySchemaRepository.GetAllAsync();
            if (filter == "IntermediateNode")
            {
                var hierarchies = fullHierarchySchema.GroupBy(x => x.IntermediateNode).Select(x => x.Key);
                return Ok(hierarchies.Take(50));
            }
            else if (filter == "AtomicEntity") return Ok(fullHierarchySchema.Take(50).Select(x => x.AtomicEntity));
            else return BadRequest("Invalid Filter");
        }

        [HttpGet]
        [EndpointSummary("List table. Filter Options: 'IntermediateNode', 'AtomicEntity'")]
        public async Task<ActionResult> Get([FromQuery] int? intermediate, [FromQuery] int? atomic)
        {
            string filter = "";
            int value = 0;
            if (atomic.HasValue)
            {
                filter = "AtomicEntity";
                value = (int)atomic;
            }
            else if (intermediate.HasValue)
            {
                filter = "IntermediateNode"; 
                value = (int)intermediate;
            }

            var list = await _tableService.GetTableViewAsync(filter, value);

            return Ok(list);
        }

        [HttpPost]
        [EndpointSummary("Add Delta - Make change to table item. Filter Options: 'IntermediateNode', 'AtomicEntity'")]
        public async Task<ActionResult> Post(Delta delta)
        {
            await _deltaRepository.AddAsync(delta);
            return Ok();
        }
    }
}