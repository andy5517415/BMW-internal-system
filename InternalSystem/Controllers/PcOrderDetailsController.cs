using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InternalSystem.Models;
using Microsoft.CodeAnalysis;

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

        // GET: api/PcOrderDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PcOrderDetail>>> GetPcOrderDetails()
        {
            return await _context.PcOrderDetails.ToListAsync();
        }

        // GET: api/PcOrderDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PcOrderDetail>> GetPcOrderDetail(int id)
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
        public async Task<IActionResult> PutPcOrderDetail(int id, PcOrderDetail pcOrderDetail)
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

        //// POST: api/PcOrderDetails
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<PcOrderDetail>> PostPcOrderDetail(PcOrderDetail pcOrderDetail)
        //{
        //    _context.PcOrderDetails.Add(pcOrderDetail);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetPcOrderDetail", new { id = pcOrderDetail.OrderId }, pcOrderDetail);
        //}

        // POST: api/PCOrderDetail
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult PostPcApplicationGoods(int OrderId, [FromBody] PcOrderDetail pcOrderDetail)
        {
            if (!_context.PcOrderDetails.Any(a => a.OrderId == OrderId))
            {
                return Content("子資料沒資料");
            }

            //if (!double.IsNaN(Convert.ToDouble(pcOrderDetail.Quantiy)) && !double.IsNaN(Convert.ToDouble(pcOrderDetail.Subtotal)))
            //{
                PcOrderDetail insert = new PcOrderDetail
            {
                OrderId = OrderId,
                ProductId = pcOrderDetail.ProductId,
                Goods = pcOrderDetail.Goods,
                Quantiy = Convert.ToInt32(pcOrderDetail.Quantiy),
                Unit = pcOrderDetail.Unit,
                UnitPrice = pcOrderDetail.UnitPrice,
                Subtotal = pcOrderDetail.Subtotal
            };

            _context.PcOrderDetails.Add(insert);
            _context.SaveChanges();
        //}
            return Content("子資料新建Ok");
        }

        // DELETE: api/PcOrderDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePcOrderDetail(int id)
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

        private bool PcOrderDetailExists(int id)
        {
            return _context.PcOrderDetails.Any(e => e.OrderId == id);
        }
    }
}
