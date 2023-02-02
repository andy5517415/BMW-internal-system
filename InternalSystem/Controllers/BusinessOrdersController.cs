using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InternalSystem.Models;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using System.Security.Cryptography.X509Certificates;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
                        AreaId = ord.AreaId,
                        isAccepted = ord.IsAccepted
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
                    //join ppl in _context.ProductionProcessLists on ord.OrderId equals ppl.OrderId
                    //join pps in _context.ProductionProcessStatusNames on ppl.StatusId equals pps.StatusId
                    //join pp in _context.ProductionProcesses on ppl.ProcessId equals pp.ProcessId
                    where opl.CategoryId == 1 /*&& ord.IsAccepted==true*/
                    select new
                    {
                        OrderNumber = ord.OrderNumber,
                        OptionalName = opl.OptionalName,
                        IsAccepted = ord.IsAccepted,
                        Area = a.AreaName,
                        //process = pp.ProcessName,
                        //processstate = pps.StatusName,
                        orderdatetime = ord.OrderDateTime,
                        editdatetime = ord.EditDatetime,



                        OrderId = ord.OrderId,
                        OptionalId = od.OptionalId,
                        CategoryId = opl.CategoryId,
                        Price = opl.Price,
                    };
            return await q.ToListAsync();
        }




        //新增父子資料
        // POST: api/BusinessOrders/withoutloop
        [HttpPost("withoutloop")]
        public string PostOrder([FromBody] ICollection<BusinessOrderDetail> bod ,
            string ordnum ,
            //string orddate ,
            int    areaid,
            int    price,
            int    empid,
            bool   isacc
            )

        {

            /*foreach (var item in bod)
            {
                if (!Regex.IsMatch(item.OptionalId.ToString(), @"^[0-9]$"))
                {
                    return "有遺漏的選配，請重新選擇!";
                }
            }

            if (areaid.ToString()==null)
            {
                return "代理商區域未選擇，請填選!";
            }
            else*/ if(bod!=null /*&& bo.BusinessOrderDetails.Count==9 && bo.AreaId>0*/)
            {
                BusinessOrder insert = new BusinessOrder
                {
                    OrderNumber          = ordnum,
                    OrderDateTime        = DateTime.Now,
                    AreaId               = areaid,
                    Price                = price,
                    EmployeeId           = empid,
                    IsAccepted           = isacc,
                    BusinessOrderDetails = bod
                };

                _context.BusinessOrders.Add(insert);
                _context.SaveChanges();
                return "訂單新增成功!Order";
            }
            else
            {
                return "last";
            };

            //return "last";

        }








        ////修改父子資料(目前未成功)
        //// PUT: api/BusinessOrders/withoutloop?ordnum=M011672502400&areaid=1&price=5741000
        //[HttpPut("withoutloop")]
        //public string PutOrder(string ordnum,int areaid, int price, ICollection<BusinessOrderDetail> bodput)
        //{
        //    var q = _context.BusinessOrders.Select(x => x).Where(x=>x.OrderNumber== ordnum).SingleOrDefault();
        //    var ed = _context.BusinessOrderDetails.Select(x => x).Where(x=>x.OrderId==q.OrderId);

        //    BusinessOrder update = new BusinessOrder
        //    {
        //        OrderId = q.OrderId,
        //        OrderNumber = q.OrderNumber,
        //        OrderDateTime = q.OrderDateTime,
        //        EditDatetime = DateTime.Now,
        //        AreaId = areaid,
        //        Price = price,
        //        EmployeeId = 5,
        //        IsAccepted = false,
        //        BusinessOrderDetails = bodput
        //    };

        //    //foreach (var item in bodput)
        //    //{

        //    //}



        //    _context.BusinessOrders.UpdateRange(update);
        //    _context.SaveChanges();
        //    return "Order大表修改成功";
        //}

        //修改父子資料(目前未成功)
        // PUT: api/BusinessOrders/withoutloop?ordnum=M011672502400&areaid=1&price=5741000
        [HttpPut("withoutloop")]
        public int PutOrder(string ordnum, int areaid, int price, ICollection<BusinessOrderDetail> bodput)
        {

            var q = _context.BusinessOrders.Select(x => new
            {
                OrderId = x.OrderId,
                OrderNumber = x.OrderNumber,
                OrderDateTime = x.OrderDateTime,
                EditDatetime = DateTime.Now,
                AreaId = areaid,
                Price = price,
                EmployeeId = 5,
                IsAccepted = false,
                BusinessOrderDetails = bodput
            }).Where(x => x.OrderNumber == ordnum).SingleOrDefault();


            //var ed = _context.BusinessOrderDetails.Select(x => x).Where(x => x.OrderId == q.OrderId);

            BusinessOrder update = new BusinessOrder
            {
                OrderId = q.OrderId,
                OrderNumber = q.OrderNumber,
                OrderDateTime = q.OrderDateTime,
                EditDatetime = DateTime.Now,
                AreaId = areaid,
                Price = price,
                EmployeeId = 5,
                IsAccepted = false,
                BusinessOrderDetails = bodput
            };

            //foreach (var item in bodput)
            //{

            //}



            _context.BusinessOrders.UpdateRange(update);
            
            //return "Order大表修改成功";
            return _context.SaveChanges();
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
