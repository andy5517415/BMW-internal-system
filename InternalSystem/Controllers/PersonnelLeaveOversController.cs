using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InternalSystem.Models;
using InternalSystem.Dotos;
namespace InternalSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonnelLeaveOversController : ControllerBase
    {
        private readonly MSIT44Context _context;

        public PersonnelLeaveOversController(MSIT44Context context)
        {
            _context = context;
        }

        // GET: api/PersonnelLeaveOvers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonnelLeaveOver>>> GetPersonnelLeaveOvers()
        {
            return await _context.PersonnelLeaveOvers.ToListAsync();
        }

        //主管審核得到該員工假別 同意時用此作PUT用
        // GET: api/PersonnelLeaveOvers/1/5
        [HttpGet("{lid}/{id}")]
        public async Task<ActionResult<dynamic>> GetAgree(int lid ,int id)
        {
            var personnelLeaveOver = from pl in _context.PersonnelLeaveOvers
                                     join p in _context.PersonnelProfileDetails on pl.EmployeeId equals p.EmployeeId
                                     join l in _context.PersonnelLeaveTypes on pl.LeaveType equals l.LeaveTypeId
                                     where p.EmployeeId == id && pl.LeaveType == lid
                                     select new
                                     {
                                         p.EmployeeId,
                                         pl.LeaveType,
                                         pl.Quantity,
                                         pl.LeaveOver,
                                         pl.Used
                                     };


            if (personnelLeaveOver == null)
            {
                return NotFound();
            }

            return await personnelLeaveOver.FirstOrDefaultAsync();
        }


        //找尋該員工假別1
        // GET: api/PersonnelLeaveOvers/leave1/5
        [HttpGet("leave1/{id}")]
        public async Task<ActionResult<dynamic>> GetLeaveOver(int id)
        {
            var personnelLeaveOver = from pl in _context.PersonnelLeaveOvers
                                     join p in _context.PersonnelProfileDetails on pl.EmployeeId equals p.EmployeeId
                                     join l in _context.PersonnelLeaveTypes on pl.LeaveType equals l.LeaveTypeId
                                     where p.EmployeeId == id && pl.LeaveType == 1
                                     select new
                                     {
                                         p.EmployeeId,
                                         pl.LeaveType,
                                         pl.Quantity,
                                         l.Type,
                                         pl.LeaveOver,
                                         pl.Used
                                     };


            if (personnelLeaveOver == null)
            {
                return NotFound();
            }

            return await personnelLeaveOver.FirstOrDefaultAsync();
        }

        //找尋該員工假別2
        // GET: api/PersonnelLeaveOvers/leave1/5
        [HttpGet("leave2/{id}")]
        public async Task<ActionResult<dynamic>> GetLeave2Over(int id)
        {
            var personnelLeaveOver = from pl in _context.PersonnelLeaveOvers
                                     join p in _context.PersonnelProfileDetails on pl.EmployeeId equals p.EmployeeId
                                     join l in _context.PersonnelLeaveTypes on pl.LeaveType equals l.LeaveTypeId
                                     where p.EmployeeId == id && pl.LeaveType == 2
                                     select new
                                     {
                                         p.EmployeeId,
                                         pl.LeaveType,
                                         pl.Quantity,
                                         l.Type,
                                         pl.LeaveOver,
                                         pl.Used
                                     };


            if (personnelLeaveOver == null)
            {
                return NotFound();
            }

            return await personnelLeaveOver.FirstOrDefaultAsync();
        }

        //找尋該員工假別3
        // GET: api/PersonnelLeaveOvers/leave3/5
        [HttpGet("leave3/{id}")]
        public async Task<ActionResult<dynamic>> GetLeave3Over(int id)
        {
            var personnelLeaveOver = from pl in _context.PersonnelLeaveOvers
                                     join p in _context.PersonnelProfileDetails on pl.EmployeeId equals p.EmployeeId
                                     join l in _context.PersonnelLeaveTypes on pl.LeaveType equals l.LeaveTypeId
                                     where p.EmployeeId == id && pl.LeaveType == 3
                                     select new
                                     {
                                         p.EmployeeId,
                                         pl.LeaveType,
                                         pl.Quantity,
                                         l.Type,
                                         pl.LeaveOver,
                                         pl.Used
                                     };


            if (personnelLeaveOver == null)
            {
                return NotFound();
            }

            return await personnelLeaveOver.FirstOrDefaultAsync();
        }

        //找尋該員工假別4
        // GET: api/PersonnelLeaveOvers/leave4/5
        [HttpGet("leave4/{id}")]
        public async Task<ActionResult<dynamic>> GetLeave4Over(int id)
        {
            var personnelLeaveOver = from pl in _context.PersonnelLeaveOvers
                                     join p in _context.PersonnelProfileDetails on pl.EmployeeId equals p.EmployeeId
                                     join l in _context.PersonnelLeaveTypes on pl.LeaveType equals l.LeaveTypeId
                                     where p.EmployeeId == id && pl.LeaveType == 4
                                     select new
                                     {
                                         p.EmployeeId,
                                         pl.LeaveType,
                                         pl.Quantity,
                                         l.Type,
                                         pl.LeaveOver,
                                         pl.Used
                                     };


            if (personnelLeaveOver == null)
            {
                return NotFound();
            }

            return await personnelLeaveOver.FirstOrDefaultAsync();
        }

        // GET: api/PersonnelLeaveOvers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PersonnelLeaveOver>> GetPersonnelLeaveOver(int id)
        {
            var personnelLeaveOver = await _context.PersonnelLeaveOvers.FindAsync(id);

            if (personnelLeaveOver == null)
            {
                return NotFound();
            }

            return personnelLeaveOver;
        }

        // PUT: api/PersonnelLeaveOvers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPersonnelLeaveOver(int id, PersonnelLeaveOver personnelLeaveOver)
        {
            if (id != personnelLeaveOver.EmployeeId)
            {
                return BadRequest();
            }

            _context.Entry(personnelLeaveOver).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonnelLeaveOverExists(id))
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


        // POST: api/PersonnelLeaveOvers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public string PostPersonnelLeaveOver(int EmployeeId, [FromBody] PersonnelLeaveOver value)
        //{
        //    if (!_context.PersonnelProfileDetails.Any(a => a.EmployeeId == EmployeeId))
        //    {
        //        return "沒有該員工";
        //    }
        //    //if(!_context.PersonnelProfileDetails.Any(a => a.Sex == Sex))
        //    //{

        //    //    return "沒有性別資料";
        //    //}

        //    //if (Sex == "男性")
        //    //{

        //    //var maleleave = _context.MaleLeaveOver.FromSqlRaw($"EXEC Male_leaveOver {EmployeeId}");

        //    //return maleleave.ToList();
        //    //}
        //    //for (value.LeaveType = 1; value.LeaveType < 4; value.LeaveType++)
        //    //{
        //    //    if (value.LeaveType == 1) { value.Quantity = 112; value.LeaveOver = 112;value.Used = 0; }
        //    //    else if(value.LeaveType == 2) { value.Quantity = 240; value.LeaveOver = 240; value.Used = 0; }
        //    //    else { value.Quantity = 0; value.LeaveOver = 0; value.Used = 0; }
        //        PersonnelLeaveOver insert = new PersonnelLeaveOver
        //        {
        //            LeaveType = value.LeaveType,
        //            Quantity = value.Quantity,
        //            Used = value.Used,
        //            LeaveOver = value.LeaveOver,
        //            EmployeeId = EmployeeId,

        //        };
        //    _context.PersonnelLeaveOvers.Add(insert);
        //    //}
        //    _context.SaveChanges();

        //    return "OK";


        //}


        // POST: api/PersonnelLeaveOvers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<PersonnelLeaveOver>> PostPersonnelLeaveOver(PersonnelLeaveOver personnelLeaveOver)
        //{
        //    _context.PersonnelLeaveOvers.Add(personnelLeaveOver);
        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateException)
        //    {
        //        if (PersonnelLeaveOverExists(personnelLeaveOver.EmployeeId))
        //        {
        //            return Conflict();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return CreatedAtAction("GetPersonnelLeaveOver", new { id = personnelLeaveOver.EmployeeId }, personnelLeaveOver);
        //}

        // DELETE: api/PersonnelLeaveOvers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePersonnelLeaveOver(int id)
        {
            var personnelLeaveOver = await _context.PersonnelLeaveOvers.FindAsync(id);
            if (personnelLeaveOver == null)
            {
                return NotFound();
            }

            _context.PersonnelLeaveOvers.Remove(personnelLeaveOver);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PersonnelLeaveOverExists(int id)
        {
            return _context.PersonnelLeaveOvers.Any(e => e.EmployeeId == id);
        }
    }
}
