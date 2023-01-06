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
    public class ProductionProcessListsController : ControllerBase
    {
        private readonly MSIT44Context _context;

        public ProductionProcessListsController(MSIT44Context context)
        {
            _context = context;
        }
        // GET: api/ProductionProcessLists
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetProductionProcessLists()
        {
            var List = from PPL in this._context.ProductionProcessLists
                       join PA in this._context.ProductionAreas on PPL.AreaId equals PA.AreaId
                       join POPS in this._context.ProductionOrderProcessStatuses on  PPL.OrderId equals POPS.OrderId
                       join PP in this._context.ProductionProcesses on POPS.ProcessId equals PP.ProcessId
                       join 
                       select new
                       {
                           OrderId = PPL.OrderId,
                           AreaId = PPL.AreaId,
                           AreaName = PA.AreaName,
                           ProcessId = POPS.ProcessId,
                           ProcessName = PP.ProcessName
                           
                       };

            return await List.ToListAsync();
        }

        // GET: api/ProductionProcessLists
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<ProductionProcessList>>> GetProductionProcessLists()
        //{
        //    return await _context.ProductionProcessLists.ToListAsync();
        //}

        // GET: api/ProductionProcessLists/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductionProcessList>> GetProductionProcessList(int id)
        {
            var productionProcessList = await _context.ProductionProcessLists.FindAsync(id);

            if (productionProcessList == null)
            {
                return NotFound();
            }

            return productionProcessList;
        }

        // PUT: api/ProductionProcessLists/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductionProcessList(int id, ProductionProcessList productionProcessList)
        {
            if (id != productionProcessList.OrderId)
            {
                return BadRequest();
            }

            _context.Entry(productionProcessList).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductionProcessListExists(id))
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

        // POST: api/ProductionProcessLists
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProductionProcessList>> PostProductionProcessList(ProductionProcessList productionProcessList)
        {
            _context.ProductionProcessLists.Add(productionProcessList);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ProductionProcessListExists(productionProcessList.OrderId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetProductionProcessList", new { id = productionProcessList.OrderId }, productionProcessList);
        }

        // DELETE: api/ProductionProcessLists/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductionProcessList(int id)
        {
            var productionProcessList = await _context.ProductionProcessLists.FindAsync(id);
            if (productionProcessList == null)
            {
                return NotFound();
            }

            _context.ProductionProcessLists.Remove(productionProcessList);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductionProcessListExists(int id)
        {
            return _context.ProductionProcessLists.Any(e => e.OrderId == id);
        }
    }
}
