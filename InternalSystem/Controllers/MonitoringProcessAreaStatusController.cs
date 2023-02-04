using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InternalSystem.Models;
using System.Diagnostics;
using System.Threading;

namespace InternalSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonitoringProcessAreaStatusController : ControllerBase
    {
        private readonly MSIT44Context _context;

        public MonitoringProcessAreaStatusController(MSIT44Context context)
        {
            _context = context;
        }


        //監控預載入
        // GET: api/MonitoringProcessAreaStatus/1/1/iX xDrive40 旗艦版
        [HttpGet("{areaid}/{processId}/{cartype}")]
        public async Task<ActionResult<dynamic>> GetMonitoringProcessAreaStatus(int areaid, int processId, string cartype)
        {
            var q = from m in _context.MonitoringProcessAreaStatuses
                    where m.AreaId == areaid && m.ProcessId == processId && m.CarType == cartype
                    select new
                    {
                        status = m.Status,
                        MonitorId = m.MonitorId
                    };

            return await q.SingleOrDefaultAsync();
        }



        //報錯系統修改狀態及更新敘述(填完報錯系統根據狀態按鈕送出)
        // GET: api/MonitoringProcessAreaStatus/putstatus?areaid=1&processId=1&cartype=M4 Competition&stateName=異常&des=999
        [HttpPut("putstatus")]
        public dynamic GetMonitoringProcessAreaStatus(int areaid, int processId,string cartype,string stateName,string des)
        {
            if (!string.IsNullOrEmpty(areaid.ToString()) && 
                !string.IsNullOrEmpty(processId.ToString()) &&
                !string.IsNullOrEmpty(cartype) &&
                !string.IsNullOrEmpty(stateName) &&
                !string.IsNullOrEmpty(des)
                )
            {
                var getmid = _context.MonitoringProcessAreaStatuses
                    .Where(x => x.AreaId == areaid && x.ProcessId == processId && x.CarType == cartype)
                    .Select(x => x.MonitorId).SingleOrDefault();

                MonitoringProcessAreaStatus mp = new MonitoringProcessAreaStatus
                {
                    MonitorId=getmid,
                    AreaId= areaid,
                    ProcessId= processId,
                    CarType= cartype,
                    Status= stateName,
                    Description=des
                };
                _context.MonitoringProcessAreaStatuses.Update(mp);
                _context.SaveChanges();
                return "commit";
            }
            else
            {
                return "rollback";
            }
        }




        //監控系統撈目前各廠區製程錯誤狀態敘述(點異常框跳出錯誤訊息)
        // GET: api/MonitoringProcessAreaStatus/description/1/1/iX xDrive40 旗艦版
        [HttpGet("description/{areaid}/{processId}/{cartype}")]
        public async Task<ActionResult<dynamic>> GetDescription(int areaid, int processId, string cartype)
        {
            var q = from m in _context.MonitoringProcessAreaStatuses
                    where m.AreaId == areaid && m.ProcessId == processId && m.CarType == cartype
                    select m.Description;

            return await q.SingleOrDefaultAsync();
        }





        //報錯系統撈訂單資料(已接單且狀態生產中)(報錯系統下拉式選單)
        // GET: api/MonitoringProcessAreaStatus/bugsysgetord
        [HttpGet("bugsysgetord")]
        public async Task<ActionResult<dynamic>> GetBugOrder()
        {
            var q = from pp in _context.ProductionProcessLists
                    join bo in _context.BusinessOrders on pp.OrderId equals bo.OrderId
                    join bod in _context.BusinessOrderDetails on bo.OrderId equals bod.OrderId
                    join bop in _context.BusinessOptionals on bod.OptionalId equals bop.OptionalId
                    where pp.StatusId == 2 && bop.CategoryId == 1 && bo.IsAccepted==true
                    select new
                    {
                        bo.OrderNumber,
                        pp.ProcessId,
                        pp.AreaId,
                        bop.OptionalName
                    };

            return await q.ToListAsync();
        }



















        // GET: api/MonitoringProcessAreaStatus
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MonitoringProcessAreaStatus>>> GetMonitoringProcessAreaStatuses()
        {
            return await _context.MonitoringProcessAreaStatuses.ToListAsync();
        }



        // GET: api/MonitoringProcessAreaStatus/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MonitoringProcessAreaStatus>> GetMonitoringProcessAreaStatus(int id)
        {
            var monitoringProcessAreaStatus = await _context.MonitoringProcessAreaStatuses.FindAsync(id);

            if (monitoringProcessAreaStatus == null)
            {
                return NotFound();
            }

            return monitoringProcessAreaStatus;
        }



        // PUT: api/MonitoringProcessAreaStatus/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMonitoringProcessAreaStatus(int id, MonitoringProcessAreaStatus monitoringProcessAreaStatus)
        {
            if (id != monitoringProcessAreaStatus.MonitorId)
            {
                return BadRequest();
            }

            _context.Entry(monitoringProcessAreaStatus).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MonitoringProcessAreaStatusExists(id))
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



        // POST: api/MonitoringProcessAreaStatus
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MonitoringProcessAreaStatus>> PostMonitoringProcessAreaStatus(MonitoringProcessAreaStatus monitoringProcessAreaStatus)
        {
            _context.MonitoringProcessAreaStatuses.Add(monitoringProcessAreaStatus);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (MonitoringProcessAreaStatusExists(monitoringProcessAreaStatus.AreaId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetMonitoringProcessAreaStatus", new { id = monitoringProcessAreaStatus.AreaId }, monitoringProcessAreaStatus);
        }



        // DELETE: api/MonitoringProcessAreaStatus/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMonitoringProcessAreaStatus(int id)
        {
            var monitoringProcessAreaStatus = await _context.MonitoringProcessAreaStatuses.FindAsync(id);
            if (monitoringProcessAreaStatus == null)
            {
                return NotFound();
            }

            _context.MonitoringProcessAreaStatuses.Remove(monitoringProcessAreaStatus);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MonitoringProcessAreaStatusExists(int id)
        {
            return _context.MonitoringProcessAreaStatuses.Any(e => e.AreaId == id);
        }
    }
}
