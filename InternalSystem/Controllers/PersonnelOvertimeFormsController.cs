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


        //個人加班訂單區域搜尋(use session)
        // GET: api/PersonnelOvertimeForms/productProcessList
        [HttpGet("productProcessList")]
        public async Task<ActionResult<dynamic>> GetOverTimeOrder()
        {

            var personnelOvertimeForm = from pl in _context.ProductionProcessLists
                                        join b in _context.BusinessOrders on pl.OrderId equals b.OrderId
                                        join a in _context.ProductionAreas on pl.AreaId equals a.AreaId
                                        join pc in _context.ProductionProcesses on pl.ProcessId equals pc.ProcessId
                                        where pl != null && pl.StatusId != 3
                                        select new {
                                            b.OrderNumber,
                                            pl.OrderId,
                                            pl.AreaId,
                                            pl.ProcessId,
                                            a.AreaName,
                                            pc.ProcessName
                                        };
            if (personnelOvertimeForm == null)
            {
                return Content("目前無未完成之訂單");
            }
            return await personnelOvertimeForm.ToListAsync();

        }


        //個人加班搜尋(use session)
        // GET: api/PersonnelOvertimeForms/5/mm  
        [HttpGet("{id}/{y}-{m}/{page}")]
        public async Task<ActionResult<dynamic>> GetPersonnelOvertime(int id,int y,int m,int page)
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
            var query = personnelOvertimeForm.Skip((page - 1) * 10).Take(10);
            return await query.ToListAsync();
        }


        //個人加班搜尋(use session)頁面
        // GET: api/PersonnelOvertimeForms/5/mm  
        [HttpGet("Page/{id}/{y}-{m}/{page}")]
        public int  GetPersonnelOvertimePage(int id, int y, int m,int page)
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
                                            EmployeeNumber = p.EmployeeNumber,
                                            StartDate = o.StartDate.ToString("yyyy-MM-dd"),
                                            StartTime = o.StartTime,
                                            EndDate = o.EndDate.ToString("yyyy-MM-dd"),
                                            EndTime = o.EndTime,
                                            TotalTime = o.TotalTime,
                                            AuditStatus = o.AuditStatus,
                                            pda.AreaName,
                                            pdp.ProcessName

                                        };

            if (personnelOvertimeForm == null)
            {
                return 0;
            }
            var total = Convert.ToInt16(Math.Ceiling(Convert.ToDouble(personnelOvertimeForm.Count()) / 10));

            return total;
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
                                            ApplicationDate = ov.ApplicationDate.ToString()
                                        };
            if (personnelOvertimeForm == null)
            {
                return NotFound();
            }

            return await personnelOvertimeForm.ToListAsync();
        }



        //用部門尋找加班資料
        // GET: api/PersonnelOvertimeForms/DepOvertime/1/2023-01
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
                           ApplicationDate = ov.ApplicationDate.ToString()

                       };
            if (overlist == null)
            {
                return NotFound();
            }

            return await overlist.ToListAsync();
        }

        //複合查詢尋找加班資料
        // GET: api/PersonnelOvertimeForms/Complex/1/name
        [HttpGet("Complex/{y}-{m}")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetPersonnelComplexOvertime(int? dep, string name , int y, int m,int page)
        {

            var overlist = from ov in _context.PersonnelOvertimeForms
                           join p in _context.PersonnelProfileDetails on ov.EmployeeId equals p.EmployeeId
                           join d in _context.PersonnelDepartmentLists on p.DepartmentId equals d.DepartmentId
                           join pda in _context.ProductionAreas on ov.AreaId equals pda.AreaId
                           join pdp in _context.ProductionProcesses on ov.PropessId equals pdp.ProcessId
                           where ov.StartDate.Year == y && ov.StartDate.Month == m
                           select new
                           {
                               ov.StartWorkId,
                               ov.EmployeeId,
                               EmployeeName = p.EmployeeName,
                               EmployeeNumber = p.EmployeeNumber,
                               DepName = d.DepName,
                               DepartmentId = d.DepartmentId,
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
                               ApplicationDate = ov.ApplicationDate.ToString()

                           };
            if (overlist == null)
            {
                return NotFound();
            }
            if (dep != null) {
                overlist = overlist.Where(a => a.DepartmentId == dep);
            }
            if (!string.IsNullOrWhiteSpace(name))
            {
                overlist = overlist.Where(a => a.EmployeeName.Contains(name));
            }
            var query = overlist.Skip((page - 1) * 10).Take(10);
            return await query.ToListAsync();
        }

        //複合查詢尋找加班資料 頁面
        // GET: api/PersonnelOvertimeForms/ComplexPage/1/name
        [HttpGet("ComplexPage/{y}-{m}")]
        public int GetPersonnelComplexOvertimePage(int? dep, string name, int y, int m,int page)
        {

            var overlist = from ov in _context.PersonnelOvertimeForms
                           join p in _context.PersonnelProfileDetails on ov.EmployeeId equals p.EmployeeId
                           join d in _context.PersonnelDepartmentLists on p.DepartmentId equals d.DepartmentId
                           join pda in _context.ProductionAreas on ov.AreaId equals pda.AreaId
                           join pdp in _context.ProductionProcesses on ov.PropessId equals pdp.ProcessId
                           where ov.StartDate.Year == y && ov.StartDate.Month == m
                           select new
                           {
                               ov.StartWorkId,
                               ov.EmployeeId,
                               EmployeeName = p.EmployeeName,
                               EmployeeNumber = p.EmployeeNumber,
                               DepName = d.DepName,
                               DepartmentId = d.DepartmentId,
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
                               ApplicationDate = ov.ApplicationDate.ToString()

                           };
            if (overlist == null)
            {
                return 0;
            }
            if (dep != null)
            {
                overlist = overlist.Where(a => a.DepartmentId == dep);
            }
            if (!string.IsNullOrWhiteSpace(name))
            {
                overlist = overlist.Where(a => a.EmployeeName.Contains(name));
            }

            var query = overlist.Skip((page - 1) * 10).Take(10);

            var total = Convert.ToInt16(Math.Ceiling(Convert.ToDouble(overlist.Count()) / 10));
            return total;
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
                                            ApplicationDate = ov.ApplicationDate.ToString()
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
                               ApplicationDate = ov.ApplicationDate.ToString()
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

        //主管審閱加班資料
        // PUT: api/PersonnelOvertimeForms/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public ActionResult PutOvertime([FromBody]PersonnelOvertimeForm personnelOvertimeForm)
        {
            var overaudit = DateTime.Now.ToString("yyyy-MM-dd");
            var update = (from o in _context.PersonnelOvertimeForms
                          where o.StartWorkId == personnelOvertimeForm.StartWorkId
                          select o).SingleOrDefault();
            if(update != null)
            {
                update.AuditStatus = true;
                _context.SaveChanges();
            }
            return Content("檢閱完成");
        }


        //人事部門修改資料
        // PUT: api/PersonnelOvertimeForms/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("dep/{id}")]
        public ActionResult  PutPersonnelOvertimeForm(int id,[FromBody]PersonnelOvertimeForm personnelOvertimeForm)
        {

            var update = (from ot in _context.PersonnelOvertimeForms
                          where ot.StartWorkId == personnelOvertimeForm.StartWorkId
                          select ot).FirstOrDefault();
            var application = DateTime.Now.ToString("yyyy-MM-dd");
            //日期差異判斷
            TimeSpan ts = personnelOvertimeForm.EndDate.Subtract(personnelOvertimeForm.StartDate);
            TimeSpan hm = Convert.ToDateTime(personnelOvertimeForm.EndTime).Subtract(Convert.ToDateTime(personnelOvertimeForm.StartTime));
            double dayCount = ts.TotalDays;
            double tshourCount = ts.TotalHours;
            double dayTohour = dayCount * 8;
            double hoursCountMinutes = hm.TotalMinutes;
            double hoursCount = hm.TotalHours;

            if (update.StartWorkId != personnelOvertimeForm.StartWorkId)
            {
                return Content("加班人員資料有誤!");
            }

            if (dayCount == 0 && hoursCountMinutes > 0 && update != null)
            {

                update.ApplicationDate = application;
                update.EmployeeId = personnelOvertimeForm.EmployeeId;
                update.PropessId = personnelOvertimeForm.PropessId;
                update.AreaId = personnelOvertimeForm.AreaId;
                update.StartDate = personnelOvertimeForm.StartDate;
                update.StartTime = personnelOvertimeForm.StartTime;
                update.EndDate = personnelOvertimeForm.EndDate;
                update.EndTime = personnelOvertimeForm.EndTime;
                update.TotalTime = hoursCount;
                ; update.AuditStatus = false;
                _context.SaveChanges();

                return Content("修改成功");
            }
            else if (dayCount > 0 && update != null)
            {
                
                    update.ApplicationDate = application;
                update.EmployeeId = personnelOvertimeForm.EmployeeId;
                update.PropessId = personnelOvertimeForm.PropessId;
                update.AreaId = personnelOvertimeForm.AreaId;
                update.StartDate = personnelOvertimeForm.StartDate;
                update.StartTime = personnelOvertimeForm.StartTime;
                update.EndDate = personnelOvertimeForm.EndDate;
                update.EndTime = personnelOvertimeForm.EndTime;
                update.TotalTime = tshourCount + hoursCount;
                update.AuditStatus = false;

                _context.SaveChanges();
                return Content("修改成功");
            }

            else
            {
                return Content("修改時間有誤");
            }
        }

        //員工加班申請
        // POST: api/PersonnelOvertimeForms
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult PostOverTime([FromBody]PersonnelOvertimeForm personnelOvertimeForm)
        {
            var application = DateTime.Now.ToString("yyyy-MM-dd");
            //日期差異判斷
            TimeSpan ts = personnelOvertimeForm.EndDate.Subtract(personnelOvertimeForm.StartDate);
            TimeSpan hm = Convert.ToDateTime(personnelOvertimeForm.EndTime).Subtract(Convert.ToDateTime(personnelOvertimeForm.StartTime));
            double dayCount = ts.TotalDays;
            double tshourCount = ts.TotalHours;
            double dayTohour = dayCount * 8;
            double hoursCountMinutes = hm.TotalMinutes;
            double hoursCount = hm.TotalHours;

            if (dayCount == 0 && hoursCountMinutes > 0)
            {
                PersonnelOvertimeForm insert = new PersonnelOvertimeForm
                {
                    ApplicationDate = application,
                    EmployeeId = personnelOvertimeForm.EmployeeId,
                    PropessId = personnelOvertimeForm.PropessId,
                    AreaId = personnelOvertimeForm.AreaId,
                    StartDate = personnelOvertimeForm.StartDate,
                    StartTime = personnelOvertimeForm.StartTime,
                    EndDate = personnelOvertimeForm.EndDate,
                    EndTime = personnelOvertimeForm.EndTime,
                    TotalTime = hoursCount,
                    AuditStatus = false
                };
                _context.PersonnelOvertimeForms.Add(insert);
                _context.SaveChanges();
                return Content("申請成功");
            }
            else if(dayCount > 0  )
                {
                PersonnelOvertimeForm insert = new PersonnelOvertimeForm
                {
                    ApplicationDate = application,
                    EmployeeId = personnelOvertimeForm.EmployeeId,
                    PropessId = personnelOvertimeForm.PropessId,
                    AreaId = personnelOvertimeForm.AreaId,
                    StartDate = personnelOvertimeForm.StartDate,
                    StartTime = personnelOvertimeForm.StartTime,
                    EndDate = personnelOvertimeForm.EndDate,
                    EndTime = personnelOvertimeForm.EndTime,
                    TotalTime = tshourCount + hoursCount,
                    AuditStatus = false
                };
                _context.PersonnelOvertimeForms.Add(insert);
                _context.SaveChanges();
                return Content("申請成功");
            }

            else
            {
                return Content("申請時間有誤");
            }
        }


        // POST: api/PersonnelOvertimeForms
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<PersonnelOvertimeForm>> PostPersonnelOvertimeForm(PersonnelOvertimeForm personnelOvertimeForm)
        //{
        //    _context.PersonnelOvertimeForms.Add(personnelOvertimeForm);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetPersonnelOvertimeForm", new { id = personnelOvertimeForm.StartWorkId }, personnelOvertimeForm);
        //}

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
