using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InternalSystem.Models;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InternalSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonnelProfileDetailsController : ControllerBase
    {
        private readonly MSIT44Context _context;

        public PersonnelProfileDetailsController(MSIT44Context context)
        {
            _context = context;
        }

        // GET: api/PersonnelProfileDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonnelProfileDetail>>> GetPersonnelProfileDetails()
        {
            return await _context.PersonnelProfileDetails.ToListAsync();
        }

        // GET: api/PersonnelProfileDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PersonnelProfileDetail>> GetPersonnelProfileDetail(int id)
        {
            var personnelProfileDetail = await _context.PersonnelProfileDetails.FindAsync(id);

            if (personnelProfileDetail == null)
            {
                return NotFound();
            }

            return personnelProfileDetail;
        }

        //員工資料查看
        // GET: api/PersonnelProfileDetails/Personnel
        [HttpGet("Personnel")]
        public async Task<ActionResult<dynamic>> GetPersonnelProfile(int? depid,int? position , int? rank,bool? dutystatus, int page)
        {
            var personnelProfileDetail = from p in _context.PersonnelProfileDetails
                                         join dep in _context.PersonnelDepartmentLists on p.DepartmentId equals dep.DepartmentId
                                         join r in _context.PersonnelRanks on p.RankId equals r.RankId
                                         join ps in _context.PersonnelPositions on p.PositionId equals ps.PositionId
                                         select new
                                         {
                                             p.EmployeeName,
                                             p.EmployeeNumber,
                                             p.DepartmentId,
                                             p.DutyStatus,
                                             dep.DepName,
                                             p.PositionId,
                                             p.RankId,
                                             r.Rank,
                                             ps.PositionName
                                         };

            if (personnelProfileDetail == null)
            {
                return NotFound();
            }
            if(depid != null) { personnelProfileDetail = personnelProfileDetail.Where(a => a.DepartmentId == depid); }
            if(position != null) { personnelProfileDetail = personnelProfileDetail.Where(a => a.PositionId == position); }
            if(rank != null) { personnelProfileDetail = personnelProfileDetail.Where(a => a.RankId == rank); }
            if(dutystatus != null) { personnelProfileDetail = personnelProfileDetail.Where(a => a.DutyStatus == dutystatus); }
            var query = personnelProfileDetail.Skip((page - 1) * 15).Take(15);

            return await query.ToListAsync();
        }


        //員工資料查看頁面
        // GET: api/PersonnelProfileDetails/PersonnelPage
        [HttpGet("PersonnelPage")]
        public int  GetPersonnelProfilePage(int? depid, int? position, int? rank, bool? dutystatus)
        {
            var personnelProfileDetail = from p in _context.PersonnelProfileDetails
                                         join dep in _context.PersonnelDepartmentLists on p.DepartmentId equals dep.DepartmentId
                                         join r in _context.PersonnelRanks on p.RankId equals r.RankId
                                         join ps in _context.PersonnelPositions on p.PositionId equals ps.PositionId
                                         select new
                                         {
                                             p.EmployeeName,
                                             p.EmployeeNumber,
                                             p.DepartmentId,
                                             p.DutyStatus,
                                             dep.DepName,
                                             p.PositionId,
                                             p.RankId,
                                             r.Rank,
                                             ps.PositionName
                                         };

            if (personnelProfileDetail == null)
            {
                return 0;
            }
            if (depid != null) { personnelProfileDetail = personnelProfileDetail.Where(a => a.DepartmentId == depid); }
            if (position != null) { personnelProfileDetail = personnelProfileDetail.Where(a => a.PositionId == position); }
            if (rank != null) { personnelProfileDetail = personnelProfileDetail.Where(a => a.RankId == rank); }
            if (dutystatus != null) { personnelProfileDetail = personnelProfileDetail.Where(a => a.DutyStatus == dutystatus); }
            var total  = Convert.ToInt16(Math.Ceiling(Convert.ToDouble(personnelProfileDetail.Count()) / 15)); 
            return total;
        }
        //// GET: api/PersonnelProfileDetails/oderby/findnew
        //[HttpGet("oderby/findnew")]
        //public ActionResult ProfileDetail()
        //{

        //    var personnelProfileDetail = _context.PersonnelProfileDetails
        //        .OrderByDescending(x => x.EmployeeNumber).Select(x => new
        //        {
        //           EmployeeNumber =  Convert.ToInt64(x.EmployeeNumber)
        //        }).First();
        //    var newyear = System.DateTime.Now.Year;
        //    var yearint = Convert.ToInt64(newyear + "001");
        //    var a = personnelProfileDetail.EmployeeNumber;
        //    if (newyear > a)
        //    {
        //        return Content(newyear.ToString());
        //    }
        //    var sa = newyear.ToString();
        //    return Content(sa);
        //}

        //生成工號
        // GET: api/PersonnelProfileDetails/makenumber
        [HttpGet("makenumber")]
        public async Task<ActionResult<dynamic>> GetId()
        {
            var date = DateTime.Now.ToString("yyyy");
            var personnelProfileDetail = from o in _context.PersonnelProfileDetails
                                         orderby o.EmployeeId descending
                                         select new
                                         {
                                             EmployeeNumber = date + (o.EmployeeId + 1).ToString(),
                                             o.EmployeeId
                                         };
            foreach (var i in personnelProfileDetail)
            {
                if (i.EmployeeId > 1000)
                {
                    string end = i.EmployeeId.ToString();
                    int endindex = end.Length - 3;

                    string EmployeeNumber = date + (i.EmployeeId + 1).ToString().Substring(endindex, 3);
                    return EmployeeNumber;
                }
                else if (i.EmployeeId < 100 && i.EmployeeId > 10)
                {
                    string EmployeeNumber = date + "0" + (i.EmployeeId + 1).ToString();
                    return EmployeeNumber;
                }
                else if (i.EmployeeId < 10)
                {
                    string EmployeeNumber = date + "00" + (i.EmployeeId + 1).ToString();
                    return EmployeeNumber;
                }
                else
                {
                    string EmployeeNumber = date + (i.EmployeeId + 1).ToString();
                    return EmployeeNumber;
                }
            }
            if (personnelProfileDetail == null)
            {
                return NotFound();
            }

            return await personnelProfileDetail.FirstOrDefaultAsync();
        }


        //個人資料觀看
        // GET: api/PersonnelProfileDetails/profile/5
        [HttpGet("profile/{id}")]
        public async Task<ActionResult<dynamic>> GetProfileDetail(int id)
        {
            var SearchProfileDetail = from o in _context.PersonnelProfileDetails
                                      where o.EmployeeId == id
                                      join c in _context.PersonnelCityLists on o.CityId equals c.CityId
                                      join p in _context.PersonnelPositions on o.PositionId equals p.PositionId
                                      join d in _context.PersonnelDepartmentLists on o.DepartmentId equals d.DepartmentId
                                      join r in _context.PersonnelRanks on o.RankId equals r.RankId
                                      select new
                                      {
                                          EmployeeId = o.EmployeeId,
                                          EmployeeName = o.EmployeeName,
                                          Sex = o.Sex,
                                          IsMarried = o.IsMarried,
                                          IdentiyId = o.IdentiyId,
                                          CityId = o.CityId,
                                          PositionId = o.PositionId,
                                          DepartmentId = o.DepartmentId,
                                          RankId = o.RankId,
                                          EmployeeNumber = o.EmployeeNumber,
                                          HomePhone = o.HomePhone,
                                          Email = o.Email,
                                          Birthday = o.Birthday.ToString(),
                                          PhoneNumber = o.PhoneNumber,
                                          Address = o.Address,
                                          DutyStatus = o.DutyStatus,
                                          Country = o.Country,
                                          EmergencyNumber = o.EmergencyNumber,
                                          EmergencyPerson = o.EmergencyPerson,
                                          EmergencyRelation = o.EmergencyRelation,
                                          EntryDate = o.EntryDate.ToString(),
                                          Acount = o.Acount,
                                          Password = o.Password,
                                          Terminationdate = o.Terminationdate.ToString(),
                                          o.Photo
                                      };

            if (SearchProfileDetail == null)
            {
                return NotFound();
            }

            return await SearchProfileDetail.FirstOrDefaultAsync();
        }

        //用工號尋找ID
        // GET: api/PersonnelProfileDetails/Number/2023001
        [HttpGet("idfind/Number/{Number}")]
        public async Task<ActionResult<dynamic>> GetPersonnelId(string Number)
        {
            var personnelProfileDetail = from o in _context.PersonnelProfileDetails
                                         where o.EmployeeNumber == Number
                                         select new
                                         {
                                             EmployeeId = o.EmployeeId
                                         };

            if (personnelProfileDetail == null)
            {
                return NotFound();
            }

            return await personnelProfileDetail.FirstOrDefaultAsync();
        }


        //找代理人
        // GET: api/PersonnelProfileDetails/proxy/dep/rank
        [HttpGet("proxy/{dep}/{position}/{id}")]
        public async Task<ActionResult<dynamic>> GetLeaveProxy(int dep, int position, int id)
        {
            var personnelProfileDetail = from o in _context.PersonnelProfileDetails
                                         where o.DepartmentId == dep && o.PositionId>=position && o.PositionId <7 && o.EmployeeId != id
                                         && o.DutyStatus==true
                                         select new
                                         {
                                             EmployeeId = o.EmployeeId,
                                             EmployeeName = o.EmployeeName
                                         };

            if (personnelProfileDetail == null)
            {
                return NotFound();
            }

            return await personnelProfileDetail.ToListAsync();
        }

        //找主管
        // GET: api/PersonnelProfileDetails/Manager/dep
        [HttpGet("Manager/{dep}/{id}")]
        public async Task<ActionResult<dynamic>> GetLeaveManager(int dep, int id)
        {
            var personnelProfileDetail = from o in _context.PersonnelProfileDetails
                                         where o.DepartmentId == dep && o.PositionId == 7 && o.EmployeeId != id && o.DutyStatus==true
                                         select new
                                         {
                                             EmployeeId = o.EmployeeId,
                                             EmployeeName = o.EmployeeName
                                         };

            if (personnelProfileDetail == null)
            {
                return NotFound();
            }

            return await personnelProfileDetail.ToListAsync();
        }


        //api/PersonnelProfileDetails/uid/2023001
        [HttpGet("uid/{id}")]
        public async Task<ActionResult<dynamic>> SearchGetPersonnelProfileDetail(int id)
        {

            var SearchProfileDetail = from o in _context.PersonnelProfileDetails
                                      where o.EmployeeId == id
                                      join c in _context.PersonnelCityLists on o.CityId equals c.CityId
                                      join p in _context.PersonnelPositions on o.PositionId equals p.PositionId
                                      join d in _context.PersonnelDepartmentLists on o.DepartmentId equals d.DepartmentId
                                      join r in _context.PersonnelRanks on o.RankId equals r.RankId
                                      select new
                                      {
                                          EmployeeId = o.EmployeeId,
                                          EmployeeName = o.EmployeeName,
                                          Sex = o.Sex,
                                          IsMarried = o.IsMarried,
                                          IdentiyId = o.IdentiyId,
                                          CityId = o.CityId,
                                          PositionId = o.PositionId,
                                          DepartmentId = o.DepartmentId,
                                          RankId = o.RankId,
                                          EmployeeNumber = o.EmployeeNumber,
                                          HomePhone = o.HomePhone,
                                          Email = o.Email,
                                          Birthday = o.Birthday.ToString(),
                                          PhoneNumber = o.PhoneNumber,
                                          Address = o.Address,
                                          DutyStatus = o.DutyStatus,
                                          Country = o.Country,
                                          EmergencyNumber = o.EmergencyNumber,
                                          EmergencyPerson = o.EmergencyPerson,
                                          EmergencyRelation = o.EmergencyRelation,
                                          EntryDate = o.EntryDate.ToString(),
                                          Acount = o.Acount,
                                          Password = o.Password,
                                          Terminationdate = o.Terminationdate.ToString()

                                      };


            if (SearchProfileDetail == null)
            {
                return NotFound();
            }

            return await SearchProfileDetail.FirstOrDefaultAsync();
        }

        //人事部門尋找員工資料修改(用工號)
        //api/PersonnelProfileDetails/Number/2023001
        [HttpGet("Number/{id}")]
        public async Task<ActionResult<dynamic>> SearchGetPersonnelProfileDetail(string id)
        {

            var SearchProfileDetail = from o in _context.PersonnelProfileDetails
                                      where o.EmployeeNumber == id
                                      join c in _context.PersonnelCityLists on o.CityId equals c.CityId
                                      join p in _context.PersonnelPositions on o.PositionId equals p.PositionId
                                      join d in _context.PersonnelDepartmentLists on o.DepartmentId equals d.DepartmentId
                                      join r in _context.PersonnelRanks on o.RankId equals r.RankId
                                      select new
                                      {
                                          EmployeeId = o.EmployeeId,
                                          EmployeeName = o.EmployeeName,
                                          Sex = o.Sex,
                                          IsMarried = o.IsMarried,
                                          IdentiyId = o.IdentiyId,
                                          CityId = o.CityId,
                                          PositionId = o.PositionId,
                                          DepartmentId = o.DepartmentId,
                                          RankId = o.RankId,
                                          EmployeeNumber = o.EmployeeNumber,
                                          HomePhone = o.HomePhone,
                                          Email = o.Email,
                                          Birthday = o.Birthday.ToString(),
                                          PhoneNumber = o.PhoneNumber,
                                          o.Photo,
                                          Address = o.Address,
                                          DutyStatus = o.DutyStatus,
                                          Country = o.Country,
                                          EmergencyNumber = o.EmergencyNumber,
                                          EmergencyPerson = o.EmergencyPerson,
                                          EmergencyRelation = o.EmergencyRelation,
                                          EntryDate = o.EntryDate.ToString(),
                                          Acount = o.Acount,
                                          Password = o.Password,
                                          Terminationdate = o.Terminationdate.ToString(),
                                          o.Note
                                      };

            if (SearchProfileDetail == null)
            {
                return NotFound();
            }

            return await SearchProfileDetail.FirstOrDefaultAsync();
        }


        //測試用(尋找該員工假別 假種 目前已在假別控制器使用 此無作用)
        //api/PersonnelProfileDetails/EmployeeNumber/2023001
        [HttpGet("EmployeeNumber/{id}")]
        public async Task<ActionResult<dynamic>> SearchGetPersonnelLeave(string id)
        {

            var SearchProfileDetail = from o in _context.PersonnelProfileDetails
                                      where o.EmployeeNumber == id
                                      join LTF in _context.PersonnelLeaveOvers on o.EmployeeId equals LTF.EmployeeId
                                      join LT in _context.PersonnelLeaveTypes on LTF.LeaveType equals LT.LeaveTypeId
                                      select new
                                      {
                                          EmployeeId = o.EmployeeId,
                                          LeaveType = LTF.LeaveType,
                                          Quantity = LTF.Quantity,
                                          Used = LTF.Used,
                                          LTF.LeaveOver

                                      };


            if (SearchProfileDetail == null)
            {
                return NotFound();
            }

            return await SearchProfileDetail.FirstOrDefaultAsync();
        }

        //人事部尋找個人資料修改(名稱尋找)
        //api/PersonnelProfileDetails/na/name
        [HttpGet("na/{name}")]
        public async Task<ActionResult<dynamic>> SearchNameGetPersonnelProfileDetail(string name)
        {

            var SearchNameProfileDetail = from o in _context.PersonnelProfileDetails
                                          where o.EmployeeName == name
                                          join c in _context.PersonnelCityLists on o.CityId equals c.CityId
                                          join p in _context.PersonnelPositions on o.PositionId equals p.PositionId
                                          join d in _context.PersonnelDepartmentLists on o.DepartmentId equals d.DepartmentId
                                          join r in _context.PersonnelRanks on o.RankId equals r.RankId
                                          select new
                                          {
                                              EmployeeId = o.EmployeeId,
                                              EmployeeName = o.EmployeeName,
                                              Sex = o.Sex,
                                              IsMarried = o.IsMarried,
                                              IdentiyId = o.IdentiyId,
                                              CityId = o.CityId,
                                              PositionId = o.PositionId,
                                              DepartmentId = o.DepartmentId,
                                              RankId = o.RankId,
                                              EmployeeNumber = o.EmployeeNumber,
                                              HomePhone = o.HomePhone,
                                              Email = o.Email,
                                              Birthday = o.Birthday.ToString(),
                                              PhoneNumber = o.PhoneNumber,
                                              o.Photo,
                                              Address = o.Address,
                                              DutyStatus = o.DutyStatus,
                                              Country = o.Country,
                                              EmergencyNumber = o.EmergencyNumber,
                                              EmergencyPerson = o.EmergencyPerson,
                                              EmergencyRelation = o.EmergencyRelation,
                                              EntryDate = o.EntryDate.ToString(),
                                              Acount = o.Acount,
                                              Password = o.Password,
                                              Terminationdate = o.Terminationdate.ToString(),
                                              o.Note

                                          };

            if (SearchNameProfileDetail == null)
            {
                return NotFound();
            }

            return await SearchNameProfileDetail.FirstOrDefaultAsync();
        }


        //人事部門尋找員工資料修改(複合查詢)
        //api/PersonnelProfileDetails/Complex/2023001
        [HttpGet("Complex")]
        public async Task<ActionResult<dynamic>> SearchGetPersonnelProfileDetailComplex(string number ,string name)
        {

            var SearchProfileDetail = from o in _context.PersonnelProfileDetails
                                      join c in _context.PersonnelCityLists on o.CityId equals c.CityId
                                      join p in _context.PersonnelPositions on o.PositionId equals p.PositionId
                                      join d in _context.PersonnelDepartmentLists on o.DepartmentId equals d.DepartmentId
                                      join r in _context.PersonnelRanks on o.RankId equals r.RankId
                                      select new
                                      {
                                          EmployeeId = o.EmployeeId,
                                          EmployeeName = o.EmployeeName,
                                          Sex = o.Sex,
                                          IsMarried = o.IsMarried,
                                          IdentiyId = o.IdentiyId,
                                          CityId = o.CityId,
                                          PositionId = o.PositionId,
                                          DepartmentId = o.DepartmentId,
                                          RankId = o.RankId,
                                          EmployeeNumber = o.EmployeeNumber,
                                          HomePhone = o.HomePhone,
                                          Email = o.Email,
                                          Birthday = o.Birthday.ToString(),
                                          PhoneNumber = o.PhoneNumber,
                                          o.Photo,
                                          Address = o.Address,
                                          DutyStatus = o.DutyStatus,
                                          Country = o.Country,
                                          EmergencyNumber = o.EmergencyNumber,
                                          EmergencyPerson = o.EmergencyPerson,
                                          EmergencyRelation = o.EmergencyRelation,
                                          EntryDate = o.EntryDate.ToString(),
                                          Acount = o.Acount,
                                          Password = o.Password,
                                          Terminationdate = o.Terminationdate.ToString(),
                                          o.Note

                                      };

            if (SearchProfileDetail == null)
            {
                return NotFound();
            }
            if (!string.IsNullOrWhiteSpace(number))
            {
                SearchProfileDetail = SearchProfileDetail.Where(a => a.EmployeeNumber == number);
            }
            if (!string.IsNullOrWhiteSpace(name))
            {
                SearchProfileDetail = SearchProfileDetail.Where(a => a.EmployeeName == name);
            }
            if(string.IsNullOrWhiteSpace(number)&& string.IsNullOrWhiteSpace(name))
            {
                return null;
            }
            return await SearchProfileDetail.FirstOrDefaultAsync();
        }

        //api/PersonnelProfileDetails/na/5
        [HttpPut("na/{id}")]
        public async Task<IActionResult> PutPersonnelEditProfile(int id, PersonnelProfileDetail SearchNameProfileDetail)
        {

            if (id != SearchNameProfileDetail.EmployeeId)
            {
                return BadRequest();
            }




            _context.Entry(SearchNameProfileDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonnelProfileDetailExists(id))
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


        // PUT: api/PersonnelProfileDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPersonnelProfileDetail(int id, PersonnelProfileDetail personnelProfileDetail)
        {
            if (id != personnelProfileDetail.EmployeeId)
            {
                return BadRequest();
            }

            _context.Entry(personnelProfileDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonnelProfileDetailExists(id))
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

        //父子資料新增(員工+該員工假別時數)
        // POST: api/PersonnelProfileDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult PostPersonnelProfileDetail([FromBody] PersonnelProfileDetail personnelProfileDetail)
        {

          

            PersonnelProfileDetail insert = new PersonnelProfileDetail
            {
                EmployeeName = personnelProfileDetail.EmployeeName,
                Sex = personnelProfileDetail.Sex,
                IsMarried = personnelProfileDetail.IsMarried,
                IdentiyId = personnelProfileDetail.IdentiyId,
                Email = personnelProfileDetail.Email,
                Birthday = personnelProfileDetail.Birthday,
                PhoneNumber = personnelProfileDetail.PhoneNumber,
                CityId = personnelProfileDetail.CityId,
                Address = personnelProfileDetail.Address,
                HomePhone = personnelProfileDetail.HomePhone,
                EmergencyPerson = personnelProfileDetail.EmergencyPerson,
                EmergencyRelation = personnelProfileDetail.EmergencyRelation,
                EmergencyNumber = personnelProfileDetail.EmergencyNumber,
                Country = personnelProfileDetail.Country,
                PositionId = personnelProfileDetail.PositionId,
                RankId = personnelProfileDetail.RankId,
                DepartmentId = personnelProfileDetail.DepartmentId,
                EmployeeNumber = personnelProfileDetail.EmployeeNumber,
                Acount = personnelProfileDetail.Acount,
                Password = personnelProfileDetail.Password,
                DutyStatus = true,
                EntryDate = personnelProfileDetail.EntryDate,
                Terminationdate = personnelProfileDetail.Terminationdate,
                Photo = personnelProfileDetail.Photo,
                Note = personnelProfileDetail.Note,
                //PersonnelLeaveOvers = personnelProfileDetail.PersonnelLeaveOvers
            };
            _context.PersonnelProfileDetails.Add(insert);
            _context.SaveChanges();
                int q;
            for (var i = 1; i < 5;i++)
            {
                if (i == 1) {    q= 112;   }
                else if (i == 2) {   q = 240;}
                else if (i == 4&& personnelProfileDetail.Sex=="女性") { q = 96; }
                else {  q = 0;  }
                PersonnelLeaveOver insertover = new PersonnelLeaveOver
                {
                    LeaveType = i,
                    Quantity = q,
                    Used = 0,
                    LeaveOver = q,
                    EmployeeId = insert.EmployeeId,

                };
            _context.PersonnelLeaveOvers.Add(insertover);
            }
            _context.SaveChanges();

            return Content("新建完成");
        }


        //原生post
        // POST: api/PersonnelProfileDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<PersonnelProfileDetail>> PostPersonnelProfileDetail(PersonnelProfileDetail personnelProfileDetail)
        //{
        //    _context.PersonnelProfileDetails.Add(personnelProfileDetail);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetPersonnelProfileDetail", new { id = personnelProfileDetail.EmployeeId }, personnelProfileDetail);
        //}

        // DELETE: api/PersonnelProfileDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePersonnelProfileDetail(int id)
        {
            var personnelProfileDetail = await _context.PersonnelProfileDetails.FindAsync(id);
            if (personnelProfileDetail == null)
            {
                return NotFound();
            }

            _context.PersonnelProfileDetails.Remove(personnelProfileDetail);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PersonnelProfileDetailExists(int id)
        {
            return _context.PersonnelProfileDetails.Any(e => e.EmployeeId == id);
        }
    }
}
