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
    public class BusinessOrdersController : ControllerBase
    {
        private readonly MSIT44Context _context;

        public BusinessOrdersController(MSIT44Context context)
        {
            _context = context;
        }






        // GET: api/BusinessOrders/getorder/i0320230105003
        [HttpGet("getorder/{ordernum}")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetOrder(string ordernum)
        {
            var q = from ord in _context.BusinessOrders
                    join od in _context.BusinessOrderDetails on ord.OrderId equals od.OrderId
                    join opl in _context.BusinessOptionals on od.OptionalId equals opl.OptionalId
                    join a in _context.BusinessAreas on ord.AreaId equals a.AreaId
                    where ord.OrderNumber == ordernum
                    select new
                    {
                        OrderId = ord.OrderId,
                        OrderNumber = ord.OrderNumber,
                        OptionalId = od.OptionalId,
                        CategoryId = opl.CategoryId,
                        Price = opl.Price,
                        OptionalName = opl.OptionalName,
                        AreaId = ord.AreaId
                    };
            return await q.ToListAsync();
        }




        // GET: api/BusinessOrders/getagent/1
        [HttpGet("getagent/{id}")]
        public async Task<ActionResult<IEnumerable<dynamic>>> getagent(int id)
        {
            var q = from a in _context.BusinessAreas 
                    where a.AreaId == id
                    select a;
            return await q.ToListAsync();
        }










        // GET: api/BusinessOrders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BusinessOrder>>> GetBusinessOrders()
        {
            return await _context.BusinessOrders.ToListAsync();
        }

        // GET: api/BusinessOrders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BusinessOrder>> GetBusinessOrder(int id)
        {
            var businessOrder = await _context.BusinessOrders.FindAsync(id);

            if (businessOrder == null)
            {
                return NotFound();
            }

            return businessOrder;
        }


        //自己改的
        // PUT: api/BusinessOrders/putorder/35
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("putorder/{id}")]
        public async Task<IActionResult> PutOrder(int id, BusinessOrder businessOrder)
        {
            if (id != businessOrder.OrderId)
            {
                return BadRequest();
            }

            _context.Entry(businessOrder).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BusinessOrderExists(id))
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






        //原廠
        // PUT: api/BusinessOrders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBusinessOrder(int id, BusinessOrder businessOrder)
        {
            if (id != businessOrder.OrderId)
            {
                return BadRequest();
            }

            _context.Entry(businessOrder).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BusinessOrderExists(id))
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












        // POST: api/BusinessOrders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BusinessOrder>> PostBusinessOrder(BusinessOrder businessOrder)
        {
            _context.BusinessOrders.Add(businessOrder);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBusinessOrder", new { id = businessOrder.OrderId }, businessOrder);
        }










        // DELETE: api/BusinessOrders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBusinessOrder(int id)
        {
            var businessOrder = await _context.BusinessOrders.FindAsync(id);
            if (businessOrder == null)
            {
                return NotFound();
            }

            _context.BusinessOrders.Remove(businessOrder);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BusinessOrderExists(int id)
        {
            return _context.BusinessOrders.Any(e => e.OrderId == id);
        }
    }
}
