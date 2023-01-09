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
    public class PersonnelDepartmentConnectEmployeeIdsController : ControllerBase
    {
        private readonly MSIT44Context _context;

        public PersonnelDepartmentConnectEmployeeIdsController(MSIT44Context context)
        {
            _context = context;
        }

        // GET: api/PersonnelDepartmentConnectEmployeeIds
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonnelDepartmentConnectEmployeeId>>> GetPersonnelDepartmentConnectEmployeeIds()
        {
            return await _context.PersonnelDepartmentConnectEmployeeIds.ToListAsync();
        }

        // GET: api/PersonnelDepartmentConnectEmployeeIds/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PersonnelDepartmentConnectEmployeeId>> GetPersonnelDepartmentConnectEmployeeId(int id)
        {
            var personnelDepartmentConnectEmployeeId = await _context.PersonnelDepartmentConnectEmployeeIds.FindAsync(id);

            if (personnelDepartmentConnectEmployeeId == null)
            {
                return NotFound();
            }

            return personnelDepartmentConnectEmployeeId;
        }

        // PUT: api/PersonnelDepartmentConnectEmployeeIds/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPersonnelDepartmentConnectEmployeeId(int id, PersonnelDepartmentConnectEmployeeId personnelDepartmentConnectEmployeeId)
        {
            if (id != personnelDepartmentConnectEmployeeId.EmployeeId)
            {
                return BadRequest();
            }

            _context.Entry(personnelDepartmentConnectEmployeeId).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonnelDepartmentConnectEmployeeIdExists(id))
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

        // POST: api/PersonnelDepartmentConnectEmployeeIds
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PersonnelDepartmentConnectEmployeeId>> PostPersonnelDepartmentConnectEmployeeId(PersonnelDepartmentConnectEmployeeId personnelDepartmentConnectEmployeeId)
        {
            _context.PersonnelDepartmentConnectEmployeeIds.Add(personnelDepartmentConnectEmployeeId);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PersonnelDepartmentConnectEmployeeIdExists(personnelDepartmentConnectEmployeeId.EmployeeId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPersonnelDepartmentConnectEmployeeId", new { id = personnelDepartmentConnectEmployeeId.EmployeeId }, personnelDepartmentConnectEmployeeId);
        }

        // DELETE: api/PersonnelDepartmentConnectEmployeeIds/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePersonnelDepartmentConnectEmployeeId(int id)
        {
            var personnelDepartmentConnectEmployeeId = await _context.PersonnelDepartmentConnectEmployeeIds.FindAsync(id);
            if (personnelDepartmentConnectEmployeeId == null)
            {
                return NotFound();
            }

            _context.PersonnelDepartmentConnectEmployeeIds.Remove(personnelDepartmentConnectEmployeeId);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PersonnelDepartmentConnectEmployeeIdExists(int id)
        {
            return _context.PersonnelDepartmentConnectEmployeeIds.Any(e => e.EmployeeId == id);
        }
    }
}
