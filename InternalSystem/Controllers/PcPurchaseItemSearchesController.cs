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
    public class PcPurchaseItemSearchesController : ControllerBase
    {
        private readonly MSIT44Context _context;

        public PcPurchaseItemSearchesController(MSIT44Context context)
        {
            _context = context;
        }

        // GET: api/PcPurchaseItemSearches/goods
        [HttpGet("goods")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetPcPurchaseItemSearches()
        {
            var i = from PS in this._context.PcPurchaseItemSearches
                    select new
                    {
                        ProductId = PS.ProductId,
                        Goods = PS.Goods,
                        Unit = PS.Unit,
                        UnitPrice = PS.UnitPrice,
                        Image = PS.Image
                    };

            return await i.ToListAsync();
        }

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<PcPurchaseItemSearch>>> GetPcPurchaseItemSearches()
        //{
        //    return await _context.PcPurchaseItemSearches.ToListAsync();
        //}

        //// GET: api/PcPurchaseItemSearches/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<PcPurchaseItemSearch>> GetPcPurchaseItemSearch(int id)
        //{
        //    var pcPurchaseItemSearch = await _context.PcPurchaseItemSearches.FindAsync(id);

        //    if (pcPurchaseItemSearch == null)
        //    {
        //        return NotFound();
        //    }

        //    return pcPurchaseItemSearch;
        //}

        // PUT: api/PcPurchaseItemSearches/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPcPurchaseItemSearch(int id, PcPurchaseItemSearch pcPurchaseItemSearch)
        {
            if (id != pcPurchaseItemSearch.ProductId)
            {
                return BadRequest();
            }

            _context.Entry(pcPurchaseItemSearch).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PcPurchaseItemSearchExists(id))
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

        // POST: api/PcPurchaseItemSearches
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PcPurchaseItemSearch>> PostPcPurchaseItemSearch(PcPurchaseItemSearch pcPurchaseItemSearch)
        {
            _context.PcPurchaseItemSearches.Add(pcPurchaseItemSearch);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPcPurchaseItemSearch", new { id = pcPurchaseItemSearch.ProductId }, pcPurchaseItemSearch);
        }

        // DELETE: api/PcPurchaseItemSearches/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePcPurchaseItemSearch(int id)
        {
            var pcPurchaseItemSearch = await _context.PcPurchaseItemSearches.FindAsync(id);
            if (pcPurchaseItemSearch == null)
            {
                return NotFound();
            }

            _context.PcPurchaseItemSearches.Remove(pcPurchaseItemSearch);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PcPurchaseItemSearchExists(int id)
        {
            return _context.PcPurchaseItemSearches.Any(e => e.ProductId == id);
        }
    }
}
