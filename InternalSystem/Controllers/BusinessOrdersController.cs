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
        private readonly MSIT44Context2 _context;

        public BusinessOrdersController(MSIT44Context2 context)
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
                        isAccepted = ord.IsAccepted,
                        deadline=ord.DeadlineDateTime
                    };
            return await q.ToListAsync();
        }


        //隱藏性欄位取得全部訂單編號
        // GET: api/BusinessOrders/hidden
        [HttpGet("hidden")]
        public async Task<ActionResult<IEnumerable<dynamic>>> HiddenGetOrderAll(string ordernum)
        {
            var q = from od in _context.BusinessOrders
                    orderby od.OrderId descending
                    select od.OrderNumber;
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

        //直送sql指令
        // GET: api/BusinessOrders/GetOrderAllSql
        [HttpGet("GetOrderAllSql")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetOrderAllFromsqlraw()
        {
            //var p = _context.Leftjoin.FromSqlRaw(@"
            //      select o.OrderId,
            //o.OrderNumber,
            //o.OrderDateTime,
            //o.EditDatetime,
            //o.IsAccepted,
            //a.AreaName,
            //pa.AreaName as AreaNameProcess,
            //pp.ProcessName
            //      from [dbo].[BusinessOrder] as o
            //      join [dbo].[BusinessArea] as a on o.AreaId=a.AreaId
            //      left join [dbo].[ProductionProcessList] as ppl on ppl.OrderId=o.OrderId
            //      left join [dbo].[ProductionProcess] as pp on ppl.ProcessId=pp.ProcessId
            //      left join [dbo].[ProductionArea] as pa on ppl.AreaId=pa.AreaId");
            var p = _context.Leftjoin.FromSqlRaw("EXEC p_left");
            return await p.ToListAsync();
        }



        //撈全部訂單資料和細項
        // GET: api/BusinessOrders/GetOrderAll
        [HttpGet("GetOrderAll")]
        public dynamic GetOrderAll()
        {
            //var q = _context.BusinessOrders.Select(a => new
            //{
            //    a.OrderId,
            //    a.OrderNumber,
            //    a.OrderDateTime,
            //    a.EditDatetime,
            //    a.Area.AreaName,
            //    a.IsAccepted,
            //    a.Price,
            //    detail = a.BusinessOrderDetails.Select(b => new
            //    {
            //        OdId = b.OdId,
            //        Optional = new
            //        {
            //            OptionalName = b.Optional.OptionalName,
            //            optionalId = b.OptionalId
            //        }
            //    })
            //}).OrderByDescending(a => a.OrderId);

            var q = (from bo in _context.BusinessOrders
                     join ppl in _context.ProductionProcessLists on bo.OrderId equals ppl.OrderId into a
                     from ppl in a.DefaultIfEmpty()
                     select new
                     {
                         bo.OrderId,
                         bo.OrderNumber,
                         bo.OrderDateTime,
                         bo.EditDatetime,
                         bo.DeadlineDateTime,
                         bo.Area.AreaName,
                         bo.Price,
                         bo.IsAccepted,
                         pparea = ppl == null ? "N/A" : ppl.Area.AreaName,
                         ppname = ppl == null ? "N/A" : ppl.Process.ProcessName,
                         ppstate = ppl == null ? "N/A" : ppl.Status.StatusName,

                         detail = bo.BusinessOrderDetails.Select(b => new {
                             b.OptionalId,
                             b.Optional.OptionalName
                         })

                         //bo.BusinessOrderDetails.Select(a => a.OptionalId)
                         //錯誤型態
                         //from bod in _context.BusinessOrderDetails 
                         //          join bp in _context.BusinessOptionals on bod.OptionalId equals bp.OptionalId
                         //          select new { 
                         //          bp.OptionalId,
                         //          bp.OptionalName
                         //          }

                         //detail= from bod in _context.BusinessOrderDetails
                         //        select new 
                         //        { 
                         //        bo.OrderId
                         //        }
                     }).OrderByDescending(d => d.OrderId).ThenByDescending(c => c.ppname);

            //過濾資料 有製程C撈C 有B撈B 有A撈A
            int oid = 0;
            List<dynamic> lt = new List<dynamic>();
            foreach (var item in q)
            {
                if (item.ppname == "N/A")
                {
                    lt.Add(item);
                }
                else if (item.ppname == "製程C-性能測試")
                {
                    if (oid != item.OrderId)
                    {
                        lt.Add(item);
                        oid = item.OrderId;
                    }
                }
                else if (item.ppname == "製程B-噴漆/內裝")
                {
                    if (oid != item.OrderId)
                    {
                        lt.Add(item);
                        oid = item.OrderId;
                    }
                }
                else if (item.ppname == "製程A-零件組裝")
                {
                    if (oid != item.OrderId)
                    {
                        lt.Add(item);
                        oid = item.OrderId;
                    }
                }
            }
            return lt.ToList();
        }





        //嘗試leftjoin
        // GET: api/BusinessOrders/GetOrderAllleftjoin
        [HttpGet("GetOrderAllleftjoin")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetOrderAllLeftjoin()
        {
           var q =  
            from bo in _context.BusinessOrders
            join ppl in _context.ProductionProcessLists on bo.OrderId equals ppl.OrderId into a
            from ppl in a.DefaultIfEmpty()
            select new
            {
                bo.OrderId,
                bo.OrderNumber,
                bo.OrderDateTime,
                bo.EditDatetime,
                bo.IsAccepted,
                bsarea=bo.Area.AreaName,
                pparea = ppl == null ? "N/A" : ppl.Area.AreaName,
                ppname = ppl == null ? "N/A" : ppl.Process.ProcessName,
                ppstate = ppl == null ? "N/A" : ppl.Status.StatusName
            };
            return await q.ToListAsync();
        }


        //新增父子資料
        // POST: api/BusinessOrders/withoutloop
        [HttpPost("withoutloop")]
        public dynamic PostOrder([FromBody] ICollection<BusinessOrderDetail> bod ,string type,int areaid,DateTime deadline)
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
                    DeadlineDateTime = deadline,
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
            int type = 0;
            //子資料先修改
            foreach (var item in bodput)
            {
                BusinessOrderDetail son = new BusinessOrderDetail
                {            
                    OdId = item.OdId,
                    OrderId = item.OrderId,
                    OptionalId = item.OptionalId
                };
                //撈修改後的型號修改訂單編號首字母
                if (item.OptionalId<4)
                {
                    type = item.OptionalId;
                }
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
                    a.OrderDateTime,
                    a.DeadlineDateTime,
                    }).SingleOrDefault();

            //改訂單首英文字母
            string strtemp = "";
            switch (type)
            {
                case 1:
                    strtemp = q.OrderNumber.Remove(0, 1).Insert(0, "M");
                    break;
                case 2:
                    strtemp = q.OrderNumber.Remove(0, 1).Insert(0, "X");
                    break;
                case 3:
                    strtemp = q.OrderNumber.Remove(0, 1).Insert(0, "i");
                    break;
                default:                    
                    break;
            }

            //算修改後的訂單價錢
            var money = 0;
            foreach (var item in bodput)
            {
                money += _context.BusinessOptionals.Where(x => x.OptionalId == item.OptionalId).Select(a => a.Price).First();
            }

            BusinessOrder USAfather = new BusinessOrder
            {
                OrderId = q.OrderId,
                OrderNumber = strtemp,
                OrderDateTime = q.OrderDateTime,
                EditDatetime = DateTime.Now,
                DeadlineDateTime = q.DeadlineDateTime,
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
