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
    public class ProcessController(IProcessRepository processRepository) : ControllerBase
    {
        private readonly IProcessRepository _repository = processRepository; 
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Process>>> Get()
        {
            var processList = await _repository.GetAllAsync();
            return Ok(processList);
        }
        
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Process>>> Post()
        {
            Process process = new();
            await _repository.AddAsync(process);
            return Ok();
        }
    }
}