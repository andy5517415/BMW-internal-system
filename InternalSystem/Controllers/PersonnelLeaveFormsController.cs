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

        //用員工ID尋找(Session帶入員工ID) 個人查詢
        // GET: api/PersonnelLeaveForms/employee/5/{y}-{m}
        [HttpGet("employee/{id}/{y}-{m}")]
        public async Task<ActionResult<dynamic>> GetPersonnelLeave(int id, int y, int m)
        {
            var personnelLeaveForm = from pl in _context.PersonnelLeaveForms
                                     join o in _context.PersonnelProfileDetails on pl.EmployeeId equals o.EmployeeId
                                     join l in _context.PersonnelLeaveAuditStatuses on pl.StatusId equals l.StatusId
                                     join lt in _context.PersonnelLeaveTypes on pl.LeaveId equals lt.LeaveTypeId
                                     where o.EmployeeId == id && pl.StartDate.Month == m && pl.StartDate.Year == y
                                     select new
                                     {
                                         EmployeeName = o.EmployeeName,
                                         EmployeeNumber = o.EmployeeNumber,
                                         EmployeeId = pl.EmployeeId,
                                         StartDate = pl.StartDate.ToString("yyyy-MM-dd"),
                                         StartTime = pl.StartTime,
                                         EndDate = pl.EndDate.ToString("yyyy-MM-dd"),
                                         EndTime = pl.EndTime,
                                         LeaveId = pl.LeaveId,
                                         LeaveType = pl.LeaveType,
                                         Type = lt.Type,
                                         StatusId = pl.StatusId,
                                         AuditStatus = l.AuditStatus,
                                         pl.AuditOpnion,
                                         pl.ProxyAudit,
                                         pl.ManagerAudit,
                                         ProxyAuditDate = pl.ProxyAuditDate.ToString(),
                                         ManagerAuditDate =  pl.ManagerAuditDate.ToString(),
                                         Proxy = pl.Proxy,
                                         auditManerger = pl.AuditManerger,
                                         o.PositionId,
                                         Reason = pl.Reason
                                     };

            if (personnelLeaveForm == null)
            {
                return NotFound();
            }

            return await personnelLeaveForm.ToListAsync();
        }

        //用名稱尋找  人事部查詢
        // GET: api/PersonnelLeaveForms/employeeName/5/{y}-{m}
        [HttpGet("employeeName/{name}/{y}-{m}")]
        public async Task<ActionResult<dynamic>> GetNameLeave(string name, int y, int m)
        {
            var personnelLeaveForm = from pl in _context.PersonnelLeaveForms
                                     join o in _context.PersonnelProfileDetails on pl.EmployeeId equals o.EmployeeId
                                     join l in _context.PersonnelLeaveAuditStatuses on pl.StatusId equals l.StatusId
                                     join d in _context.PersonnelDepartmentLists on o.DepartmentId equals d.DepartmentId
                                     where o.EmployeeName == name && pl.StartDate.Month == m && pl.StartDate.Year == y
                                     select new
                                     {
                                         EmployeeName = o.EmployeeName,
                                         EmployeeNumber = o.EmployeeNumber,
                                         EmployeeId = pl.EmployeeId,
                                         DepName = d.DepName,
                                         StartDate = pl.StartDate.ToString("yyyy-MM-dd"),
                                         StartTime = pl.StartTime,
                                         EndDate = pl.EndDate.ToString("yyyy-MM-dd"),
                                         EndTime = pl.EndTime,
                                         LeaveId = pl.LeaveId,
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



        //用部門尋找 人事部查詢
        // GET: api/PersonnelLeaveForms/department/5/{y}-{m}
        [HttpGet("department/{depId}/{y}-{m}")]
        public async Task<ActionResult<dynamic>> GetDepartmentLeave(int depId, int y, int m)
        {
            var personnelLeaveForm = from pl in _context.PersonnelLeaveForms
                                     join o in _context.PersonnelProfileDetails on pl.EmployeeId equals o.EmployeeId
                                     join l in _context.PersonnelLeaveAuditStatuses on pl.StatusId equals l.StatusId
                                     join d in _context.PersonnelDepartmentLists on o.DepartmentId equals d.DepartmentId
                                     where o.DepartmentId == depId && pl.StartDate.Month == m && pl.StartDate.Year == y
                                     select new
                                     {
                                         EmployeeName = o.EmployeeName,
                                         EmployeeNumber = o.EmployeeNumber,
                                         EmployeeId = pl.EmployeeId,
                                         DepName = d.DepName,
                                         StartDate = pl.StartDate.ToString("yyyy-MM-dd"),
                                         StartTime = pl.StartTime,
                                         EndDate = pl.EndDate.ToString("yyyy-MM-dd"),
                                         EndTime = pl.EndTime,
                                         LeaveId = pl.LeaveId,
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

        //GET被指定代理人之申請
        // GET: api/PersonnelLeaveForms/5
        [HttpGet("proxyAudit/{depId}/{position}/{id}")]
        public async Task<ActionResult<dynamic>> ProxyLeaveForm(int depId , int position , int Id)
        {
            var personnelLeaveForm = from pl in _context.PersonnelLeaveForms
                                     join o in _context.PersonnelProfileDetails on pl.EmployeeId equals o.EmployeeId
                                     join l in _context.PersonnelLeaveAuditStatuses on pl.StatusId equals l.StatusId
                                     join d in _context.PersonnelDepartmentLists on o.DepartmentId equals d.DepartmentId
                                     join lt in _context.PersonnelLeaveTypes on pl.LeaveType equals lt.LeaveTypeId
                                     where o.DepartmentId == depId && o.PositionId == position && pl.Proxy == Id 
                                     && pl.StatusId == 1
                                     orderby pl.LeaveId descending
                                     select new
                                     {
                                         EmployeeName = o.EmployeeName,
                                         EmployeeNumber = o.EmployeeNumber,
                                         EmployeeId = pl.EmployeeId,
                                         DepName = d.DepName,
                                         StartDate = pl.StartDate.ToString("yyyy-MM-dd"),
                                         StartTime = pl.StartTime,
                                         EndDate = pl.EndDate.ToString("yyyy-MM-dd"),
                                         EndTime = pl.EndTime,
                                         ProxyAuditDate = pl.ProxyAuditDate.ToString(),
                                         ManagerAuditDate = pl.ManagerAuditDate.ToString(),
                                         LeaveId = pl.LeaveId,
                                         Type = lt.Type,
                                         LeaveType = pl.LeaveType,
                                         pl.TotalTime,
                                         StatusId = pl.StatusId,
                                         pl.ProxyAudit,
                                         pl.ManagerAudit,
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


        //主管拿員工請假申請(代理人已同意)
        // GET: api/PersonnelLeaveForms/5
        [HttpGet("manager/{depId}")]
        public async Task<ActionResult<dynamic>> ManagerLeaveForm(int depId)
        {
            var personnelLeaveForm = from pl in _context.PersonnelLeaveForms
                                     join o in _context.PersonnelProfileDetails on pl.EmployeeId equals o.EmployeeId
                                     join l in _context.PersonnelLeaveAuditStatuses on pl.StatusId equals l.StatusId
                                     join d in _context.PersonnelDepartmentLists on o.DepartmentId equals d.DepartmentId
                                     join lt in _context.PersonnelLeaveTypes on pl.LeaveType equals lt.LeaveTypeId
                                     where o.DepartmentId == depId  && pl.StatusId == 2
                                     orderby pl.LeaveId descending
                                     select new
                                     {
                                         EmployeeName = o.EmployeeName,
                                         EmployeeNumber = o.EmployeeNumber,
                                         EmployeeId = pl.EmployeeId,
                                         DepName = d.DepName,
                                         StartDate = pl.StartDate.ToString("yyyy-MM-dd"),
                                         StartTime = pl.StartTime,
                                         EndDate = pl.EndDate.ToString("yyyy-MM-dd"),
                                         ProxyAuditDate = pl.ProxyAuditDate.ToString(),
                                         ManagerAuditDate = pl.ManagerAuditDate.ToString(),
                                         EndTime = pl.EndTime,
                                         pl.TotalTime,
                                         LeaveId = pl.LeaveId,
                                         Type = lt.Type,
                                         LeaveType = pl.LeaveType,
                                         StatusId = pl.StatusId,
                                         pl.ProxyAudit,
                                         pl.ManagerAudit,
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


        //員工GET請假申請退件(代理人不同意or主管不同意)
        // GET: api/PersonnelLeaveForms/5
        [HttpGet("retrun/{depId}/{eid}")]
        public async Task<ActionResult<dynamic>> LeaveReturnForm(int depId,int eid)
        {
            var personnelLeaveForm = from pl in _context.PersonnelLeaveForms
                                     join o in _context.PersonnelProfileDetails on pl.EmployeeId equals o.EmployeeId
                                     join l in _context.PersonnelLeaveAuditStatuses on pl.StatusId equals l.StatusId
                                     join d in _context.PersonnelDepartmentLists on o.DepartmentId equals d.DepartmentId
                                     join lt in _context.PersonnelLeaveTypes on pl.LeaveType equals lt.LeaveTypeId
                                     where o.DepartmentId == depId && pl.EmployeeId == eid && (pl.StatusId == 3 || pl.StatusId == 5)
                                     select new
                                     {
                                         EmployeeName = o.EmployeeName,
                                         EmployeeNumber = o.EmployeeNumber,
                                         EmployeeId = pl.EmployeeId,
                                         DepName = d.DepName,
                                         StartDate = pl.StartDate.ToString("yyyy-MM-dd"),
                                         StartTime = pl.StartTime,
                                         EndDate = pl.EndDate.ToString("yyyy-MM-dd"),
                                         EndTime = pl.EndTime,
                                         LeaveId = pl.LeaveId,
                                         Type = lt.Type,
                                         LeaveType = pl.LeaveType,
                                         StatusId = pl.StatusId,
                                         pl.ProxyAudit,
                                         pl.ProxyAuditDate,
                                         pl.ManagerAudit,
                                         pl.ManagerAuditDate,
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


        //GET被退件之採購申請單
        // GET: api/PersonnelLeaveForms/5
        [HttpGet("pcappcication/{id}")]
        public async Task<ActionResult<dynamic>> GetPcApplication(int id)
        {
            var personnelLeaveForm = from ap in _context.PcApplications
                                     where ap.EmployeeId == id && ap.AcceptanceStatus == false
                                     select new
                                     {

                                         ap.OrderId,
                                         ap.PurchaseId,
                                         ap.EmployeeId,
                                         ap.Department,
                                         ap.Date,
                                         ap.Comment,
                                         ap.Total,
                                         ap.AcceptanceStatus,
                                         ap.DeliveryStatus,
                                         ap.ApplicationStatus
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
