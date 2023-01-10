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
