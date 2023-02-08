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
    public class PersonnelAttendanceTimesController : ControllerBase
    {
        private readonly MSIT44Context _context;

        public PersonnelAttendanceTimesController(MSIT44Context context)
        {
            _context = context;
        }

        // GET: api/PersonnelAttendanceTimes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonnelAttendanceTime>>> GetPersonnelAttendanceTimes()
        {
            return await _context.PersonnelAttendanceTimes.ToListAsync();
        }

        // GET: api/PersonnelAttendanceTimes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PersonnelAttendanceTime>> GetPersonnelAttendanceTime(int id)
        {
            var personnelAttendanceTime = await _context.PersonnelAttendanceTimes.FindAsync(id);

            if (personnelAttendanceTime == null)
            {
                return NotFound();
            }

            return personnelAttendanceTime;
        }

        //個人查詢
        // GET: api/PersonnelAttendanceTimes/4/2023-01
        [HttpGet("id/{id}/{y}-{m}/{page}")]
        public async Task<ActionResult<dynamic>> GetAttendanceTimeWithProfile(int id, int y, int m,int page)
        {
            var personnelAttendanceTime = from pa in _context.PersonnelAttendanceTimes
                                          join pd in _context.PersonnelProfileDetails on pa.EmployeeId equals pd.EmployeeId
                                          where pa.EmployeeId == id && pa.Date.Year == y && pa.Date.Month == m
                                          select new
                                          {
                                              Date = pa.Date.ToString(),
                                              pa.AttendTime,
                                              pd.EmployeeName,
                                              pd.EmployeeNumber
                                          };

            if (personnelAttendanceTime == null)
            {
                return NotFound();
            }
            var query = personnelAttendanceTime.Skip((page - 1) * 15).Take(15);
            return await query.ToListAsync();
        }

        //個人查詢 頁面
        // GET: api/PersonnelAttendanceTimes/4/2023-01
        [HttpGet("Page/id/{id}/{y}-{m}")]
        public int GetAttendanceTimeWithProfilePage(int id, int y, int m)
        {
            var personnelAttendanceTime = from pa in _context.PersonnelAttendanceTimes
                                          join pd in _context.PersonnelProfileDetails on pa.EmployeeId equals pd.EmployeeId
                                          where pa.EmployeeId == id && pa.Date.Year == y && pa.Date.Month == m
                                          select new
                                          {
                                              Date = pa.Date.ToString(),
                                              pa.AttendTime,
                                              pd.EmployeeName,
                                              pd.EmployeeNumber
                                          };

            if (personnelAttendanceTime == null)
            {
                return 0;
            }
            var total = Convert.ToInt16(Math.Ceiling(Convert.ToDouble(personnelAttendanceTime.Count())/15));

            return total;
        }
        //工號查詢
        // GET: api/PersonnelAttendanceTimes/number/5
        //[HttpGet("number/{number}/{y}-{m}")]
        //public async Task<ActionResult<dynamic>> GetAttendanceTime(string number, int y, int m)
        //{
        //    var personnelAttendanceTime = from pa in _context.PersonnelAttendanceTimes
        //                                  join pd in _context.PersonnelProfileDetails on pa.EmployeeId equals pd.EmployeeId
        //                                  where pd.EmployeeNumber == number && pa.Date.Year == y && pa.Date.Month == m
        //                                  select new
        //                                  {
        //                                      Date = pa.Date.ToString(),
        //                                      pa.AttendTime,
        //                                      pd.EmployeeName,
        //                                      pd.EmployeeNumber
        //                                  };

        //    if (personnelAttendanceTime == null)
        //    {
        //        return NotFound();
        //    }

        //    return await personnelAttendanceTime.ToListAsync();
        //}


        //名稱查詢
        // GET: api/PersonnelAttendanceTimes/name/5
        //[HttpGet("name/{name}/{y}-{m}")]
        //public async Task<ActionResult<dynamic>> GetAttendanceTimeWithName(string name, int y, int m)
        //{
        //    var personnelAttendanceTime = from pa in _context.PersonnelAttendanceTimes
        //                                  join pd in _context.PersonnelProfileDetails on pa.EmployeeId equals pd.EmployeeId
        //                                  where pd.EmployeeName == name && pa.Date.Year == y && pa.Date.Month == m
        //                                  select new
        //                                  {
        //                                      Date = pa.Date.ToString(),
        //                                      pa.AttendTime,
        //                                      pd.EmployeeName,
        //                                      pd.EmployeeNumber

        //                                  };

        //    if (personnelAttendanceTime == null)
        //    {
        //        return NotFound();
        //    }

        //    return await personnelAttendanceTime.ToListAsync();
        //}

        //部門查詢
        // GET: api/PersonnelAttendanceTimes/dep/5
        //[HttpGet("dep/{dep}/{y}-{m}")]
        //public async Task<ActionResult<dynamic>> GetAttendanceTimeWithDep(int dep, int y, int m)
        //{
        //    var personnelAttendanceTime = from pa in _context.PersonnelAttendanceTimes
        //                                  join pd in _context.PersonnelProfileDetails on pa.EmployeeId equals pd.EmployeeId
        //                                  where pd.DepartmentId == dep && pa.Date.Year == y && pa.Date.Month == m
        //                                  select new
        //                                  {
        //                                      Date = pa.Date.ToString(),
        //                                      pa.AttendTime,
        //                                      pd.EmployeeName,
        //                                      pd.EmployeeNumber
        //                                  };

        //    if (personnelAttendanceTime == null)
        //    {
        //        return NotFound();
        //    }

        //    return await personnelAttendanceTime.ToListAsync();
        //}

        //複合查詢
        // GET: api/PersonnelAttendanceTimes/Complex/5/name/2023001
        [HttpGet("Complex/{y}-{m}")]
        public async Task<ActionResult<dynamic>> GetAttendanceTimeComplex(int? dep,string name , string number, int y, int m,int page)
        {
            var personnelAttendanceTime = from pa in _context.PersonnelAttendanceTimes
                                          join pd in _context.PersonnelProfileDetails on pa.EmployeeId equals pd.EmployeeId
                                          where pa.Date.Year == y && pa.Date.Month == m
                                          select new
                                          {
                                              Date = pa.Date.ToString(),
                                              pa.AttendTime,
                                              pd.EmployeeName,
                                              pd.EmployeeNumber,
                                              pd.DepartmentId
                                          };

            if (personnelAttendanceTime == null)
            {
                return NotFound();
            }
            if(dep != null) { personnelAttendanceTime = personnelAttendanceTime.Where(a => a.DepartmentId == dep); }
            if (!string.IsNullOrWhiteSpace(name)) { personnelAttendanceTime = personnelAttendanceTime.Where(a => a.EmployeeName.Contains(name)); }
            if (!string.IsNullOrWhiteSpace(number)) { personnelAttendanceTime = personnelAttendanceTime.Where(a => a.EmployeeNumber.Contains(number)); }

            var query = personnelAttendanceTime.Skip((page-1) * 15).Take(15);
            var totalpage = query.Count();
            return await query.ToListAsync();
        }


        //複合查詢頁數
        // GET: api/PersonnelAttendanceTimes/Complex/5/name/2023001
        [HttpGet("ComplexPage/{y}-{m}")]
        public int GetAttendanceTimePage(int? dep, string name, string number, int y, int m, int page)
        {
            var personnelAttendanceTime = from pa in _context.PersonnelAttendanceTimes
                                          join pd in _context.PersonnelProfileDetails on pa.EmployeeId equals pd.EmployeeId
                                          where pa.Date.Year == y && pa.Date.Month == m
                                          select new
                                          {
                                              Date = pa.Date.ToString(),
                                              pa.AttendTime,
                                              pd.EmployeeName,
                                              pd.EmployeeNumber,
                                              pd.DepartmentId
                                          };

            if (personnelAttendanceTime == null)
            {
                return 0;
            }
            if (dep != null) { personnelAttendanceTime = personnelAttendanceTime.Where(a => a.DepartmentId == dep); }
            if (!string.IsNullOrWhiteSpace(name)) { personnelAttendanceTime = personnelAttendanceTime.Where(a => a.EmployeeName.Contains(name)); }
            if (!string.IsNullOrWhiteSpace(number)) { personnelAttendanceTime = personnelAttendanceTime.Where(a => a.EmployeeNumber.Contains(number)); }
            var query = personnelAttendanceTime.Skip(page * 15).Take(15);
            var totalpage = Convert.ToInt16(Math.Ceiling(Convert.ToDouble(personnelAttendanceTime.Count())/ 15));
            
            return totalpage;
        }
        // PUT: api/PersonnelAttendanceTimes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPersonnelAttendanceTime(int id, PersonnelAttendanceTime personnelAttendanceTime)
        {
            if (id != personnelAttendanceTime.AttendId)
            {
                return BadRequest();
            }

            _context.Entry(personnelAttendanceTime).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonnelAttendanceTimeExists(id))
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

        //打卡登入
        // POST: api/PersonnelAttendanceTimes/number
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{id}")]
        public ActionResult PersonAttdence(int id, [FromBody] PersonnelAttendanceTime personnelAttendanceTime)
        {
            var attendencedate = DateTime.Now;
            var attendencetime = DateTime.Now.ToString("HH:mm");


                PersonnelAttendanceTime insert = new PersonnelAttendanceTime
                {
                    EmployeeId = personnelAttendanceTime.EmployeeId,
                    Date = attendencedate,
                    AttendTime = attendencetime,

                };
            if (personnelAttendanceTime.EmployeeId == id)
            {
                _context.PersonnelAttendanceTimes.Add(insert);
                _context.SaveChanges();
                return Content("打卡成功");
            }
            else
            {
                return Content("打卡錯誤:非本人打卡或伺服器錯誤，請再試一次");
            }

        }


        // POST: api/PersonnelAttendanceTimes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<PersonnelAttendanceTime>> PostPersonnelAttendanceTime(PersonnelAttendanceTime personnelAttendanceTime)
        //{
        //    _context.PersonnelAttendanceTimes.Add(personnelAttendanceTime);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetPersonnelAttendanceTime", new { id = personnelAttendanceTime.AttendId }, personnelAttendanceTime);
        //}

        // DELETE: api/PersonnelAttendanceTimes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePersonnelAttendanceTime(int id)
        {
            var personnelAttendanceTime = await _context.PersonnelAttendanceTimes.FindAsync(id);
            if (personnelAttendanceTime == null)
            {
                return NotFound();
            }

            _context.PersonnelAttendanceTimes.Remove(personnelAttendanceTime);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PersonnelAttendanceTimeExists(int id)
        {
            return _context.PersonnelAttendanceTimes.Any(e => e.AttendId == id);
        }
    }
}
