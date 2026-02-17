using System;
using System.Collections.Generic;
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
        //[Route("/value-filters/{filter}")]
        //public async Task<ActionResult<IEnumerable<int>>> Get(string filter)
        public async Task<ActionResult<IEnumerable<Catalog>>> Get()
        {
            var processList = await _catalogRepository.GetAllAsync();
            return Ok(processList.Take(10));
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