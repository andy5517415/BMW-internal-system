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






        //自己寫的
        // PUT: api/BusinessOrderDetails/25/3
        [HttpPut("{ordid}/{oplid}")]
        public async Task<ActionResult<dynamic>> PutOrderDetail(int ordid, int oplid)
        {


            var data = _context.BusinessOrderDetails
                .Where(od => od.OrderId == ordid);

            foreach (var item in data)
            {
                item.OptionalId = oplid;
                _context.BusinessOrderDetails.Update(item);
            }
            await _context.SaveChangesAsync();


            //if (id != businessOrderDetail.OdId)
            //{
            //    return BadRequest();
            //}

            //_context.Entry(businessOrderDetail).State = EntityState.Modified;

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!BusinessOrderDetailExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            return data.ToList();
        }





        //Delete訂單(父子資料同時刪除)
        // Delete: api/BusinessOrderDetails/order/X021674138417
        [HttpDelete("order/{ordernum}")]
        public void DeleteOrder(string ordernum)
        {

            var detail = from bod in _context.BusinessOrderDetails
                          join bo in _context.BusinessOrders on bod.OrderId equals bo.OrderId
                          where bo.OrderNumber == ordernum
                          select bod;

            _context.BusinessOrderDetails.RemoveRange(detail.ToList());
            _context.SaveChanges();


            var ord = (from bo in _context.BusinessOrders
                          where bo.OrderNumber == ordernum
                          select bo).SingleOrDefault();
            if (ord != null)
            {
                _context.BusinessOrders.Remove(ord);
                _context.SaveChanges();
            }
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
