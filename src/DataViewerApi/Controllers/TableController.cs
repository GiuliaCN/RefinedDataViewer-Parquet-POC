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
    public class TableController(IDeltaRepository deltaRepository, ICatalogRepository catalogRepository, ITableService tableService) : ControllerBase
    {
        private readonly IDeltaRepository _deltaRepository = deltaRepository;
        private readonly ICatalogRepository _catalogRepository = catalogRepository;
        private readonly ITableService _tableService = tableService;

        [HttpGet]
        [EndpointSummary("List top 50 values available from a column of Catalog. Options: 'Category', 'SKU'")]
        [Route("/value-filters/{filter}")]
        public async Task<ActionResult> Get(string filter)
        {
            var fullCatalog = await _catalogRepository.GetAllAsync();
            if (filter == "Category")
            {
                var categories = fullCatalog.GroupBy(x => x.Category).Select(x => x.Key);
                return Ok(categories.Take(50));
            }
            else if (filter == "SKU") return Ok(fullCatalog.Take(50).Select(x => x.SKU));
            else return BadRequest("Invalid Filter");
        }

        [HttpGet]
        [EndpointSummary("List table. Filter Options: 'Category', 'SKU'")]
        public async Task<ActionResult> Get([FromQuery] int? category, [FromQuery] int? sku)
        {
            string filter = "";
            int value = 0;
            if (sku.HasValue)
            {
                filter = "SKU";
                value = (int)sku;
            }
            else if (category.HasValue)
            {
                filter = "Category"; 
                value = (int)category;
            }

            var list = await _tableService.GetTableViewAsync(filter, value);

            return Ok(list);
        }

        [HttpPost]
        [EndpointSummary("Add Delta - Make change to table item. Filter Options: 'Category', 'SKU'")]
        public async Task<ActionResult> Post(Delta delta)
        {
            await _deltaRepository.AddAsync(delta);
            return Ok();
        }
    }
}