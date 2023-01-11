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

        //找尋該員工假別1
        // GET: api/PersonnelLeaveOvers/leave1/5
        [HttpGet("leave1/{id}")]
        public async Task<ActionResult<dynamic>> GetLeaveOver(int id )
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

        //找尋該員工假別1
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

        //找尋該員工假別1
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
        [HttpPost]
        public async Task<ActionResult<PersonnelLeaveOver>> PostPersonnelLeaveOver(PersonnelLeaveOver personnelLeaveOver)
        {
            _context.PersonnelLeaveOvers.Add(personnelLeaveOver);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PersonnelLeaveOverExists(personnelLeaveOver.EmployeeId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPersonnelLeaveOver", new { id = personnelLeaveOver.EmployeeId }, personnelLeaveOver);
        }

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
