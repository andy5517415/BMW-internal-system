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
    public class ProductionProcessListsController : ControllerBase
    {
        private readonly MSIT44Context _context;

        public ProductionProcessListsController(MSIT44Context context)
        {
            _context = context;
        }

        //異常清單(全部+複合式查詢訂單及異常等級)
        // GET: api/ProductionProcessLists/GetBugList
        [HttpGet("GetBugList")]
        public async Task<ActionResult<dynamic>> GetBugList(string orderNumber, string date,string title)
        {
            var BugList = from PBC in this._context.ProductionBugContexts
                          join PPL in this._context.ProductionProcessLists on PBC.ProcessId equals PPL.ProcessId
                          join PA in this._context.ProductionAreas on PPL.AreaId equals PA.AreaId
                          join PP in this._context.ProductionProcesses on PPL.ProcessId equals PP.ProcessId
                          join PPSN in this._context.ProductionProcessStatusNames on PPL.StatusId equals PPSN.StatusId
                          join BO in this._context.BusinessOrders on PPL.OrderId equals BO.OrderId
                          join BOD in this._context.BusinessOrderDetails on BO.OrderId equals BOD.OrderId
                          join BOT in this._context.BusinessOptionals on BOD.OptionalId equals BOT.OptionalId
                          join BC in this._context.BusinessCategories on BOT.CategoryId equals BC.CategoryId
                          where PBC.OrderId == PPL.OrderId && BC.CategoryId == 1
                          select new
                          {
                              OrderId = PBC.OrderId,
                              OrderNumber = BO.OrderNumber,
                              OptionalName = BOT.OptionalName,
                              AreaId = PBC.AreaId,
                              AreaName = PA.AreaName,
                              ProcessId = PBC.ProcessId,
                              ProcessName = PP.ProcessName,
                              Date = PBC.Date.ToString(),
                              StartTime = PBC.StartTime,
                              EndTime = PBC.EndTime,
                              Title = PBC.Title,
                              Context = PBC.Context,
                              Rank = PBC.Rank,
                              Dispose = PBC.Dispose,
                              Photo = PBC.Photo
                          };

            if (!string.IsNullOrWhiteSpace(orderNumber))
            {
                BugList = BugList.Where(a => a.OrderNumber.Contains(orderNumber));
            }
            if (!string.IsNullOrWhiteSpace(title))
            {
                BugList = BugList.Where(a => a.Title.Contains(title));
            }
            if (!string.IsNullOrWhiteSpace(date))
            {
                BugList = BugList.Where(a => a.Date.Contains(date));
            }
            
            return await BugList.FirstOrDefaultAsync();
        }

        //異常清單(全部+複合式查詢訂單及異常等級)
        // GET: api/ProductionProcessLists/GetBugList
        [HttpGet("GetBugListAll")]
        public async Task<ActionResult<dynamic>> GetBugListAll(string orderNumber , string rank)
        {
            var BugList = from PBC in this._context.ProductionBugContexts
                          join PPL in this._context.ProductionProcessLists on PBC.ProcessId equals PPL.ProcessId
                          join PA in this._context.ProductionAreas on PPL.AreaId equals PA.AreaId
                          join PP in this._context.ProductionProcesses on PPL.ProcessId equals PP.ProcessId
                          join PPSN in this._context.ProductionProcessStatusNames on PPL.StatusId equals PPSN.StatusId
                          join BO in this._context.BusinessOrders on PPL.OrderId equals BO.OrderId
                          join BOD in this._context.BusinessOrderDetails on BO.OrderId equals BOD.OrderId
                          join BOT in this._context.BusinessOptionals on BOD.OptionalId equals BOT.OptionalId
                          join BC in this._context.BusinessCategories on BOT.CategoryId equals BC.CategoryId
                          where PBC.OrderId == PPL.OrderId && BC.CategoryId == 1
                          select new
                                {
                                    OrderId = PBC.OrderId,
                                    OrderNumber = BO.OrderNumber,
                                    Date = PBC.Date.ToString(),
                                    Title = PBC.Title,
                                    Rank = PBC.Rank

                                };

            if (!string.IsNullOrWhiteSpace(orderNumber))
            {
                BugList = BugList.Where(a => a.OrderNumber.Contains(orderNumber));
            }
            if (!string.IsNullOrWhiteSpace(rank))
            {
                BugList = BugList.Where(a => a.Rank.Contains(rank));
            }
            return await BugList.ToListAsync();
        }

        //異常清單(全部+複合式查詢訂單、異常等級及日期)
        // GET: api/ProductionProcessLists/GetBugListDate
        [HttpGet("GetBugListDate")]
        public async Task<ActionResult<dynamic>> GetBugListDate(string orderNumber, string rank , string startDate , string endDate)
        {
            var std = DateTime.Parse(startDate);
            var edd = DateTime.Parse(endDate);

            var BugList = from PBC in this._context.ProductionBugContexts
                          join PPL in this._context.ProductionProcessLists on PBC.ProcessId equals PPL.ProcessId
                          join PA in this._context.ProductionAreas on PPL.AreaId equals PA.AreaId
                          join PP in this._context.ProductionProcesses on PPL.ProcessId equals PP.ProcessId
                          join PPSN in this._context.ProductionProcessStatusNames on PPL.StatusId equals PPSN.StatusId
                          join BO in this._context.BusinessOrders on PPL.OrderId equals BO.OrderId
                          join BOD in this._context.BusinessOrderDetails on BO.OrderId equals BOD.OrderId
                          join BOT in this._context.BusinessOptionals on BOD.OptionalId equals BOT.OptionalId
                          join BC in this._context.BusinessCategories on BOT.CategoryId equals BC.CategoryId
                          where PBC.OrderId == PPL.OrderId && BC.CategoryId == 1 && PBC.Date >= std && PBC.Date <= edd
                          select new
                          {
                              OrderId = PBC.OrderId,
                              OrderNumber = BO.OrderNumber,
                              Date = PBC.Date.ToString(),
                              Title = PBC.Title,
                              Rank = PBC.Rank
                          };

            if (!string.IsNullOrWhiteSpace(orderNumber))
            {
                BugList = BugList.Where(a => a.OrderNumber.Contains(orderNumber));
            }
            if (!string.IsNullOrWhiteSpace(rank))
            {
                BugList = BugList.Where(a => a.Rank.Contains(rank));
            }
            return await BugList.ToListAsync();
        }

        // 異常清單建立
        // GET: api/ProductionProcessLists/bugListCreate
        [HttpGet("bugListCreate/{id}")]
        public async Task<ActionResult<dynamic>> GetbugCreate(int id)
        {
            var BugList = from PPL in this._context.ProductionProcessLists
                       join PA in this._context.ProductionAreas on PPL.AreaId equals PA.AreaId
                       join PP in this._context.ProductionProcesses on PPL.ProcessId equals PP.ProcessId
                       join PPSN in this._context.ProductionProcessStatusNames on PPL.StatusId equals PPSN.StatusId
                       join BO in this._context.BusinessOrders on PPL.OrderId equals BO.OrderId
                       join BOD in this._context.BusinessOrderDetails on BO.OrderId equals BOD.OrderId
                       join BOT in this._context.BusinessOptionals on BOD.OptionalId equals BOT.OptionalId
                       join BC in this._context.BusinessCategories on BOT.CategoryId equals BC.CategoryId
                       where PPL.StatusId == 2 && BC.CategoryId == 1 && PPL.OrderId == id
                          select new
                       {
                           OrderId = PPL.OrderId,
                           OrderNumber = BO.OrderNumber,
                           OptionalName = BOT.OptionalName,
                           AreaId = PPL.AreaId,
                           AreaName = PA.AreaName,
                           ProcessId = PP.ProcessId,
                           ProcessName = PP.ProcessName,
                       };

            return await BugList.FirstOrDefaultAsync();
        }

        // 業務訂單狀態
        // GET: api/ProductionProcessLists/ordercheak
        [HttpGet("orderlist")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetOrderList()
        {
            var orderList = from BO in this._context.BusinessOrders
                            select new
                              {
                                IsAccepted = BO.IsAccepted
                              };




            return await orderList.ToListAsync();
        }


        //確認訂單內容、發送到製程
        // GET: api/ProductionProcessLists/ordercheak
        [HttpGet("ordercheak")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetOrderCheak()
        {
            var processList = from PPL in this._context.ProductionProcessLists
                              select new
                              {
                                  OrderId = PPL.OrderId,
                                  ProcessId = PPL.ProcessId,
                                  AreaId = PPL.AreaId,
                                  StarDate = PPL.StarDate.ToString(),
                              };
                             

                              

            return await processList.ToListAsync();
        }

        //車子訂單號碼
        // GET: api/ProductionProcessLists/OrderNumber
        [HttpGet("OrderNumber")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetOrderNumber()
        {
            var orderNunber = from PPL in this._context.ProductionProcessLists
                              join BO in this._context.BusinessOrders on PPL.OrderId equals BO.OrderId
                              where PPL.StatusId == 2
                              select new
                              {
                                  OrderId = BO.OrderId,
                                  OrderNumber = BO.OrderNumber
                              };

            return await orderNunber.ToListAsync();
        }

        //車子型號
        // GET: api/ProductionProcessLists/model
        [HttpGet("model")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetModel()
        {
            var processList = from BO in this._context.BusinessOptionals
                              join BC in this._context.BusinessCategories on BO.CategoryId equals BC.CategoryId
                              where BC.CategoryId == 1
                              select new
                              {
                                  OptionalId = BO.OptionalId,
                                  OptionalName = BO.OptionalName
                              };

            return await processList.ToListAsync();
        }

        //製程單
        // GET: api/ProductionProcessLists/process
        [HttpGet("process")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetProcess()
        {
            var processList = from PP in this._context.ProductionProcesses
                              select PP;

            return await processList.ToListAsync();
        }

        //報工的大表單
        // GET: api/ProductionProcessLists/Processor/{id}/{carid}
        [HttpGet("Processor/{id}/{carid}")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetProductionProcessLists(int id, int carid)
        {
            var List = from PPL in this._context.ProductionProcessLists
                       join PA in this._context.ProductionAreas on PPL.AreaId equals PA.AreaId
                       join PP in this._context.ProductionProcesses on PPL.ProcessId equals PP.ProcessId
                       join PPSN in this._context.ProductionProcessStatusNames on PPL.StatusId equals PPSN.StatusId
                       join BO in this._context.BusinessOrders on PPL.OrderId equals BO.OrderId
                       join BOD in this._context.BusinessOrderDetails on BO.OrderId equals BOD.OrderId
                       join BOT in this._context.BusinessOptionals on BOD.OptionalId equals BOT.OptionalId
                       join BC in this._context.BusinessCategories on BOT.CategoryId equals BC.CategoryId
                       where BC.CategoryId == 1 && PPSN.StatusId == 2 && PP.ProcessId == id && BOT.OptionalId == carid
                       select new
                       {
                           OrderId = PPL.OrderId,
                           OrderNumber = BO.OrderNumber,
                           AreaId = PPL.AreaId,
                           AreaName = PA.AreaName,
                           ProcessId = PP.ProcessId,
                           ProcessName = PP.ProcessName,
                           StarDate = PPL.StarDate.ToString(),
                           OptionalId = BOT.OptionalId,
                           OptionalName = BOT.OptionalName,
                           StatusId = PPL.StatusId,
                           StatusName = PPSN.StatusName,
                           DeadlineDateTime = BO.DeadlineDateTime

                       };

            return await List.ToListAsync();
        }

        // 現場主管代辦事項
        // GET: api/ProductionProcessLists/BusinessOrderProcessor
        [HttpGet("BusinessOrderProcessor")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetBusinessOrderTodo()
        {
            var List = from BO in this._context.BusinessOrders
                       join BOD in this._context.BusinessOrderDetails on BO.OrderId equals BOD.OrderId
                       join BOT in this._context.BusinessOptionals on BOD.OptionalId equals BOT.OptionalId
                       join BC in this._context.BusinessCategories on BOT.CategoryId equals BC.CategoryId
                       orderby BO.OrderDateTime descending
                       where BO.IsAccepted == false && BC.CategoryId == 1
                       select new
                       {
                           OrderId = BO.OrderId,
                           OrderNumber = BO.OrderNumber,
                           OptionalId = BOT.OptionalId,
                           OptionalName = BOT.OptionalName,
                           IsAccepted = BO.IsAccepted,
                           DeadlineDateTime = BO.DeadlineDateTime
                       };

            return await List.ToListAsync();
        }

        // 現場主管代辦事項，製程完成回到主管代辦
        // GET: api/ProductionProcessLists/ProcessTodo
        [HttpGet("ProcessTodo")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetProcessTodo()
        {
            var List = from PPL in this._context.ProductionProcessLists
                       join PP in this._context.ProductionProcesses on PPL.ProcessId equals PP.ProcessId
                       join BO in this._context.BusinessOrders on PPL.OrderId equals BO.OrderId
                       join BOD in this._context.BusinessOrderDetails on BO.OrderId equals BOD.OrderId
                       join BOT in this._context.BusinessOptionals on BOD.OptionalId equals BOT.OptionalId
                       join BC in this._context.BusinessCategories on BOT.CategoryId equals BC.CategoryId
                       where PPL.StatusId == 1 && BC.CategoryId == 1
                       select new
                       {
                           OrderId = PPL.OrderId,
                           ProcessId = PP.ProcessId,
                           ProcessName = PP.ProcessName,
                           OrderNumber = BO.OrderNumber,
                           OptionalId = BOT.OptionalId,
                           OptionalName = BOT.OptionalName,
                           StatusId = PPL.StatusId,
                           DeadlineDateTime = BO.DeadlineDateTime
                       };

            return await List.ToListAsync();
        }
        //新訂單-訂單內容
        // GET: api/ProductionProcessLists/BusinessOrderProcessor/5
        [HttpGet("BusinessOrderProcessor/{id}")]
        public async Task<ActionResult<dynamic>> GetBusinessOrder(int id)
        {
            var List = from BO in this._context.BusinessOrders          
                       join BOD in this._context.BusinessOrderDetails on BO.OrderId equals BOD.OrderId
                       join BOT in this._context.BusinessOptionals on BOD.OptionalId equals BOT.OptionalId
                       join BC in this._context.BusinessCategories on BOT.CategoryId equals BC.CategoryId
                       join BA in this._context.BusinessAreas on BO.AreaId equals BA.AreaId
                       where BO.IsAccepted == false && BO.OrderId == id
                       
                       select new
                       {
                           OrderId = BO.OrderId,
                           OrderNumber = BO.OrderNumber,
                           OptionalId = BOT.OptionalId,
                           OptionalName = BOT.OptionalName,
                           Price = BOT.Price
                       };
            if (List == null)
            {
                return "沒有資料";
            }


            return await List.ToListAsync();
        }

        //製程回來-訂單內容
        // GET: api/ProductionProcessLists/BusinessOrderProcessorBack/5
        [HttpGet("BusinessOrderProcessorBack/{id}")]
        public async Task<ActionResult<dynamic>> GetProcessBusinessOrder(int id)
        {
            var List = from BO in this._context.BusinessOrders
                       join BOD in this._context.BusinessOrderDetails on BO.OrderId equals BOD.OrderId
                       join BOT in this._context.BusinessOptionals on BOD.OptionalId equals BOT.OptionalId
                       join BC in this._context.BusinessCategories on BOT.CategoryId equals BC.CategoryId
                       join BA in this._context.BusinessAreas on BO.AreaId equals BA.AreaId
                       where BO.OrderId == id

                       select new
                       {
                           OrderId = BO.OrderId,
                           OrderNumber = BO.OrderNumber,
                           OptionalId = BOT.OptionalId,
                           OptionalName = BOT.OptionalName,
                           Price = BOT.Price,
                           DeadlineDateTime = BO.DeadlineDateTime
                       };
            if (List == null)
            {
                return "沒有資料";
            }


            return await List.ToListAsync();
        }


        //GET: api/ProductionProcessLists
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductionProcessList>>> GetProductionProcessLists()
        {
            return await _context.ProductionProcessLists.ToListAsync();
        }

        // GET: api/ProductionProcessLists/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductionProcessList>> GetProductionProcessList(int id)
        {
            var productionProcessList = await _context.ProductionProcessLists.FindAsync(id);

            if (productionProcessList == null)
            {
                return NotFound();
            }

            return productionProcessList;
        }


        // PUT: api/ProductionProcessLists/putBug
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("putBug/{orderid}/{date}/{title}")]
        public async Task<IActionResult> PutProductionProcessListPutBug(int orderid, string date , string title, ProductionBugContext productionBugContext)
        {
            if (orderid != productionBugContext.OrderId)
            {
                return BadRequest();
            }

            _context.Entry(productionBugContext).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (productionBugContext.OrderId != orderid)
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


        // PUT: api/ProductionProcessLists/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductionProcessList(int id, ProductionProcessList productionProcessList)
        {
            if (id != productionProcessList.OrderId)
            {
                return BadRequest();
            }

            _context.Entry(productionProcessList).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductionProcessListExists(id))
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

        // POST: api/ProductionProcessLists
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProductionProcessList>> PostProductionProcessList(ProductionProcessList productionProcessList)
        {
            _context.ProductionProcessLists.Add(productionProcessList);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ProductionProcessListExists(productionProcessList.OrderId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetProductionProcessList", new { id = productionProcessList.OrderId }, productionProcessList);
        }

        // DELETE: api/ProductionProcessLists/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductionProcessList(int id)
        {
            var productionProcessList = await _context.ProductionProcessLists.FindAsync(id);
            if (productionProcessList == null)
            {
                return NotFound();
            }

            _context.ProductionProcessLists.Remove(productionProcessList);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductionProcessListExists(int id)
        {
            return _context.ProductionProcessLists.Any(e => e.OrderId == id);
        }
    }
}
