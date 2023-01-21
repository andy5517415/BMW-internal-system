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



        //自己寫的
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


        //自己寫的
        // GET: api/BusinessOrders/getorder/i0320230105003/1
        [HttpGet("getorder/{ordernum}/{category}")]
        public async Task<ActionResult<dynamic>> GetOrderdetail(string ordernum, int category)
        {
            var q = from ord in _context.BusinessOrders
                    join od in _context.BusinessOrderDetails on ord.OrderId equals od.OrderId
                    join opl in _context.BusinessOptionals on od.OptionalId equals opl.OptionalId
                    join a in _context.BusinessAreas on ord.AreaId equals a.AreaId
                    where ord.OrderNumber == ordernum && opl.CategoryId == category
                    select new
                    {
                        OptionalId = od.OptionalId,
                        CategoryId = opl.CategoryId,
                        Price = opl.Price,
                        OptionalName = opl.OptionalName,

                        OrderId = ord.OrderId,
                        OrderNumber = ord.OrderNumber,
                        AreaId = ord.AreaId,
                        OrderDateTime = ord.OrderDateTime
                    };
            return await q.SingleOrDefaultAsync();
        }








        //自己寫的
        // GET: api/BusinessOrders/getagent/1
        [HttpGet("getagent/{id}")]
        public async Task<ActionResult<dynamic>> getagent(int id)
        {
            var q = from a in _context.BusinessAreas
                    where a.AreaId == id
                    select a;
            return await q.SingleOrDefaultAsync();
        }







        //自己寫的
        // GET: api/BusinessOrders/GetOrderAll
        [HttpGet("GetOrderAll")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetOrderAll()
        {
            var q = from ord in _context.BusinessOrders
                    join od in _context.BusinessOrderDetails on ord.OrderId equals od.OrderId
                    join opl in _context.BusinessOptionals on od.OptionalId equals opl.OptionalId
                    join a in _context.BusinessAreas on ord.AreaId equals a.AreaId
                    join ppl in _context.ProductionProcessLists on ord.OrderId equals ppl.OrderId
                    join pps in _context.ProductionProcessStatusNames on ppl.StatusId equals pps.StatusId
                    join pp in _context.ProductionProcesses on ppl.ProcessId equals pp.ProcessId
                    where opl.CategoryId == 1
                    select new
                    {
                        OrderNumber = ord.OrderNumber,
                        OptionalName = opl.OptionalName,
                        IsAccepted = ord.IsAccepted,
                        Area = a.AreaName,
                        process = pp.ProcessName,
                        processstate = pps.StatusName,
                        orderdatetime = ord.OrderDateTime,



                        OrderId = ord.OrderId,
                        OptionalId = od.OptionalId,
                        CategoryId = opl.CategoryId,
                        Price = opl.Price,
                    };
            return await q.ToListAsync();
        }




        //測試沒迴圈新增
        // POST: api/BusinessOrders/withoutloop
        [HttpPost("withoutloop")]
        public void PostOrder([FromBody] BusinessOrder bo)
        {
            BusinessOrder insert = new BusinessOrder
            {
                OrderNumber = bo.OrderNumber,
                OrderDateTime = DateTime.Now,
                AreaId = bo.AreaId,
                Price = bo.Price,
                EmployeeId = bo.EmployeeId,
                IsAccepted = bo.IsAccepted,
                BusinessOrderDetails = bo.BusinessOrderDetails
            };

            _context.BusinessOrders.Add(insert);
            _context.SaveChanges();
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
