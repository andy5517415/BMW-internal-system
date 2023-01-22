using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InternalSystem.Models;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;
using Microsoft.CodeAnalysis;
using static System.Net.Mime.MediaTypeNames;

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
                                         pl.TotalTime,
                                         LeaveId = pl.LeaveId,
                                         LeaveType = pl.LeaveType,
                                         Type = lt.Type,
                                         StatusId = pl.StatusId,
                                         AuditStatus = l.AuditStatus,
                                         pl.AuditOpnion,
                                         pl.ProxyAudit,
                                         pl.ManagerAudit,
                                         ProxyAuditDate = pl.ProxyAuditDate.ToString(),
                                         ManagerAuditDate = pl.ManagerAuditDate.ToString(),
                                         Proxy = pl.Proxy,
                                         auditManerger = pl.AuditManerger,
                                         o.PositionId,
                                         Reason = pl.Reason,
                                         pl.ApplicationDate
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
                                         pl.TotalTime,
                                         LeaveId = pl.LeaveId,
                                         LeaveType = pl.LeaveType,
                                         StatusId = pl.StatusId,
                                         AuditStatus = l.AuditStatus,
                                         Proxy = pl.Proxy,
                                         auditManerger = pl.AuditManerger,
                                         Reason = pl.Reason,
                                         pl.ApplicationDate
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
                                         pl.TotalTime,
                                         LeaveId = pl.LeaveId,
                                         LeaveType = pl.LeaveType,
                                         StatusId = pl.StatusId,
                                         AuditStatus = l.AuditStatus,
                                         Proxy = pl.Proxy,
                                         auditManerger = pl.AuditManerger,
                                         Reason = pl.Reason,
                                         pl.ApplicationDate


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
        public async Task<ActionResult<dynamic>> ProxyLeaveForm(int depId, int position, int Id)
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
                                         Reason = pl.Reason,
                                         pl.ApplicationDate
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
                                     where o.DepartmentId == depId && pl.StatusId == 2
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
                                         Reason = pl.Reason,
                                         pl.ApplicationDate

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
        public async Task<ActionResult<dynamic>> LeaveReturnForm(int depId, int eid)
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
                                         ProxyAuditDate = pl.ProxyAuditDate.ToString(),
                                         pl.ManagerAudit,
                                         ManagerAuditDate = pl.ManagerAuditDate.ToString(),
                                         AuditStatus = l.AuditStatus,
                                         Proxy = pl.Proxy,
                                         auditManerger = pl.AuditManerger,
                                         Reason = pl.Reason,
                                         pl.ApplicationDate

                                     };

            if (personnelLeaveForm == null)
            {
                return NotFound();
            }

            return await personnelLeaveForm.ToListAsync();
        }


        //GET被退件之採購申請單
        // GET: api/PersonnelLeaveForms/pcappcication/5
        [HttpGet("pcappcication/{id}")]
        public async Task<ActionResult<dynamic>> GetPcApplication(int id)
        {
            var personnelLeaveForm = from ap in _context.PcApplications
                                     join pd in _context.PersonnelProfileDetails on ap.EmployeeId equals pd.EmployeeId
                                     join pod in _context.PcOrderDetails on ap.OrderId equals pod.OrderId
                                     where ap.EmployeeId == id && ap.ApplicationRejectStatus == false

                                     select new
                                     {

                                         ap.OrderId,
                                         ap.PurchaseId,
                                         ap.EmployeeId,
                                         pd.EmployeeName,
                                         pd.EmployeeNumber,
                                         ap.Department,
                                         Date = ap.Date.ToString(),
                                         ap.Comment,
                                         ap.Total,
                                         ap.AcceptanceStatus,
                                         ap.DeliveryStatus,
                                         ap.ApplicationStatus,
                                         ap.ApplicationRejectStatus,
                                         pod.ProductId
                                     };

            if (personnelLeaveForm == null)
            {
                return NotFound();
            }

            return await personnelLeaveForm.ToListAsync();
        }

        // 物品查詢細項專用
        // 用於 PC_ApplicationRecordDetails
        // GET: api/PersonnelLeaveForms/recordsearchdetail
        [HttpGet("recordsearchdetail/{id}")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetPCrecordsearchdetail(int id)
        {
            var List = from AP in this._context.PcApplications
                       join PD in this._context.PersonnelProfileDetails on AP.EmployeeId equals PD.EmployeeId
                       join PDL in this._context.PersonnelDepartmentLists on PD.DepartmentId equals PDL.DepartmentId
                       join OD in this._context.PcOrderDetails on AP.OrderId equals OD.OrderId
                       join GL in this._context.PcGoodLists on OD.ProductId equals GL.ProductId
                       where AP.PurchaseId == id
                       select new
                       {
                           PurchaseId = AP.PurchaseId,
                           EmployeeName = PD.EmployeeName,
                           Department = PDL.DepName,
                           Date = AP.Date.ToString(),
                           Comment = AP.Comment,
                           Total = AP.Total,
                           Goods = OD.Goods,
                           Unit = OD.Unit,
                           Quantiy = OD.Quantiy,
                           UnitPrice = OD.UnitPrice,
                           Subtotal = OD.Subtotal,
                           ProductId = OD.ProductId
                       };


            return await List.ToListAsync();
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
        public async Task<IActionResult> PutPersonnelLeaveFormForManager(int id, PersonnelLeaveForm personnelLeaveForm)
        {
            if (id != personnelLeaveForm.LeaveId)
            {
                return BadRequest();
            }
            else if (personnelLeaveForm.StatusId != 2)
            {
                return NotFound();
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


        //代理人同意
        // PUT: api/PersonnelLeaveForms/proxy/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("proxy/{id}")]
        public void PutLeaveProxyForm([FromBody]PersonnelLeaveForm personnelLeaveForm)
        {
            var proxydate = DateTime.Now;
            var update = (from a in _context.PersonnelLeaveForms
                          where a.LeaveId== personnelLeaveForm.LeaveId
                          select a).SingleOrDefault();

            if(update != null)
            {
                update.StatusId = 2;
                update.ProxyAudit = true;
                update.ProxyAuditDate = proxydate;
                _context.SaveChanges();
            }

            
        }

        //代理人拒絕
        // PUT: api/PersonnelLeaveForms/proxy/refuse/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("proxy/refuse/{id}")]
        public void PutLeaveProxyRefuseForm([FromBody] PersonnelLeaveForm personnelLeaveForm)
        {
            var proxydate = DateTime.Now;
            var update = (from a in _context.PersonnelLeaveForms
                          where a.LeaveId == personnelLeaveForm.LeaveId
                          select a).SingleOrDefault();

            if (update != null)
            {
                update.StatusId = 3;
                update.ProxyAudit = false;
                update.ProxyAuditDate = proxydate;
                _context.SaveChanges();
            }

        }

        //主管同意
        // PUT: api/PersonnelLeaveForms/manager/5
        [HttpPut("manager/auditleave/{id}")]
        public void PutLeaveManagerForm([FromBody] PersonnelLeaveForm personnelLeaveForm)
        {
            var managerdate = DateTime.Now;

            var update = (from a in _context.PersonnelLeaveForms
                          where a.LeaveId == personnelLeaveForm.LeaveId
                          select a).SingleOrDefault();

            if (update != null)
            {
                update.StatusId = 4;
                update.ManagerAudit = true;
                update.ManagerAuditDate = managerdate;
                update.AuditOpnion = personnelLeaveForm.AuditOpnion;
                _context.SaveChanges();
            }
        }

        //主管不同意
        // PUT: api/PersonnelLeaveForms/manager/refuse/5
        [HttpPut("manager/Refuse/{id}")]
        public void PutLeaveManagerRefuseForm([FromBody] PersonnelLeaveForm personnelLeaveForm)
        {
            var managerdate = DateTime.Now;

            var update = (from a in _context.PersonnelLeaveForms
                          where a.LeaveId == personnelLeaveForm.LeaveId
                          select a).SingleOrDefault();

            if (update != null)
            {
                update.StatusId = 5;
                update.ManagerAudit = false;
                update.ManagerAuditDate = managerdate;
                update.AuditOpnion = personnelLeaveForm.AuditOpnion;
                _context.SaveChanges();
            }
        }

        //申請人更改後申請
        // PUT: api/PersonnelLeaveForms/proxy/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("Put/{id}")]
        public void PutLeaveApplicationForm([FromBody] PersonnelLeaveForm personnelLeaveForm)
        {
            var application = DateTime.Now.ToString("yyyy-MM-dd");
            var proxydate = DateTime.Now;
            var update = (from a in _context.PersonnelLeaveForms
                          where a.LeaveId == personnelLeaveForm.LeaveId
                          select a).SingleOrDefault();

            if (update != null)
            {
                update.ApplicationDate = application;
                update.StatusId = 1;
                update.LeaveType = personnelLeaveForm.LeaveType;
                update.StartDate = personnelLeaveForm.StartDate;
                update.EndDate = personnelLeaveForm.EndDate;
                update.StartTime = personnelLeaveForm.StartTime;
                update.EndTime = personnelLeaveForm.EndTime;
                update.Proxy = personnelLeaveForm.Proxy;
                update.AuditManerger = personnelLeaveForm.AuditManerger;
                update.TotalTime = personnelLeaveForm.TotalTime;
                update.Reason = personnelLeaveForm.Reason;
                _context.SaveChanges();
            }


        }

        //部門更改員工請假申請資料
        // PUT: api/PersonnelLeaveForms/proxy/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("PutDep/{id}")]
        public void DepPutLeaveApplicationForm([FromBody] PersonnelLeaveForm personnelLeaveForm)
        {
            var update = (from a in _context.PersonnelLeaveForms
                          where a.LeaveId == personnelLeaveForm.LeaveId
                          select a).SingleOrDefault();

            if (update != null)
            {
                update.ApplicationDate = personnelLeaveForm.ApplicationDate;
                update.StatusId = 1;
                update.LeaveType = personnelLeaveForm.LeaveType;
                update.StartDate = personnelLeaveForm.StartDate;
                update.EndDate = personnelLeaveForm.EndDate;
                update.StartTime = personnelLeaveForm.StartTime;
                update.EndTime = personnelLeaveForm.EndTime;
                update.Proxy = personnelLeaveForm.Proxy;
                update.AuditManerger = personnelLeaveForm.AuditManerger;
                update.TotalTime = personnelLeaveForm.TotalTime;
                update.ManagerAudit = null;
                update.ProxyAudit = null;
                update.ManagerAuditDate = null;
                update.ProxyAuditDate = null;




                _context.SaveChanges();
            }


        }

        // PUT: api/PersonnelLeaveForms/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutPersonnelLeaveForm(int id, PersonnelLeaveForm personnelLeaveForm)
        //{
        //    if (id != personnelLeaveForm.LeaveId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(personnelLeaveForm).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!PersonnelLeaveFormExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //申請請假
        // POST: api/PersonnelLeaveForms
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public void PostPersonnalLeaveForm([FromBody]PersonnelLeaveForm personnelLeaveForm) 
        {
            var application = DateTime.Now.ToString("yyyy-MM-dd");

            PersonnelLeaveForm insert = new PersonnelLeaveForm
            {
                EmployeeId = personnelLeaveForm.EmployeeId,
                ApplicationDate = application,
                StatusId = 1,
                LeaveType = personnelLeaveForm.LeaveType,
                StartDate = personnelLeaveForm.StartDate,
                EndDate = personnelLeaveForm.EndDate,
                StartTime = personnelLeaveForm.StartTime,
                EndTime = personnelLeaveForm.EndTime,
                Proxy = personnelLeaveForm.Proxy,
                AuditManerger = personnelLeaveForm.AuditManerger,
                TotalTime = personnelLeaveForm.TotalTime,
                Reason = personnelLeaveForm.Reason,

            };
            _context.PersonnelLeaveForms.Add(insert);
            _context.SaveChanges();
        }

        //主管申請請假
        // POST: api/PersonnelLeaveForms/manager
        [HttpPost("manager")]
        public void PostManagerLeaveForm([FromBody] PersonnelLeaveForm personnelLeaveForm)
        {
            var application = DateTime.Now.ToString("yyyy-MM-dd");

            PersonnelLeaveForm insert = new PersonnelLeaveForm
            {
                EmployeeId = personnelLeaveForm.EmployeeId,
                ApplicationDate = application,
                StatusId = 6,
                LeaveType = personnelLeaveForm.LeaveType,
                StartDate = personnelLeaveForm.StartDate,
                EndDate = personnelLeaveForm.EndDate,
                StartTime = personnelLeaveForm.StartTime,
                EndTime = personnelLeaveForm.EndTime,
                TotalTime = personnelLeaveForm.TotalTime,
                Reason = personnelLeaveForm.Reason

            };
            _context.PersonnelLeaveForms.Add(insert);
            _context.SaveChanges();
        }
        //public async Task<ActionResult<PersonnelLeaveForm>> PostPersonnelLeaveForm(PersonnelLeaveForm personnelLeaveForm)
        //{
        //    _context.PersonnelLeaveForms.Add(personnelLeaveForm);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetPersonnelLeaveForm", new { id = personnelLeaveForm.LeaveId }, personnelLeaveForm);
        //}



        //Delete被退件之採購申請單(父子資料同時刪除)
        // Delete: api/PersonnelLeaveForms/buyrej/5
        [HttpDelete("buyrej/{id}")]
        public void GetRejectOrder(int id)
        {
           
            var profile = from pod in _context.PcOrderDetails
                          where pod.OrderId ==id
                          select pod;
          
                _context.PcOrderDetails.RemoveRange(profile);
                _context.SaveChanges();


            var delete = (from a in _context.PcApplications
                          where a.OrderId == id
                          select a).SingleOrDefault();
            if (delete != null)
            {
                _context.PcApplications.Remove(delete);
                _context.SaveChanges();
            }
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
