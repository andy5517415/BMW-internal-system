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
    public class BusinessOrderDetailsController : ControllerBase
    {
        private readonly MSIT44Context _context;

        public BusinessOrderDetailsController(MSIT44Context context)
        {
            _context = context;
        }





        //自己寫的
        // GET: api/BusinessOrderDetails/25/1
        [HttpGet("{ordid}/{oplid}")]
        public async Task<ActionResult<dynamic>> GetOdId(int ordid, int oplid)
        {
            var q = from od in _context.BusinessOrderDetails
                    where od.OrderId == ordid && od.OptionalId == oplid
                    select od.OdId;
            return await q.SingleOrDefaultAsync();
        }


        //自己寫的
        // POST: api/BusinessOrderDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public string PostOrderDetail(int OrderId , [FromBody] BusinessOrderDetail bod)
        {
            if (!_context.BusinessOrderDetails.Any(a => a.OrderId == OrderId))
            {
                return "沒有該筆資料";
            }

            BusinessOrderDetail insert = new BusinessOrderDetail
            {
                OrderId = bod.OrderId,
                OptionalId = bod.OptionalId
            };

            _context.BusinessOrderDetails.Add(insert);
            _context.SaveChanges();

            return "Okay";
        }
















        // GET: api/BusinessOrderDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BusinessOrderDetail>>> GetBusinessOrderDetails()
        {
            return await _context.BusinessOrderDetails.ToListAsync();
        }

        // GET: api/BusinessOrderDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BusinessOrderDetail>> GetBusinessOrderDetail(int id)
        {
            var businessOrderDetail = await _context.BusinessOrderDetails.FindAsync(id);

            if (businessOrderDetail == null)
            {
                return NotFound();
            }

            return businessOrderDetail;
        }

        // PUT: api/BusinessOrderDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBusinessOrderDetail(int id, BusinessOrderDetail businessOrderDetail)
        {
            if (id != businessOrderDetail.OdId)
            {
                return BadRequest();
            }

            _context.Entry(businessOrderDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BusinessOrderDetailExists(id))
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

        // POST: api/BusinessOrderDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BusinessOrderDetail>> PostBusinessOrderDetail(BusinessOrderDetail businessOrderDetail)
        {
            _context.BusinessOrderDetails.Add(businessOrderDetail);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBusinessOrderDetail", new { id = businessOrderDetail.OdId }, businessOrderDetail);
        }

        // DELETE: api/BusinessOrderDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBusinessOrderDetail(int id)
        {
            var businessOrderDetail = await _context.BusinessOrderDetails.FindAsync(id);
            if (businessOrderDetail == null)
            {
                return NotFound();
            }

            _context.BusinessOrderDetails.Remove(businessOrderDetail);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BusinessOrderDetailExists(int id)
        {
            return _context.BusinessOrderDetails.Any(e => e.OdId == id);
        }
    }
}
