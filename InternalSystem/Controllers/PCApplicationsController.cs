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
    public class PCApplicationsController : ControllerBase
    {
        private readonly MSIT44Context _context;

        public PCApplicationsController(MSIT44Context context)
        {
            _context = context;
        }

        //申請表
        //GET: api/PCApplications
       [HttpGet("test")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetApplicationsList()
        {
            var i = from AP in this._context.PcApplications
                   join SU in this._context.PcSupplierLists on AP.SupplierId equals SU.SupplierId
                   join ARS in this._context.PcApplicationRecordSearches on AP.PurchaseId equals ARS.PurchaseId
                   join PPD in this._context.PersonnelProfileDetails on ARS.EmployeeId equals PPD.EmployeeId
                   join PD in this._context.PersonnelDepartmentLists on PPD.DepartmentId equals PD.DepartmentId
                   select new
                   {
                       EmployeeName =  PPD.EmployeeName,
                       DepartmentName = PD.DepName,
                       Date = AP.Date,
                       PurchaseId = AP.PurchaseId,
                       Supplier = AP.Supplier,
                       SupplierPhone = SU.SupplierPhone,
                       Comment = AP.Comment
                   };

            return await i.ToListAsync();
        }

        // GET: api/PCApplications
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PcApplication>>> GetPcApplications()
        {
            return await _context.PcApplications.ToListAsync();
        }

        // GET: api/PCApplications/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PcApplication>> GetPcApplication(int id)
        {
            var pcApplication = await _context.PcApplications.FindAsync(id);

            if (pcApplication == null)
            {
                return NotFound();
            }

            return pcApplication;
        }

        // PUT: api/PCApplications/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPcApplication(int id, PcApplication pcApplication)
        {
            if (id != pcApplication.OrderId)
            {
                return BadRequest();
            }

            _context.Entry(pcApplication).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PcApplicationExists(id))
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

        // POST: api/PCApplications
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PcApplication>> PostPcApplication(PcApplication pcApplication)
        {
            _context.PcApplications.Add(pcApplication);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPcApplication", new { id = pcApplication.OrderId }, pcApplication);
        }

        // DELETE: api/PCApplications/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePcApplication(int id)
        {
            var pcApplication = await _context.PcApplications.FindAsync(id);
            if (pcApplication == null)
            {
                return NotFound();
            }

            _context.PcApplications.Remove(pcApplication);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PcApplicationExists(int id)
        {
            return _context.PcApplications.Any(e => e.OrderId == id);
        }
    }
}
