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
    public class PersonnelLeaveTypesController : ControllerBase
    {
        private readonly MSIT44Context _context;

        public PersonnelLeaveTypesController(MSIT44Context context)
        {
            _context = context;
        }

        // GET: api/PersonnelLeaveTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonnelLeaveType>>> GetPersonnelLeaveTypes()
        {
            return await _context.PersonnelLeaveTypes.ToListAsync();
        }

        // GET: api/PersonnelLeaveTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PersonnelLeaveType>> GetPersonnelLeaveType(int id)
        {
            var personnelLeaveType = await _context.PersonnelLeaveTypes.FindAsync(id);

            if (personnelLeaveType == null)
            {
                return NotFound();
            }

            return personnelLeaveType;
        }

        // PUT: api/PersonnelLeaveTypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPersonnelLeaveType(int id, PersonnelLeaveType personnelLeaveType)
        {
            if (id != personnelLeaveType.LeaveTypeId)
            {
                return BadRequest();
            }

            _context.Entry(personnelLeaveType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonnelLeaveTypeExists(id))
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

        // POST: api/PersonnelLeaveTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PersonnelLeaveType>> PostPersonnelLeaveType(PersonnelLeaveType personnelLeaveType)
        {
            _context.PersonnelLeaveTypes.Add(personnelLeaveType);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PersonnelLeaveTypeExists(personnelLeaveType.LeaveTypeId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPersonnelLeaveType", new { id = personnelLeaveType.LeaveTypeId }, personnelLeaveType);
        }

        // DELETE: api/PersonnelLeaveTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePersonnelLeaveType(int id)
        {
            var personnelLeaveType = await _context.PersonnelLeaveTypes.FindAsync(id);
            if (personnelLeaveType == null)
            {
                return NotFound();
            }

            _context.PersonnelLeaveTypes.Remove(personnelLeaveType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PersonnelLeaveTypeExists(int id)
        {
            return _context.PersonnelLeaveTypes.Any(e => e.LeaveTypeId == id);
        }
    }
}
