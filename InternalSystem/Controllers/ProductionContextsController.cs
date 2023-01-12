using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InternalSystem.Models;
using System.Timers;

namespace InternalSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductionContextsController : ControllerBase
    {
        private readonly MSIT44Context _context;

        public ProductionContextsController(MSIT44Context context)
        {
            _context = context;
        }

        //報工內容
        //GET: api/ProductionContexts/Contexts
        [HttpGet("Contexts")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetContexts()
        {
            var query = from PC in this._context.ProductionContexts
                        join PPD in this._context.PersonnelProfileDetails on PC.EmployeeId equals PPD.EmployeeId
                        join PPL in this._context.ProductionProcessLists on PC.OrderId equals PPL.OrderId
                      
                        select new
                        {
                            OrderId = PC.OrderId,
                            EmployeeId  = PC.EmployeeId,
                            EmployeeName = PPD.EmployeeName,
                            Date = PC.Date.ToString(),
                            //StartTime = PC.StartTime.ToString(),
                            //EndTime = PC.EndTime.ToString(),
                            Context = PC.Context
                        };
                       


            return await query.ToListAsync();
        }

        // GET: api/ProductionContexts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductionContext>>> GetProductionContexts()
        {
            return await _context.ProductionContexts.ToListAsync();
        }

        // GET: api/ProductionContexts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductionContext>> GetProductionContext(int id)
        {
            var productionContext = await _context.ProductionContexts.FindAsync(id);

            if (productionContext == null)
            {
                return NotFound();
            }

            return productionContext;
        }

        // PUT: api/ProductionContexts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductionContext(int id, ProductionContext productionContext)
        {
            if (id != productionContext.OrderId)
            {
                return BadRequest();
            }

            _context.Entry(productionContext).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductionContextExists(id))
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

        // POST: api/ProductionContexts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProductionContext>> PostProductionContext(ProductionContext productionContext)
        {
            _context.ProductionContexts.Add(productionContext);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ProductionContextExists(productionContext.OrderId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetProductionContext", new { id = productionContext.OrderId }, productionContext);
        }

        // DELETE: api/ProductionContexts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductionContext(int id)
        {
            var productionContext = await _context.ProductionContexts.FindAsync(id);
            if (productionContext == null)
            {
                return NotFound();
            }

            _context.ProductionContexts.Remove(productionContext);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductionContextExists(int id)
        {
            return _context.ProductionContexts.Any(e => e.OrderId == id);
        }
    }
}
