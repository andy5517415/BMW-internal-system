using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InternalSystem.Models;

namespace InternalSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductionProcessesController : ControllerBase
    {
        private readonly MSIT44Context _context;

        public ProductionProcessesController(MSIT44Context context)
        {
            _context = context;
        }

        // GET: api/ProductionProcesses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductionProcess>>> GetProductionProcesses()
        {
            return await _context.ProductionProcesses.ToListAsync();
        }

        // GET: api/ProductionProcesses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductionProcess>> GetProductionProcess(int id)
        {
            var productionProcess = await _context.ProductionProcesses.FindAsync(id);

            if (productionProcess == null)
            {
                return NotFound();
            }

            return productionProcess;
        }

        // PUT: api/ProductionProcesses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductionProcess(int id, ProductionProcess productionProcess)
        {
            if (id != productionProcess.ProcessId)
            {
                return BadRequest();
            }

            _context.Entry(productionProcess).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductionProcessExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ProductionProcesses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProductionProcess>> PostProductionProcess(ProductionProcess productionProcess)
        {
            _context.ProductionProcesses.Add(productionProcess);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ProductionProcessExists(productionProcess.ProcessId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetProductionProcess", new { id = productionProcess.ProcessId }, productionProcess);
        }

        // DELETE: api/ProductionProcesses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductionProcess(int id)
        {
            var productionProcess = await _context.ProductionProcesses.FindAsync(id);
            if (productionProcess == null)
            {
                return NotFound();
            }

            _context.ProductionProcesses.Remove(productionProcess);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductionProcessExists(int id)
        {
            return _context.ProductionProcesses.Any(e => e.ProcessId == id);
        }
    }
}
