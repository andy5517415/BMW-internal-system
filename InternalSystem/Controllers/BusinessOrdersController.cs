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
                        OrderDateTime = ord.OrderDateTime,
                        ord.IsAccepted
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



        //複合式查詢訂單
        //GET: api/BusinessOrders/GetOrderAllFilter
        [HttpGet("GetOrderAllFilter")]
        public dynamic GetOrderAllFilter(/*string cartype, string area*/)
        {

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
                        OptionalName = b.Optional.OptionalName,
                        optionalId = b.OptionalId
                    }
                }),
                //p
            }).OrderByDescending(a => a.OrderId);

            //var p = q.Where(a => a.detail.Where(b => b.Optional.optionalId == 2);

            //List<dynamic> lt = new List<dynamic>();
            //    foreach (var i in q)
            //    {
            //        lt.Add(i.detail.Where(a=>a.Optional.optionalId==2));
            //    }
            //if (!string.IsNullOrWhiteSpace(cartype))
            //{
            //        //q = q.Where(a => a.detail[0].optional.optionalId.Contains(cartype));
            //    //foreach (var item in q)
            //    //{
            //    //}
            //}
            //if (!string.IsNullOrWhiteSpace(area))
            //{
            //    q = q.Where(a => a.EmployeeName.Contains(area));
            //}

            return q;
        }



        ////直送sql指令
        //// GET: api/BusinessOrders/GetOrderAllSql
        //[HttpGet("GetOrderAllSql")]
        //public async Task<ActionResult<IEnumerable<dynamic>>> GetOrderAllFromsqlraw()
        //{

        //    var p = _context.leftjoin.FromSqlRaw<Leftjoin>(@"
        //          select o.OrderId,
        //    o.OrderNumber,
        //    o.OrderDateTime,
        //    o.EditDatetime,
        //    o.IsAccepted,
        //    a.AreaName,
        //    pa.AreaName as AreaNameProcess,
        //    pp.ProcessName
        //          from [dbo].[BusinessOrder] as o
        //          join [dbo].[BusinessArea] as a on o.AreaId=a.AreaId
        //          left join [dbo].[ProductionProcessList] as ppl on ppl.OrderId=o.OrderId
        //          left join [dbo].[ProductionProcess] as pp on ppl.ProcessId=pp.ProcessId
        //          left join [dbo].[ProductionArea] as pa on ppl.AreaId=pa.AreaId");


        //    //var p = _context.leftjoin.FromSqlRaw<leftjoin>("EXEC p_left");

        //    return await p.ToListAsync();
        //}



        //撈全部訂單資料和細項
        // GET: api/BusinessOrders/GetOrderAll
        [HttpGet("GetOrderAll")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetOrderAll()
        {
            //string sql = @"
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
            //      left join [dbo].[ProductionArea] as pa on ppl.AreaId=pa.AreaId";
            //var p = _context.leftjoin.FromSqlRaw<leftjoin>(sql).ToList();            

            var q = _context.BusinessOrders.Select(a => new
            {
                a.OrderId,
                a.OrderNumber,
                a.OrderDateTime,
                a.EditDatetime,
                a.Area.AreaName,
                a.IsAccepted,
                a.Price,
                detail = a.BusinessOrderDetails.Select(b => new
                {
                    OdId = b.OdId,
                    Optional = new
                    {
                        OptionalName = b.Optional.OptionalName,
                        optionalId = b.OptionalId
                    }
                }),
                //a.leftjoin
            }).OrderByDescending(a => a.OrderId);



            return await q.ToListAsync();
        }




        //新增父子資料
        // POST: api/BusinessOrders/withoutloop
        [HttpPost("withoutloop")]
        public dynamic PostOrder([FromBody] ICollection<BusinessOrderDetail> bod ,string type,int areaid)
        {

            if (bod != null && type != null && areaid.ToString() != null)
            {
                //找第一位業務部員工
                var emp = _context.PersonnelProfileDetails
                    .Where(a => a.DepartmentId == 3)
                    .Select(x => x.EmployeeId)
                    .First();
                //組裝訂單編號
                var ordnum = $"{type}0{areaid}{DateTimeOffset.Now.ToUnixTimeSeconds()}";


                //算本訂單價錢
                var money = 0;
                foreach (var item in bod)
                {
                    money += _context.BusinessOptionals.Where(x => x.OptionalId == item.OptionalId).Select(a => a.Price).First();
                }

                BusinessOrder insert = new BusinessOrder
                {
                    OrderNumber = ordnum,
                    OrderDateTime = DateTime.Now,
                    AreaId = areaid,
                    Price = money,
                    EmployeeId = emp,
                    IsAccepted = false,
                    BusinessOrderDetails = bod
                };

                _context.BusinessOrders.Add(insert);
                _context.SaveChanges();
                return "訂單新增成功!";
            }
            else
            {
                return "有誤!";
            };
        }



        //修改父子資料(分開作成功)
        // PUT: api/BusinessOrders/withoutloop?ordnum=M011672502400&areaid=3&price=9999998
        [HttpPut("withoutloop")]
        public dynamic PutOrder(string ordnum, int areaid, [FromBody] ICollection<BusinessOrderDetail> bodput)
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
            //找第一位業務部員工
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

            //算修改後的訂單價錢
            var money = 0;
            foreach (var item in bodput)
            {
                money += _context.BusinessOptionals.Where(x => x.OptionalId == item.OptionalId).Select(a => a.Price).First();
            }

            BusinessOrder USAfather = new BusinessOrder
            {
                OrderId = q.OrderId,
                OrderNumber = q.OrderNumber,
                OrderDateTime = q.OrderDateTime,
                EditDatetime = DateTime.Now,
                AreaId = areaid,
                Price = money,
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
