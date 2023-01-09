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
    public class PcOrderDetailsController : ControllerBase
    {
        private readonly MSIT44Context _context;

        public PcOrderDetailsController(MSIT44Context context)
        {
            _context = context;
        }

        //寫錯的 暫時不使用
        // GET: api/PcOrderDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetPcOrderDetails()
        {
            var i = from OD in this._context.PcOrderDetails
                    join PS in this._context.PcPurchaseItemSearches on OD.ProductId equals PS.ProductId
                    select new
                    {
                        OrderId = OD.OrderId,
                        ProductId = PS.ProductId,
                        Goods = OD.Goods,
                        Quantiy = OD.Quantiy,
                        Unit = OD.Unit,
                        UnitPrice = OD.UnitPrice,
                        Subtotal = OD.Subtotal
                    };

            return await i.ToListAsync();
        }
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<PcOrderDetail>>> GetPcOrderDetails()
        //{
        //    return await _context.PcOrderDetails.ToListAsync();
        //}

        // GET: api/PcOrderDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PcOrderDetail>> GetPcOrderDetail(string id)
        {
            var pcOrderDetail = await _context.PcOrderDetails.FindAsync(id);

            if (pcOrderDetail == null)
            {
                return NotFound();
            }

            return pcOrderDetail;
        }

        // PUT: api/PcOrderDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPcOrderDetail(string id, PcOrderDetail pcOrderDetail)
        {
            if (id != pcOrderDetail.OrderId)
            {
                return BadRequest();
            }

            _context.Entry(pcOrderDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PcOrderDetailExists(id))
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

        // POST: api/PcOrderDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PcOrderDetail>> PostPcOrderDetail(PcOrderDetail pcOrderDetail)
        {
            _context.PcOrderDetails.Add(pcOrderDetail);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PcOrderDetailExists(pcOrderDetail.OrderId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPcOrderDetail", new { id = pcOrderDetail.OrderId }, pcOrderDetail);
        }

        // DELETE: api/PcOrderDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePcOrderDetail(string id)
        {
            var pcOrderDetail = await _context.PcOrderDetails.FindAsync(id);
            if (pcOrderDetail == null)
            {
                return NotFound();
            }

            _context.PcOrderDetails.Remove(pcOrderDetail);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PcOrderDetailExists(string id)
        {
            return _context.PcOrderDetails.Any(e => e.OrderId == id);
        }
    }
}
