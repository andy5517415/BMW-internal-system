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
using Microsoft.CodeAnalysis;
using System.Security.Cryptography;
using InternalSystem.Dotos;

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



        //以訂單號碼找該筆訂單
        // GET: api/BusinessOrders/getorder/M011672502400
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


        //訂單查詢分流
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








        //找代理商區域ID
        // GET: api/BusinessOrders/getagent/1
        [HttpGet("getagent/{id}")]
        public async Task<ActionResult<dynamic>> getagent(int id)
        {
            var q = from a in _context.BusinessAreas
                    where a.AreaId == id
                    select a;
            return await q.SingleOrDefaultAsync();
        }







        //撈全部訂單資料和細項
        // GET: api/BusinessOrders/GetOrderAll
        [HttpGet("GetOrderAll")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetOrderAll()
        {
            //var b = _context.BusinessOrders.Include

            //var q = from ord in _context.BusinessOrders
            //        join od in _context.BusinessOrderDetails on ord.OrderId equals od.OrderId
            //        join opl in _context.BusinessOptionals on od.OptionalId equals opl.OptionalId
            //        join a in _context.BusinessAreas on ord.AreaId equals a.AreaId
            //        //join ppl in _context.ProductionProcessLists on ord.OrderId equals ppl.OrderId
            //        //join pps in _context.ProductionProcessStatusNames on ppl.StatusId equals pps.StatusId
            //        //join pp in _context.ProductionProcesses on ppl.ProcessId equals pp.ProcessId
            //        /*where opl.CategoryId == 1 && ord.IsAccepted==true*/
            //        select new
            //        {
            //            OrderNumber = ord.OrderNumber,
            //            OptionalName = opl.OptionalName,
            //            IsAccepted = ord.IsAccepted,
            //            Area = a.AreaName,
            //            //process = pp.ProcessName,
            //            //processstate = pps.StatusName,
            //            orderdatetime = ord.OrderDateTime,
            //            editdatetime = ord.EditDatetime,



            //            OrderId = ord.OrderId,
            //            OptionalId = od.OptionalId,
            //            CategoryId = opl.CategoryId,
            //            Price = opl.Price,


            //            //ordiddis=_context.BusinessOrders.Select(x=>x.OrderId)
            //        };

            //var p = _context.leftjoin.FromSqlRaw<leftjoin>(@"
            //      select o.OrderId,
            //o.OrderNumber,
            //o.OrderDateTime,
            //o.EditDatetime,
            //o.IsAccepted,
            //op.OptionalName,
            //a.AreaName,
            //pa.AreaName as AreaNameProcess,
            //pp.ProcessName
            //      from [dbo].[BusinessOrder] as o
            //      join [dbo].[BusinessOrderDetail] as od on o.OrderId=od.OrderId
            //      join [dbo].[BusinessOptional] as op on od.OptionalId=op.OptionalId
            //      join [dbo].[BusinessArea] as a on o.AreaId=a.AreaId
            //      left join [dbo].[ProductionProcessList] as ppl on ppl.OrderId=o.OrderId
            //      left join [dbo].[ProductionProcess] as pp on ppl.ProcessId=pp.ProcessId
            //      left join [dbo].[ProductionArea] as pa on ppl.AreaId=pa.AreaId");

            var q = _context.BusinessOrders.Select(a => new
            {
                a.OrderId,
                a.OrderNumber,
                a.OrderDateTime,
                a.EditDatetime,
                a.Area.AreaName,
                a.IsAccepted,
                detail = a.BusinessOrderDetails.Select(b => new
                {
                    OdId = b.OdId,
                    Optional = new
                    {
                        OptionalName = b.Optional.OptionalName /*,
                        Price = b.Optional.Price,*/
                    }
                }),
                //p
            });;



            return await q.ToListAsync();
            //return await q.ToListAsync();
        }




        //新增父子資料
        // POST: api/BusinessOrders/withoutloop
        [HttpPost("withoutloop")]
        public string PostOrder([FromBody] ICollection<BusinessOrderDetail> bod ,
            string ordnum ,
            //string orddate ,
            int    areaid,
            int    price,
            //int    empid,
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

                //找第一位業務部員工
                var emp = _context.PersonnelProfileDetails
                    .Where(a => a.DepartmentId == 3)
                    .Select(x => x.EmployeeId)
                    .First();


                BusinessOrder insert = new BusinessOrder
                {
                    OrderNumber          = ordnum,
                    OrderDateTime        = DateTime.Now,
                    AreaId               = areaid,
                    Price                = price,
                    EmployeeId           = emp,
                    IsAccepted           = isacc,
                    BusinessOrderDetails = bod
                };

                _context.BusinessOrders.Add(insert);
                _context.SaveChanges();
                return "訂單新增成功!";
            }
            else
            {
                return "last";
            };
        }



        //修改父子資料(目前未成功)
        // PUT: api/BusinessOrders/withoutloop?ordnum=M011672502400&areaid=3&price=9999998
        [HttpPut("withoutloop")]
        public dynamic PutOrder(string ordnum, int areaid, int price, [FromBody] ICollection<BusinessOrderDetail> bodput)
        {
            //子資料先修改
            foreach (var item in bodput)
            {
                BusinessOrderDetail son = new BusinessOrderDetail
                {            
                    OdId = item.OdId,
                    OrderId = item.OrderId,
                    OptionalId = item.OptionalId
                };
                _context.BusinessOrderDetails.Update(son);
            }

            //父資料再修改
            //找業務部員工
            var emp = _context.PersonnelProfileDetails
                .Where(a => a.DepartmentId == 3)
                .Select(x => x.EmployeeId)
                .First();
            //找訂單在資料庫既有的資料
            var q = _context.BusinessOrders
                    .Where(x => x.OrderNumber == ordnum)
                    .Select(a => new { 
                    a.OrderId,
                    a.OrderNumber,
                    a.OrderDateTime 
                    }).SingleOrDefault();

            BusinessOrder USAfather = new BusinessOrder
            {
                OrderId = q.OrderId,
                OrderNumber = q.OrderNumber,
                OrderDateTime = q.OrderDateTime,
                EditDatetime = DateTime.Now,
                AreaId = areaid,
                Price = price,
                EmployeeId = emp,
                IsAccepted = false
                //,BusinessOrderDetails = bodput
            };


            _context.BusinessOrders.Update(USAfather);
            var cnt =_context.SaveChanges();
            return cnt;
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
