using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InternalSystem.Models;
using System.Runtime.Intrinsics.Arm;

namespace InternalSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonnelOvertimeFormsController : ControllerBase
    {
        private readonly MSIT44Context _context;

        public PersonnelOvertimeFormsController(MSIT44Context context)
        {
            _context = context;
        }

        // GET: api/PersonnelOvertimeForms
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<PersonnelOvertimeForm>>> GetPersonnelOvertimeForms()
        //{
        //    return await _context.PersonnelOvertimeForms.ToListAsync();
        //}


        //個人加班搜尋(use session)
        // GET: api/PersonnelOvertimeForms/5/mm  
        [HttpGet("{id}/{y}-{m}")]
        public async Task<ActionResult<dynamic>> GetPersonnelOvertime(int id,int y,int m)
        {
            var personnelOvertimeForm = from o in _context.PersonnelOvertimeForms
                                        where o.EmployeeId == id && o.StartDate.Month == m && o.StartDate.Year == y
                                        join p in _context.PersonnelProfileDetails on o.EmployeeId equals p.EmployeeId
                                        join pda in _context.ProductionAreas on o.AreaId equals pda.AreaId
                                        join pdp in _context.ProductionProcesses on o.PropessId equals pdp.ProcessId
                                        select new
                                        {
                                            EmployeeId = o.EmployeeId,
                                            EmployeeName = p.EmployeeName,
                                            EmployeeNumber =p.EmployeeNumber,
                                            StartDate = o.StartDate.ToString("yyyy-MM-dd"),
                                            StartTime = o.StartTime,
                                            EndDate = o.EndDate.ToString("yyyy-MM-dd"),
                                            EndTime = o.EndTime,
                                            TotalTime =o.TotalTime,
                                            AuditStatus =o.AuditStatus,
                                            pda.AreaName,
                                            pdp.ProcessName

                                        };

            if (personnelOvertimeForm == null)
            {
                return NotFound();
            }

            return await personnelOvertimeForm.ToListAsync();
        }


        //用人名尋找加班資料
        // GET: api/PersonnelOvertimeForms/
        [HttpGet("Overtime/{name}/{y}-{m}")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetPersonnelOvertime(string name, int y, int m)
        {

            var personnelOvertimeForm = from ov in _context.PersonnelOvertimeForms
                                        join p in _context.PersonnelProfileDetails on ov.EmployeeId equals p.EmployeeId
                                        join d in _context.PersonnelDepartmentLists on p.DepartmentId equals d.DepartmentId
                                        join pda in _context.ProductionAreas on ov.AreaId equals pda.AreaId
                                        join pdp in _context.ProductionProcesses on ov.PropessId equals pdp.ProcessId
                                        where p.EmployeeName == name && ov.StartDate.Year == y && ov.StartDate.Month == m
                                        select new
                                        {

                                            ov.StartWorkId,
                                            ov.EmployeeId,
                                            EmployeeName = p.EmployeeName,
                                            EmployeeNumber = p.EmployeeNumber,
                                            DepName = d.DepName,
                                            StartDate = ov.StartDate.ToString("yyyy-MM-dd"),
                                            ov.StartTime,
                                            EndDate = ov.EndDate.ToString("yyyy-MM-dd"),
                                            ov.EndTime,
                                            ov.AuditStatus,
                                            ov.AreaId,
                                            ov.PropessId,
                                            pda.AreaName,
                                            pdp.ProcessName,
                                            AppcationDate = ov.AppcationDate.ToString()
                                        };
            if (personnelOvertimeForm == null)
            {
                return NotFound();
            }

            return await personnelOvertimeForm.ToListAsync();
        }



        //用部門尋找加班資料
        // GET: api/PersonnelOvertimeForms 
        [HttpGet("DepOvertime/{dep}/{y}-{m}")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetPersonnelDepOvertime(int dep, int y, int m)
        {

            var overlist = from ov in _context.PersonnelOvertimeForms
                       join p in _context.PersonnelProfileDetails on ov.EmployeeId equals p.EmployeeId
                       join d in _context.PersonnelDepartmentLists on p.DepartmentId equals d.DepartmentId
                       join pda in _context.ProductionAreas on ov.AreaId equals pda.AreaId
                       join pdp in _context.ProductionProcesses on ov.PropessId equals pdp.ProcessId
                       where p.DepartmentId == dep && ov.StartDate.Year == y && ov.StartDate.Month == m
                       select new
                       {
                           ov.StartWorkId,
                           ov.EmployeeId,
                           EmployeeName = p.EmployeeName,
                           EmployeeNumber = p.EmployeeNumber,
                           DepName = d.DepName,
                           StartDate = ov.StartDate.ToString("yyyy-MM-dd"),
                           ov.StartTime,
                           EndDate = ov.EndDate.ToString("yyyy-MM-dd"),
                           ov.EndTime,
                           ov.TotalTime,
                           ov.AuditStatus,
                           ov.AreaId,
                           ov.PropessId,
                           pda.AreaName,
                           pdp.ProcessName,
                           AppcationDate = ov.AppcationDate.ToString()

                       };
            if (overlist == null)
            {
                return NotFound();
            }

            return await overlist.ToListAsync();
        }
        //員工自身ID找尋還未被主管檢閱之資料
        // GET: api/PersonnelOvertimeForms/NotyetAudit/{id}
        [HttpGet("NotyetAudit/{id}")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetOvertime(int id, int y, int m)
        {

            var personnelOvertimeForm = from ov in _context.PersonnelOvertimeForms
                                        join p in _context.PersonnelProfileDetails on ov.EmployeeId equals p.EmployeeId
                                        join d in _context.PersonnelDepartmentLists on p.DepartmentId equals d.DepartmentId
                                        join pda in _context.ProductionAreas on ov.AreaId equals pda.AreaId
                                        join pdp in _context.ProductionProcesses on ov.PropessId equals pdp.ProcessId
                                        where p.EmployeeId==id && ov.AuditStatus==false
                                        select new
                                        {

                                            ov.StartWorkId,
                                            ov.EmployeeId,
                                            EmployeeName = p.EmployeeName,
                                            EmployeeNumber = p.EmployeeNumber,
                                            DepName = d.DepName,
                                            StartDate = ov.StartDate.ToString(),
                                            ov.StartTime,
                                            EndDate = ov.EndDate.ToString(),
                                            ov.EndTime,
                                            ov.TotalTime,
                                            ov.AuditStatus,
                                            ov.AreaId,
                                            ov.PropessId,
                                            pda.AreaName,
                                            pdp.ProcessName,
                                            AppcationDate = ov.AppcationDate.ToString()
                                        };
            if (personnelOvertimeForm == null)
            {
                return NotFound();
            }

            return await personnelOvertimeForm.ToListAsync();
        }


        //主管GET同部門員工加班資料
        // GET: api/PersonnelOvertimeForms/5 
        [HttpGet("manager/over/{dep}")]
        public async Task<ActionResult<dynamic>> ManagerGetOvertimeForm(int dep)
        {
            var overlist = from ov in _context.PersonnelOvertimeForms
                           join p in _context.PersonnelProfileDetails on ov.EmployeeId equals p.EmployeeId
                           join d in _context.PersonnelDepartmentLists on p.DepartmentId equals d.DepartmentId
                           join pda in _context.ProductionAreas on ov.AreaId equals pda.AreaId
                           join pdp in _context.ProductionProcesses on ov.PropessId equals pdp.ProcessId
                           where p.DepartmentId == dep && ov.AuditStatus ==false
                           select new
                           {
                               ov.StartWorkId,
                               ov.EmployeeId,
                               EmployeeName = p.EmployeeName,
                               EmployeeNumber = p.EmployeeNumber,
                               DepName = d.DepName,
                               StartDate = ov.StartDate.ToString("yyyy-MM-dd"),
                               ov.StartTime,
                               EndDate = ov.EndDate.ToString("yyyy-MM-dd"),
                               ov.EndTime,
                               ov.TotalTime,
                               ov.AuditStatus,
                               ov.AreaId,
                               ov.PropessId,
                               pda.AreaName,
                               pdp.ProcessName,
                               AppcationDate = ov.AppcationDate.ToString()
                           };
            if (overlist == null)
            {
                return NotFound();
            }

            return await overlist.ToListAsync();
        }


        // GET: api/PersonnelOvertimeForms/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PersonnelOvertimeForm>> GetPersonnelOvertimeForm(int id)
        {
            var personnelOvertimeForm = await _context.PersonnelOvertimeForms.FindAsync(id);

            if (personnelOvertimeForm == null)
            {
                return NotFound();
            }

            return personnelOvertimeForm;
        }

        // PUT: api/PersonnelOvertimeForms/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPersonnelOvertimeForm(int id, PersonnelOvertimeForm personnelOvertimeForm)
        {
            if (id != personnelOvertimeForm.StartWorkId)
            {
                return BadRequest();
            }

            _context.Entry(personnelOvertimeForm).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonnelOvertimeFormExists(id))
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

        // POST: api/PersonnelOvertimeForms
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PersonnelOvertimeForm>> PostPersonnelOvertimeForm(PersonnelOvertimeForm personnelOvertimeForm)
        {
            _context.PersonnelOvertimeForms.Add(personnelOvertimeForm);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPersonnelOvertimeForm", new { id = personnelOvertimeForm.StartWorkId }, personnelOvertimeForm);
        }

        // DELETE: api/PersonnelOvertimeForms/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePersonnelOvertimeForm(int id)
        {
            var personnelOvertimeForm = await _context.PersonnelOvertimeForms.FindAsync(id);
            if (personnelOvertimeForm == null)
            {
                return NotFound();
            }

            _context.PersonnelOvertimeForms.Remove(personnelOvertimeForm);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PersonnelOvertimeFormExists(int id)
        {
            return _context.PersonnelOvertimeForms.Any(e => e.StartWorkId == id);
        }
    }
}
