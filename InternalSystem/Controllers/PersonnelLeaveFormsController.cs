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
using Microsoft.AspNetCore.Mvc.RazorPages;
using InternalSystem.Dotos;
namespace InternalSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonnelLeaveFormsController : ControllerBase
    {
        private readonly MSIT44Context2 _context;

        public PersonnelLeaveFormsController(MSIT44Context2 context)
        {
            _context = context;
        }

        // GET: api/PersonnelLeaveForms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonnelLeaveForm>>> GetPersonnelLeaveForms()
        {
            return await _context.PersonnelLeaveForms.ToListAsync();
        }

        //用員工ID尋找(Session帶入員工ID) 個人查詢 請假頁面
        // GET: api/PersonnelLeaveForms/profile/5/{y}-{m}
        [HttpGet("profile/{id}/{y}-{m}/{page}")]
        public async Task<ActionResult<dynamic>> GetPersonnelLeave(int id ,int y , int m,int page)
        {
            
             var personnelLeaveForm = from pl in _context.PersonnelLeaveForms
                                     join o in _context.PersonnelProfileDetails on pl.EmployeeId equals o.EmployeeId
                                     join l in _context.PersonnelLeaveAuditStatuses on pl.StatusId equals l.StatusId
                                     join lt in _context.PersonnelLeaveTypes on pl.LeaveType equals lt.LeaveTypeId
                                     where o.EmployeeId == id && pl.StartDate.Year == y && pl.StartDate.Month == m
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
                                         StatusId = pl.StatusId,
                                         lt.Type,
                                         AuditStatus = l.AuditStatus,
                                         ProxyAuditDate =  pl.ProxyAuditDate.ToString(),
                                         pl.ProxyAudit,
                                         pl.ManagerAudit,
                                         ManagerAuditDate = pl.ManagerAuditDate.ToString(),
                                         Proxy = pl.Proxy,
                                         auditManerger = pl.AuditManerger,
                                         Reason = pl.Reason,
                                         pl.ApplicationDate,
                                         pl.AuditOpnion
                                     };

            if (personnelLeaveForm == null)
            {
                return NotFound();
            }
            var query = personnelLeaveForm.Skip((page - 1) * 15).Take(15);

            return await query.ToListAsync();
        }

        //用員工ID尋找(Session帶入員工ID) 個人查詢 請假 頁面
        // GET: api/PersonnelLeaveForms/profile/5/{y}-{m}
        [HttpGet("profilePage/{id}/{y}-{m}")]
        public int GetPersonnelLeavePage(int id, int y, int m,int page)
        {

            var personnelLeaveForm = from pl in _context.PersonnelLeaveForms
                                     join o in _context.PersonnelProfileDetails on pl.EmployeeId equals o.EmployeeId
                                     join l in _context.PersonnelLeaveAuditStatuses on pl.StatusId equals l.StatusId
                                     join lt in _context.PersonnelLeaveTypes on pl.LeaveType equals lt.LeaveTypeId
                                     where o.EmployeeId == id && pl.StartDate.Year == y && pl.StartDate.Month == m
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
                                         StatusId = pl.StatusId,
                                         lt.Type,
                                         AuditStatus = l.AuditStatus,
                                         ProxyAuditDate = pl.ProxyAuditDate.ToString(),
                                         pl.ProxyAudit,
                                         pl.ManagerAudit,
                                         ManagerAuditDate = pl.ManagerAuditDate.ToString(),
                                         Proxy = pl.Proxy,
                                         auditManerger = pl.AuditManerger,
                                         Reason = pl.Reason,
                                         pl.ApplicationDate,
                                         pl.AuditOpnion
                                     };

            if (personnelLeaveForm == null)
            {
                return 0;
            }
            var query = personnelLeaveForm.Skip((page - 1) * 15).Take(15);

            var total = Convert.ToInt16(Math.Ceiling(Convert.ToDouble(personnelLeaveForm.Count()) / 15));
            return total;
        }
        //用名稱尋找  人事部查詢
        // GET: api/PersonnelLeaveForms/employeeName/5/{y}-{m}
        //[HttpGet("employeeName/{name}/{y}-{m}")]
        //public async Task<ActionResult<dynamic>> GetNameLeave(string name, int y, int m)
        //{
        //    var personnelLeaveForm = from pl in _context.PersonnelLeaveForms
        //                             join o in _context.PersonnelProfileDetails on pl.EmployeeId equals o.EmployeeId
        //                             join l in _context.PersonnelLeaveAuditStatuses on pl.StatusId equals l.StatusId
        //                             join d in _context.PersonnelDepartmentLists on o.DepartmentId equals d.DepartmentId
        //                             join lt in _context.PersonnelLeaveTypes on pl.LeaveType equals lt.LeaveTypeId
        //                             where o.EmployeeName == name && pl.StartDate.Month == m && pl.StartDate.Year == y
        //                             select new
        //                             {
        //                                 EmployeeName = o.EmployeeName,
        //                                 EmployeeNumber = o.EmployeeNumber,
        //                                 EmployeeId = pl.EmployeeId,
        //                                 DepName = d.DepName,
        //                                 StartDate = pl.StartDate.ToString("yyyy-MM-dd"),
        //                                 StartTime = pl.StartTime,
        //                                 EndDate = pl.EndDate.ToString("yyyy-MM-dd"),
        //                                 EndTime = pl.EndTime,
        //                                 pl.TotalTime,
        //                                 lt.Type,
        //                                 LeaveId = pl.LeaveId,
        //                                 LeaveType = pl.LeaveType,
        //                                 StatusId = pl.StatusId,
        //                                 AuditStatus = l.AuditStatus,
        //                                 Proxy = pl.Proxy,
        //                                 auditManerger = pl.AuditManerger,
        //                                 Reason = pl.Reason,
        //                                 pl.ApplicationDate
        //                             };

        //    if (personnelLeaveForm == null)
        //    {
        //        return NotFound();
        //    }

        //    return await personnelLeaveForm.ToListAsync();
        //}



        //用部門尋找 人事部查詢
        // GET: api/PersonnelLeaveForms/department/5/{y}-{m}
        //[HttpGet("department/{depId}/{y}-{m}")]
        //public async Task<ActionResult<dynamic>> GetDepartmentLeave(int depId, int y, int m)
        //{

        //    var personnelLeaveForm = from pl in _context.PersonnelLeaveForms
        //                             join o in _context.PersonnelProfileDetails on pl.EmployeeId equals o.EmployeeId
        //                             join l in _context.PersonnelLeaveAuditStatuses on pl.StatusId equals l.StatusId
        //                             join d in _context.PersonnelDepartmentLists on o.DepartmentId equals d.DepartmentId
        //                             join lt in _context.PersonnelLeaveTypes on pl.LeaveType equals lt.LeaveTypeId
        //                             where o.DepartmentId == depId && pl.StartDate.Month == m && pl.StartDate.Year == y
        //                             select new
        //                             {
        //                                 EmployeeName = o.EmployeeName,
        //                                 EmployeeNumber = o.EmployeeNumber,
        //                                 EmployeeId = pl.EmployeeId,
        //                                 DepName = d.DepName,
        //                                 StartDate = pl.StartDate.ToString("yyyy-MM-dd"),
        //                                 StartTime = pl.StartTime,
        //                                 EndDate = pl.EndDate.ToString("yyyy-MM-dd"),
        //                                 EndTime = pl.EndTime,
        //                                 pl.TotalTime,
        //                                 LeaveId = pl.LeaveId,
        //                                 lt.Type,
        //                                 LeaveType = pl.LeaveType,
        //                                 StatusId = pl.StatusId,
        //                                 AuditStatus = l.AuditStatus,
        //                                 Proxy = pl.Proxy,
        //                                 auditManerger = pl.AuditManerger,
        //                                 Reason = pl.Reason,
        //                                 pl.ApplicationDate


        //                             };

        //    if (personnelLeaveForm == null)
        //    {
        //        return NotFound();
        //    }

        //    return await personnelLeaveForm.ToListAsync();
        //}

        
        //請假複合查詢(部門  員工名稱) 人事部查詢
        // GET: api/PersonnelLeaveForms/Complex/1/2/{y}-{m}
        [HttpGet("Complex/{y}-{m}")]
        public async Task<ActionResult<dynamic>> GetLeaveComplex(string name, int? depId, int y, int m, int page)
        {
            var personnelLeaveForm = from pl in _context.PersonnelLeaveForms
                                     join o in _context.PersonnelProfileDetails on pl.EmployeeId equals o.EmployeeId
                                     join l in _context.PersonnelLeaveAuditStatuses on pl.StatusId equals l.StatusId
                                     join lt in _context.PersonnelLeaveTypes on pl.LeaveType equals lt.LeaveTypeId
                                     join d in _context.PersonnelDepartmentLists on o.DepartmentId equals d.DepartmentId
                                     where pl.StartDate.Month == m && pl.StartDate.Year == y
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
                                         lt.Type,
                                         LeaveType = pl.LeaveType,
                                         StatusId = pl.StatusId,
                                         AuditStatus = l.AuditStatus,
                                         Proxy = pl.Proxy,
                                         auditManerger = pl.AuditManerger,
                                         Reason = pl.Reason,
                                         DepartmentId =  o.DepartmentId,
                                         pl.ApplicationDate
                                     };

            if (personnelLeaveForm == null)
            {
                return NotFound();
            }
            if (!string.IsNullOrWhiteSpace(name))
            { personnelLeaveForm = personnelLeaveForm.Where(a => a.EmployeeName.Contains(name)); }
            if (depId!=null)
            { personnelLeaveForm = personnelLeaveForm.Where(a => a.DepartmentId == depId); }

            var query = personnelLeaveForm.Skip((page - 1) * 15).Take(15);
            return await query.ToListAsync();
        }


        //人事查詢請假
        //複合查詢頁面
        // GET: api/PersonnelLeaveForms/Complex/1/2/{y}-{m}
        [HttpGet("ComplexPage/{y}-{m}")]
        public int  GetLeaveComplexPage(string name, int? depId, int y, int m,int page)
        {
            var personnelLeaveForm = from pl in _context.PersonnelLeaveForms
                                     join o in _context.PersonnelProfileDetails on pl.EmployeeId equals o.EmployeeId
                                     join l in _context.PersonnelLeaveAuditStatuses on pl.StatusId equals l.StatusId
                                     join lt in _context.PersonnelLeaveTypes on pl.LeaveType equals lt.LeaveTypeId
                                     join d in _context.PersonnelDepartmentLists on o.DepartmentId equals d.DepartmentId
                                     where pl.StartDate.Month == m && pl.StartDate.Year == y
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
                                         lt.Type,
                                         LeaveType = pl.LeaveType,
                                         StatusId = pl.StatusId,
                                         AuditStatus = l.AuditStatus,
                                         Proxy = pl.Proxy,
                                         auditManerger = pl.AuditManerger,
                                         Reason = pl.Reason,
                                         DepartmentId = o.DepartmentId,
                                         pl.ApplicationDate
                                     };

            if (personnelLeaveForm == null)
            {
                return 0;
            }
            if (!string.IsNullOrWhiteSpace(name))
            { personnelLeaveForm = personnelLeaveForm.Where(a => a.EmployeeName.Contains(name)); }
            if (depId != null)
            { personnelLeaveForm = personnelLeaveForm.Where(a => a.DepartmentId == depId); }
            var query = personnelLeaveForm.Skip((page - 1) * 15).Take(15);

            var total = Convert.ToInt16(Math.Ceiling(Convert.ToDouble(personnelLeaveForm.Count() )/ 15));
            return total;
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
                                     where o.DepartmentId == depId && o.PositionId <= position && o.PositionId<7 && pl.Proxy == Id
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
                                         pl.Photo,
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
                                         pl.Photo,
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
                                         pl.Photo,
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
                                     where ap.EmployeeId == id && ap.ApplicationRejectStatus == true

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
                                         ap.RejectReason

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
            var List = from AP in _context.PcApplications
                       join PD in _context.PersonnelProfileDetails on AP.EmployeeId equals PD.EmployeeId
                       join PDL in _context.PersonnelDepartmentLists on PD.DepartmentId equals PDL.DepartmentId
                       join OD in _context.PcOrderDetails on AP.OrderId equals OD.OrderId
                       join GL in _context.PcGoodLists on OD.ProductId equals GL.ProductId
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
        public ActionResult PutLeaveProxyForm([FromBody]PersonnelLeaveForm personnelLeaveForm)
        {
            var proxydate = DateTime.Now;
            var update = (from a in _context.PersonnelLeaveForms
                          where a.LeaveId== personnelLeaveForm.LeaveId
                          select a).SingleOrDefault();

            if(update != null && update.StatusId ==1)
            {
                update.StatusId = 2;
                update.ProxyAudit = true;
                update.ProxyAuditDate = proxydate;
                _context.SaveChanges();
                return Content("已送出同意");
            }
            else
            {
                return Content("假表狀態異常，請聯絡人事修改重新送出");
            }

        }

        //代理人拒絕
        // PUT: api/PersonnelLeaveForms/proxy/refuse/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("proxy/refuse/{id}")]
        public ActionResult PutLeaveProxyRefuseForm([FromBody] PersonnelLeaveForm personnelLeaveForm)
        {
            var proxydate = DateTime.Now;
            var update = (from a in _context.PersonnelLeaveForms
                          where a.LeaveId == personnelLeaveForm.LeaveId
                          select a).SingleOrDefault();

            if (update != null && update.StatusId == 1)
            {
                update.StatusId = 3;
                update.ProxyAudit = false;
                update.ProxyAuditDate = proxydate;
                _context.SaveChanges();
                return Content("已送出不同意");
            }
            else
            {
                return Content("假表狀態異常，請聯絡人事修改重新送出");
            }
        }

        //主管同意
        // PUT: api/PersonnelLeaveForms/manager/5
        [HttpPut("manager/auditleave")]
        public ActionResult PutLeaveManagerForm([FromBody] PersonnelLeaveForm personnelLeaveForm)
        {
            var leave = (from lo in _context.PersonnelLeaveOvers
                         where lo.EmployeeId == personnelLeaveForm.EmployeeId && lo.LeaveType == personnelLeaveForm.LeaveType
                         select lo
                         ).FirstOrDefault();

            var managerdate = DateTime.Now;

            var update = (from a in _context.PersonnelLeaveForms
                          where a.LeaveId == personnelLeaveForm.LeaveId
                          select a).SingleOrDefault();
            if (personnelLeaveForm.LeaveType <= 4) { 
            
                if (update != null && leave != null && update.StatusId == 2)
            {
                update.StatusId = 4;
                update.ManagerAudit = true;
                update.ManagerAuditDate = managerdate;
                update.AuditOpnion = personnelLeaveForm.AuditOpnion;
                leave.LeaveOver = (double)(leave.LeaveOver - update.TotalTime);
                leave.Used= (double)(leave.Used + update.TotalTime);
                _context.SaveChanges();
                return Content("已送出同意");
            }
            else
            {
                return Content("假表狀態異常，請聯絡人事修改重新送出");
            }
            }
            else
            {
                if (update != null &&  update.StatusId == 2)
                {
                    update.StatusId = 4;
                    update.ManagerAudit = true;
                    update.ManagerAuditDate = managerdate;
                    update.AuditOpnion = personnelLeaveForm.AuditOpnion;
                    _context.SaveChanges();
                    return Content("已送出同意");
                }
                else
                {
                    return Content("假表狀態異常，請聯絡人事修改重新送出");
                }
            }
        }

        //主管不同意
        // PUT: api/PersonnelLeaveForms/manager/refuse/5
        [HttpPut("manager/Refuse/{id}")]
        public ActionResult PutLeaveManagerRefuseForm([FromBody] PersonnelLeaveForm personnelLeaveForm)
        {
            var managerdate = DateTime.Now;

            var update = (from a in _context.PersonnelLeaveForms
                          where a.LeaveId == personnelLeaveForm.LeaveId
                          select a).SingleOrDefault();

            if (update != null && update.StatusId == 2)
            {
                update.StatusId = 5;
                update.ManagerAudit = false;
                update.ManagerAuditDate = managerdate;
                update.AuditOpnion = personnelLeaveForm.AuditOpnion;
                _context.SaveChanges();
                return Content("已送出不同意");
            }
            else
            {
                return Content("假表狀態異常，請聯絡人事修改重新送出");
            }
        }

        //申請人更改後申請
        // PUT: api/PersonnelLeaveForms/proxy/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("Put/{id}")]
        public ActionResult PutLeaveApplicationForm([FromBody] PersonnelLeaveForm personnelLeaveForm)
        {
            var application = DateTime.Now.ToString("yyyy-MM-dd");
            var proxydate = DateTime.Now;
            var update = (from pl in _context.PersonnelLeaveForms
                          where pl.LeaveId == personnelLeaveForm.LeaveId
                          select pl).SingleOrDefault();


            //判斷該員工是否有剩餘假可使用
            var leave = (from lo in _context.PersonnelLeaveOvers
                         where lo.EmployeeId == personnelLeaveForm.EmployeeId && lo.LeaveType == personnelLeaveForm.LeaveType
                         select lo
                         ).FirstOrDefault();
            //時間全為轉分鐘
            var sh = Convert.ToInt32(personnelLeaveForm.StartTime.Substring(0, 2)) * 60;
            var sm = Convert.ToInt32(personnelLeaveForm.StartTime.Substring(3, 2));
            var eh = Convert.ToInt32(personnelLeaveForm.EndTime.Substring(0, 2)) * 60;
            var em = Convert.ToInt32(personnelLeaveForm.EndTime.Substring(3, 2));
            //日期差異判斷
            TimeSpan ts = personnelLeaveForm.EndDate.Subtract(personnelLeaveForm.StartDate);
            TimeSpan hm = Convert.ToDateTime(personnelLeaveForm.EndTime).Subtract(Convert.ToDateTime(personnelLeaveForm.StartTime));
            DayOfWeek sd = personnelLeaveForm.StartDate.DayOfWeek;
            DayOfWeek ed = personnelLeaveForm.EndDate.DayOfWeek;
            double dayCount = ts.TotalDays;
            double hourCount = ts.TotalHours;
            double dayTohour = dayCount * 8;
            double hoursCount = hm.TotalMinutes;

            //判斷請假區間是否有經過假日
            var d = 0;
            DateTime a = personnelLeaveForm.StartDate;
            DayOfWeek dayOfWeek;
            for (int i = 1; i <= dayCount; i++)
            {
                dayOfWeek = a.DayOfWeek;
                if (dayOfWeek == DayOfWeek.Sunday || dayOfWeek == DayOfWeek.Saturday) { d++; }
                a = a.AddDays(1);

            }


            if (dayTohour > 0 || (dayTohour == 0 && hoursCount > 0))
            {
                if (dayTohour > 0 && sh < 720 && (eh == 780 && em != 0) || sh < 720 && eh > 780)
                {
                    personnelLeaveForm.TotalTime = ((((eh + em) - (sh + sm)) / 60) - 1) + dayTohour -(d * 8);
                }
                else if (dayTohour > 0 && sh > eh && ((sh < 720 && eh <= 720 || eh == 780 && em != 0) || eh > 780))
                {
                    personnelLeaveForm.TotalTime = ((((eh + em) - (sh + sm)) / 60)) + dayTohour - (d * 8);
                }
                else if (dayTohour > 0 && sh > eh && eh <= 720)
                {
                    personnelLeaveForm.TotalTime = ((((eh + em) - (sh + sm)) / 60) + 1) + dayTohour - (d * 8);
                }
                //判斷請假是否跨休息時間
                else if (dayTohour == 0 && sh < 720 && (eh == 780 && em != 0) || sh < 720 && eh > 780)
                {
                    personnelLeaveForm.TotalTime = ((((eh + em) - (sh + sm)) / 60) - 1) + dayTohour;
                }
                else
                {
                    personnelLeaveForm.TotalTime = ((((eh + em) - (sh + sm)) / 60)) + dayTohour;
                }
            }
            else
            {
                return Content("請假時段錯誤!");
            }

            if (personnelLeaveForm.LeaveType <= 4)
            {

                if (leave.LeaveOver >= personnelLeaveForm.TotalTime && sd != DayOfWeek.Saturday &&
                    sd != DayOfWeek.Sunday && ed != DayOfWeek.Saturday && ed != DayOfWeek.Sunday)
                {
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
                        update.Photo = personnelLeaveForm.Photo;
                        update.ManagerAuditDate = null;
                        update.ManagerAudit = null;
                        update.ProxyAudit = null;
                        update.ProxyAuditDate = null;
                        _context.SaveChanges();
                    }
                    return Content("已重新提交申請");
                }
                else if (sd == DayOfWeek.Saturday || sd == DayOfWeek.Sunday ||
                   ed == DayOfWeek.Saturday || ed == DayOfWeek.Sunday)
                {
                    return Content("請假開始時間或結束時間不可選於假日");
                }
                else
                {
                    return Content("請假時數不足");
                }

            }
            else {
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
                    update.ManagerAuditDate = null;
                    update.ManagerAudit = null;
                    update.ProxyAudit = null;
                    update.ProxyAuditDate = null;
                    _context.SaveChanges();
                }
                return Content("已重新提交申請");
            }



        }
        
        //部門更改員工請假申請資料
        // PUT: api/PersonnelLeaveForms/proxy/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("PutDep/{id}")]
        public ActionResult DepPutLeaveApplicationForm([FromBody] PersonnelLeaveForm personnelLeaveForm)
        {
            //判斷該員工是否有剩餘假可使用
            var leave = (from lo in _context.PersonnelLeaveOvers
                         where lo.EmployeeId == personnelLeaveForm.EmployeeId && lo.LeaveType == personnelLeaveForm.LeaveType
                         select lo
                         ).FirstOrDefault();

            var update = (from pl in _context.PersonnelLeaveForms
                          where pl.LeaveId == personnelLeaveForm.LeaveId
                          select pl).SingleOrDefault();

        
            //時間全為轉分鐘
            var sh = Convert.ToInt32(personnelLeaveForm.StartTime.Substring(0, 2)) * 60;
            var sm = Convert.ToInt32(personnelLeaveForm.StartTime.Substring(3, 2));
            var eh = Convert.ToInt32(personnelLeaveForm.EndTime.Substring(0, 2)) * 60;
            var em = Convert.ToInt32(personnelLeaveForm.EndTime.Substring(3, 2));
            //日期差異判斷
            TimeSpan ts = personnelLeaveForm.EndDate.Subtract(personnelLeaveForm.StartDate);
            TimeSpan hm = Convert.ToDateTime(personnelLeaveForm.EndTime).Subtract(Convert.ToDateTime(personnelLeaveForm.StartTime));
            double dayCount = ts.TotalDays;
            double hourCount = ts.TotalHours;
            double dayTohour = dayCount * 8;
            double hoursCount = hm.TotalMinutes;

            //判斷請假區間是否有經過假日
            var d = 0;
            DateTime a = personnelLeaveForm.StartDate;
            DayOfWeek dayOfWeek;
            for (int i = 1; i <= dayCount; i++)
            {
                dayOfWeek = a.DayOfWeek;
                if (dayOfWeek == DayOfWeek.Sunday || dayOfWeek == DayOfWeek.Saturday) { d++; }
                a = a.AddDays(1);

            }

            var application = DateTime.Now.ToString("yyyy-MM-dd");
            
            if (dayTohour > 0 || (dayTohour == 0 && hoursCount > 0))
            {
                if (dayTohour > 0 && sh < 720 && (eh == 780 && em != 0) || sh < 720 && eh > 780)
                {
                    personnelLeaveForm.TotalTime = ((((eh + em) - (sh + sm)) / 60) - 1) + dayTohour - (d * 8);
                }
                else if (dayTohour > 0 && sh > eh && ((sh < 720 && eh <= 720 || eh == 780 && em != 0) || eh > 780))
                {
                    personnelLeaveForm.TotalTime = ((((eh + em) - (sh + sm)) / 60)) + dayTohour - (d * 8);
                }
                else if (dayTohour > 0 && sh > eh && eh <= 720)
                {
                    personnelLeaveForm.TotalTime = ((((eh + em) - (sh + sm)) / 60) + 1) + dayTohour - (d * 8);
                }
                //判斷請假是否跨休息時間
                else if (dayTohour == 0 && sh < 720 && (eh == 780 && em != 0) || sh < 720 && eh > 780)
                {
                    personnelLeaveForm.TotalTime = ((((eh + em) - (sh + sm)) / 60) - 1) + dayTohour;
                }
                else
                {
                    personnelLeaveForm.TotalTime = ((((eh + em) - (sh + sm)) / 60)) + dayTohour;
                }
            }
            else
            {
                return Content("請假時段錯誤!");
            }



            if (update.StatusId < 4 )
            {
                if (personnelLeaveForm.LeaveType <= 4)
                {

                    if (leave.LeaveOver >= personnelLeaveForm.TotalTime)
                    {
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
                        return Content("修改成功");
                    }
                    else
                    {
                        return Content("剩餘時數不足");
                    }
                }
                else
                {
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
                        return Content("修改成功");
                    }
                     else
                    {
                        return Content("伺服器錯誤");
                    }
                }
            }
            else
            {
                return Content("主管已審核同意之請假無法進行更改!");
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
        public  ActionResult PostPersonnalLeaveForm([FromBody]PersonnelLeaveForm personnelLeaveForm) 
        {

            //判斷該員工是否有剩餘假可使用
            var leave = (from lo in _context.PersonnelLeaveOvers
                         where lo.EmployeeId == personnelLeaveForm.EmployeeId && lo.LeaveType == personnelLeaveForm.LeaveType
                         select lo
                         ).FirstOrDefault();
            //時間全為轉分鐘
            var sh =  Convert.ToInt32(personnelLeaveForm.StartTime.Substring(0, 2)) * 60;
            var sm =  Convert.ToInt32(personnelLeaveForm.StartTime.Substring(3, 2));
            var eh =  Convert.ToInt32(personnelLeaveForm.EndTime.Substring(0, 2)) * 60;
            var em = Convert.ToInt32(personnelLeaveForm.EndTime.Substring(3, 2));
            //日期差異判斷
            TimeSpan ts = personnelLeaveForm.EndDate.Subtract(personnelLeaveForm.StartDate);
            TimeSpan hm = Convert.ToDateTime(personnelLeaveForm.EndTime).Subtract(Convert.ToDateTime(personnelLeaveForm.StartTime));
            DayOfWeek sd = personnelLeaveForm.StartDate.DayOfWeek;
            DayOfWeek ed = personnelLeaveForm.EndDate.DayOfWeek;
            double dayCount = ts.TotalDays;
            double hourCount = ts.TotalHours;
            double dayTohour = dayCount * 8;
            double hoursCount = hm.TotalMinutes;


            var d = 0;
            DateTime a = personnelLeaveForm.StartDate;
            DayOfWeek dayOfWeek ;
            for (int i = 1; i <= dayCount; i++)
            {
                dayOfWeek = a.DayOfWeek;
                if (dayOfWeek == DayOfWeek.Sunday || dayOfWeek == DayOfWeek.Saturday) { d++; }
                a = a.AddDays(1);

            }

            var application = DateTime.Now.ToString("yyyy-MM-dd");

            if (dayTohour > 0 || (dayTohour == 0 && hoursCount >0))
            {
                if (dayTohour > 0 && sh < 720 && (eh == 780 && em != 0) || sh < 720 && eh > 780)
                {
                    personnelLeaveForm.TotalTime = ((((eh + em) - (sh + sm)) / 60) - 1) + dayTohour - (d*8);
                }
                else if (dayTohour > 0 && sh > eh && ((sh < 720 && eh <= 720 || eh == 780 && em != 0) || eh > 780))
                {
                    personnelLeaveForm.TotalTime = ((((eh + em) - (sh + sm)) / 60)) + dayTohour - (d * 8);
                }
                else if (dayTohour > 0 && sh > eh && eh <= 720)
                {
                    personnelLeaveForm.TotalTime = ((((eh + em) - (sh + sm)) / 60) + 1) + dayTohour - (d * 8);
                }
                //判斷請假是否跨休息時間
                else if (dayTohour == 0 && sh < 720 && (eh == 780 && em != 0) || sh < 720 && eh > 780)
                {
                    personnelLeaveForm.TotalTime = ((((eh + em) - (sh + sm)) / 60) - 1) + dayTohour;
                }
                else
                {
                    personnelLeaveForm.TotalTime = ((((eh + em) - (sh + sm)) / 60)) + dayTohour;
                }
            }
            else
            {
                return Content("請假時段錯誤!");
            }



            if (personnelLeaveForm.LeaveType <= 4)
            {
                if (leave.LeaveOver >= personnelLeaveForm.TotalTime && sd != DayOfWeek.Saturday && 
                    sd != DayOfWeek.Sunday && ed != DayOfWeek.Saturday && ed != DayOfWeek.Sunday)
                {
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
                        Photo = personnelLeaveForm.Photo
                    };
                    _context.PersonnelLeaveForms.Add(insert);
                    _context.SaveChanges();
                    return Content("申請成功");
                }
                else if(sd == DayOfWeek.Saturday || sd == DayOfWeek.Sunday || 
                    ed == DayOfWeek.Saturday || ed== DayOfWeek.Sunday)
                { 
                    return Content("請假開始時間或結束時間不可選於假日");
                }
                else
                {
                    return Content("剩餘時數不足");
                }
            }
            else
            {
                if (sd != DayOfWeek.Saturday &&
                    sd != DayOfWeek.Sunday && ed != DayOfWeek.Saturday && ed != DayOfWeek.Sunday)
                {
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
                        Photo = personnelLeaveForm.Photo


                    };
                    _context.PersonnelLeaveForms.Add(insert);
                    _context.SaveChanges();
                    return Content("申請成功");
                }

                else
                {
                    return Content("請假開始時間或結束時間不可選於假日");
                }
            }


        }

        //主管申請請假
        // POST: api/PersonnelLeaveForms/manager
        [HttpPost("manager")]
        public ActionResult PostManagerLeaveForm([FromBody] PersonnelLeaveForm personnelLeaveForm)
        {

            //判斷該員工是否有剩餘假可使用
            var leave = (from lo in _context.PersonnelLeaveOvers
                         where lo.EmployeeId == personnelLeaveForm.EmployeeId && lo.LeaveType == personnelLeaveForm.LeaveType
                         select lo
                         ).FirstOrDefault();
            var application = DateTime.Now.ToString("yyyy-MM-dd");

            //時間轉為分鐘
          
            var sh = Convert.ToInt32(personnelLeaveForm.StartTime.Substring(0, 2)) * 60;
            var sm = Convert.ToInt32(personnelLeaveForm.StartTime.Substring(3, 2));
            var eh = Convert.ToInt32(personnelLeaveForm.EndTime.Substring(0, 2)) * 60;
            var em = Convert.ToInt32(personnelLeaveForm.EndTime.Substring(3, 2));
            //日期差距判斷
            TimeSpan ts = personnelLeaveForm.EndDate.Subtract(personnelLeaveForm.StartDate);
            TimeSpan hm = Convert.ToDateTime(personnelLeaveForm.EndTime).Subtract(Convert.ToDateTime(personnelLeaveForm.StartTime));
            DayOfWeek sd = personnelLeaveForm.StartDate.DayOfWeek;
            DayOfWeek ed = personnelLeaveForm.EndDate.DayOfWeek;
            double dayCount = ts.TotalDays;
            double hourCount = ts.TotalHours;
            double dayTohour = dayCount * 8;
            double hoursCount = hm.TotalMinutes;

            //判斷請假區間是否有經過假日
            var d = 0;
            DateTime a = personnelLeaveForm.StartDate;
            DayOfWeek dayOfWeek;
            for (int i = 1; i <= dayCount; i++)
            {
                dayOfWeek = a.DayOfWeek;
                if (dayOfWeek == DayOfWeek.Sunday || dayOfWeek == DayOfWeek.Saturday) { d++; }
                a = a.AddDays(1);

            }

            if (dayTohour > 0 || (dayTohour == 0 && hoursCount > 0))
            {
                if (dayTohour > 0 && sh < 720 && (eh == 780 && em != 0) || sh < 720 && eh > 780)
                {
                    personnelLeaveForm.TotalTime = ((((eh + em) - (sh + sm)) / 60) - 1) + dayTohour - (d * 8);
                }
                else if (dayTohour > 0 && sh > eh && ((sh < 720 && eh <= 720 || eh == 780 && em != 0) || eh > 780))
                {
                    personnelLeaveForm.TotalTime = ((((eh + em) - (sh + sm)) / 60)) + dayTohour - (d * 8);
                }
                else if (dayTohour > 0 && sh > eh && eh <= 720)
                {
                    personnelLeaveForm.TotalTime = ((((eh + em) - (sh + sm)) / 60) + 1) + dayTohour - (d * 8);
                }
                //判斷請假是否跨休息時間
                else if (dayTohour == 0 && sh < 720 && (eh == 780 && em != 0) || sh < 720 && eh > 780)
                {
                    personnelLeaveForm.TotalTime = ((((eh + em) - (sh + sm)) / 60) - 1) + dayTohour ;
                }
                else
                {
                    personnelLeaveForm.TotalTime = ((((eh + em) - (sh + sm)) / 60)) + dayTohour;
                }
            }
            else
            {
                return Content("請假時段錯誤!");
            }

            if (personnelLeaveForm.LeaveType <= 4)
            {

                if (leave.LeaveOver >= personnelLeaveForm.TotalTime)
                {
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
                    if (leave != null)
                    {

                        leave.LeaveOver = (double)(leave.LeaveOver - personnelLeaveForm.TotalTime);
                        leave.Used = (double)(leave.Used + personnelLeaveForm.TotalTime);
                        _context.SaveChanges();
                    }
                    return Content("申請成功");
                }
                else
                { return Content("時數不足"); }
            }

            else
            {
                if (sd != DayOfWeek.Saturday &&
                 sd != DayOfWeek.Sunday && ed != DayOfWeek.Saturday && ed != DayOfWeek.Sunday)
                {

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
                    return Content("申請成功");
                }
                else {
                    return Content("請假開始時間或結束時間不可選於假日");
                }
            }
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
        public ActionResult GetRejectOrder(int id)
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
            return Content("已刪除資料");
            }

            else {
                return Content("伺服器出現問題，請稍後再試");
            }
        }

        // DELETE: api/PersonnelLeaveForms/5
        [HttpDelete("{id}")]
        public ActionResult  DeletePersonnelLeaveForm(int id)
        {
            var personnelLeaveForm = (from lf in _context.PersonnelLeaveForms
                                      where lf.LeaveId == id
                                      select lf).FirstOrDefault();
            if (personnelLeaveForm == null)
            {
                return NotFound();
            }

            _context.PersonnelLeaveForms.Remove(personnelLeaveForm);
            _context.SaveChangesAsync();

            return Content("已刪除本次請假申請");

        }

        private bool PersonnelLeaveFormExists(int id)
        {
            return _context.PersonnelLeaveForms.Any(e => e.LeaveId == id);
        }
    }
}
