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
    public class ProductionBugContextsController : ControllerBase
    {
        private readonly MSIT44Context _context;

        public ProductionBugContextsController(MSIT44Context context)
        {
            _context = context;
        }

        // GET: api/ProductionBugContexts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductionBugContext>>> GetProductionBugContexts()
        {
            return await _context.ProductionBugContexts.ToListAsync();
        }

        // GET: api/ProductionBugContexts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductionBugContext>> GetProductionBugContext(int id)
        {
            var productionBugContext = await _context.ProductionBugContexts.FindAsync(id);

            if (productionBugContext == null)
            {
                return NotFound();
            }

            return productionBugContext;
        }

        // PUT: api/ProductionBugContexts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductionBugContext(int id, ProductionBugContext productionBugContext)
        {
            if (id != productionBugContext.OrderId)
            {
                return BadRequest();
            }

            _context.Entry(productionBugContext).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductionBugContextExists(id))
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

        // POST: api/ProductionBugContexts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProductionBugContext>> PostProductionBugContext(ProductionBugContext productionBugContext)
        {
            _context.ProductionBugContexts.Add(productionBugContext);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ProductionBugContextExists(productionBugContext.OrderId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetProductionBugContext", new { id = productionBugContext.OrderId }, productionBugContext);
        }

        // DELETE: api/ProductionBugContexts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductionBugContext(int id)
        {
            var productionBugContext = await _context.ProductionBugContexts.FindAsync(id);
            if (productionBugContext == null)
            {
                return NotFound();
            }

            _context.ProductionBugContexts.Remove(productionBugContext);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductionBugContextExists(int id)
        {
            return _context.ProductionBugContexts.Any(e => e.OrderId == id);
        }
    }
}
