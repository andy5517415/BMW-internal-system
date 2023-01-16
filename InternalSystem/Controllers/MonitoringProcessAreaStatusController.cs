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






        //自己寫的
        // GET: api/MonitoringProcessAreaStatus/1/1/iX xDrive40 旗艦版
        [HttpGet("{areaid}/{processId}/{cartype}")]
        public async Task<ActionResult<dynamic>> GetMonitoringProcessAreaStatus(int areaid, int processId, string cartype)
        {

            var q = from m in _context.MonitoringProcessAreaStatuses
                    where m.AreaId== areaid && m.ProcessId == processId && m.CarType== cartype
                    select new { 
                        status = m.Status,
                        MonitorId = m.MonitorId
                    };


            return await q.SingleOrDefaultAsync();
        }




        //自己寫的
        // GET: api/MonitoringProcessAreaStatus/description/1/1/iX xDrive40 旗艦版
        [HttpGet("description/{areaid}/{processId}/{cartype}")]
        public async Task<ActionResult<dynamic>> GetDescription(int areaid, int processId, string cartype)
        {
            var q = from m in _context.MonitoringProcessAreaStatuses
                    where m.AreaId == areaid && m.ProcessId == processId && m.CarType == cartype
                    select m.Description;

            return await q.SingleOrDefaultAsync();
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
