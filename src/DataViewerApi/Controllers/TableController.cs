using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DataViewerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TableController(IDeltaRepository deltaRepository, ICatalogRepository catalogRepository) : ControllerBase
    {        
        private readonly IDeltaRepository _deltaRepository = deltaRepository; 
        private readonly ICatalogRepository _catalogRepository = catalogRepository; 
        
        [HttpGet]
        [EndpointSummary("List top 50 values available from a column of Catalog. Options: 'Category', 'SKU'")]
        [Route("/value-filters/{filter}")]
        public async Task<ActionResult> Get(string filter)
        {
            var fullCatalog = await _catalogRepository.GetAllAsync();
            if(filter == "Category")
            {
                var categories = fullCatalog.GroupBy(x => x.Category).Select(x => x.Key);
                return Ok(categories.Take(50));
            }
            else if(filter == "SKU") return Ok(fullCatalog.Take(50).Select(x => x.SKU));
            else return BadRequest("Invalid Filter");
        }
        
        // [HttpPost]
        // public async Task<ActionResult<IEnumerable<Process>>> Post()
        // {
        //     Process process = new();
        //     await _repository.AddAsync(process);
        //     return Ok();
        // }
    }
}