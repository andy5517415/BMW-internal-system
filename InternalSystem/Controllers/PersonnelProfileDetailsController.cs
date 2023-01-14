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

        //個人資料觀看
        // GET: api/PersonnelProfileDetails/5
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
                                          Terminationdate = o.Terminationdate.ToString()

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
                                         where o.DepartmentId == dep && o.PositionId==position && o.EmployeeId != id
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
                                         where o.DepartmentId == dep && o.RankId == 5 && o.EmployeeId != id
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

            if (SearchNameProfileDetail == null)
            {
                return NotFound();
            }

            return await SearchNameProfileDetail.FirstOrDefaultAsync();
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

        // POST: api/PersonnelProfileDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PersonnelProfileDetail>> PostPersonnelProfileDetail(PersonnelProfileDetail personnelProfileDetail)
        {
            _context.PersonnelProfileDetails.Add(personnelProfileDetail);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPersonnelProfileDetail", new { id = personnelProfileDetail.EmployeeId }, personnelProfileDetail);
        }

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
