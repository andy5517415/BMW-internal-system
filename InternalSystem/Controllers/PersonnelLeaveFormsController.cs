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
    public class PersonnelLeaveFormsController : ControllerBase
    {
        private readonly MSIT44Context _context;

        public PersonnelLeaveFormsController(MSIT44Context context)
        {
            _context = context;
        }

        // GET: api/PersonnelLeaveForms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonnelLeaveForm>>> GetPersonnelLeaveForms()
        {
            return await _context.PersonnelLeaveForms.ToListAsync();
        }

        //用員工ID尋找
        // GET: api/PersonnelLeaveForms/employee/5/{y}-{m}
        [HttpGet("employee/{id}/{y}-{m}")]
        public async Task<ActionResult<dynamic>> GetPersonnelLeave(int id, int y ,int m)
        {
            var personnelLeaveForm = from pl in _context.PersonnelLeaveForms
                                     join o in _context.PersonnelProfileDetails on pl.EmployeeId equals o.EmployeeId
                                     join l in _context.PersonnelLeaveAuditStatuses on pl.StatusId equals l.StatusId
                                     where o.EmployeeId == id && pl.StartDate.Month ==m && pl.StartDate.Year==y
                                     select new
                                     {
                                         EmployeeName = o.EmployeeName,
                                         EmployeeNumber =o.EmployeeNumber,
                                         EmployeeId = pl.EmployeeId,
                                         StartDate = pl.StartDate.ToString("yyyy-MM-dd"),
                                         StartTime = pl.StartTime,
                                         EndDate = pl.EndDate.ToString("yyyy-MM-dd"),
                                         EndTime = pl.EndTime,
                                         LeaveType = pl.LeaveType,
                                         StatusId = pl.StatusId,
                                         AuditStatus = l.AuditStatus,
                                         Proxy = pl.Proxy,
                                         auditManerger = pl.AuditManerger,
                                         Reason = pl.Reason
                                     };

            if (personnelLeaveForm == null)
            {
                return NotFound();
            }
            
            return await  personnelLeaveForm.ToListAsync();
        }


        //用部門尋找
        // GET: api/PersonnelLeaveForms/department/5/{y}-{m}
        [HttpGet("department/{depId}/{y}-{m}")]
        public async Task<ActionResult<dynamic>> GetDepartmentLeave(int depId, int y, int m)
        {
            var personnelLeaveForm = from pl in _context.PersonnelLeaveForms
                                     join o in _context.PersonnelProfileDetails on pl.EmployeeId equals o.EmployeeId
                                     join l in _context.PersonnelLeaveAuditStatuses on pl.StatusId equals l.StatusId
                                     where o.DepartmentId == depId && pl.StartDate.Month == m && pl.StartDate.Year == y
                                     select new
                                     {
                                         EmployeeName = o.EmployeeName,
                                         EmployeeNumber = o.EmployeeNumber,
                                         EmployeeId = pl.EmployeeId,
                                         StartDate = pl.StartDate.ToString("yyyy-MM-dd"),
                                         StartTime = pl.StartTime,
                                         EndDate = pl.EndDate.ToString("yyyy-MM-dd"),
                                         EndTime = pl.EndTime,
                                         LeaveType = pl.LeaveType,
                                         StatusId = pl.StatusId,
                                         AuditStatus = l.AuditStatus,
                                         Proxy = pl.Proxy,
                                         auditManerger = pl.AuditManerger,
                                         Reason = pl.Reason
                                     };

            if (personnelLeaveForm == null)
            {
                return NotFound();
            }

            return await personnelLeaveForm.ToListAsync();
        }

        // GET: api/PersonnelLeaveForms/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PersonnelLeaveForm>> GetPersonnelLeaveForm(int id)
        {
            var personnelLeaveForm = await _context.PersonnelLeaveForms.FindAsync(id);

            if (personnelLeaveForm == null)
            {
                return NotFound();
            }

            return personnelLeaveForm;
        }

        // PUT: api/PersonnelLeaveForms/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPersonnelLeaveForm(int id, PersonnelLeaveForm personnelLeaveForm)
        {
            if (id != personnelLeaveForm.LeaveId)
            {
                return BadRequest();
            }

            _context.Entry(personnelLeaveForm).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonnelLeaveFormExists(id))
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

        // POST: api/PersonnelLeaveForms
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PersonnelLeaveForm>> PostPersonnelLeaveForm(PersonnelLeaveForm personnelLeaveForm)
        {
            _context.PersonnelLeaveForms.Add(personnelLeaveForm);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPersonnelLeaveForm", new { id = personnelLeaveForm.LeaveId }, personnelLeaveForm);
        }

        // DELETE: api/PersonnelLeaveForms/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePersonnelLeaveForm(int id)
        {
            var personnelLeaveForm = await _context.PersonnelLeaveForms.FindAsync(id);
            if (personnelLeaveForm == null)
            {
                return NotFound();
            }

            _context.PersonnelLeaveForms.Remove(personnelLeaveForm);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PersonnelLeaveFormExists(int id)
        {
            return _context.PersonnelLeaveForms.Any(e => e.LeaveId == id);
        }
    }
}
