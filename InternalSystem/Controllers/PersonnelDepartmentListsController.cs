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
    public class PersonnelDepartmentListsController : ControllerBase
    {
        private readonly MSIT44Context _context;

        public PersonnelDepartmentListsController(MSIT44Context context)
        {
            _context = context;
        }

        // GET: api/PersonnelDepartmentLists
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonnelDepartmentList>>> GetPersonnelDepartmentLists()
        {
            return await _context.PersonnelDepartmentLists.ToListAsync();
        }

        // GET: api/PersonnelDepartmentLists/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PersonnelDepartmentList>> GetPersonnelDepartmentList(int id)
        {
            var personnelDepartmentList = await _context.PersonnelDepartmentLists.FindAsync(id);

            if (personnelDepartmentList == null)
            {
                return NotFound();
            }

            return personnelDepartmentList;
        }

        // PUT: api/PersonnelDepartmentLists/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPersonnelDepartmentList(int id, PersonnelDepartmentList personnelDepartmentList)
        {
            if (id != personnelDepartmentList.DepartmentId)
            {
                return BadRequest();
            }

            _context.Entry(personnelDepartmentList).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonnelDepartmentListExists(id))
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

        // POST: api/PersonnelDepartmentLists
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PersonnelDepartmentList>> PostPersonnelDepartmentList(PersonnelDepartmentList personnelDepartmentList)
        {
            _context.PersonnelDepartmentLists.Add(personnelDepartmentList);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PersonnelDepartmentListExists(personnelDepartmentList.DepartmentId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPersonnelDepartmentList", new { id = personnelDepartmentList.DepartmentId }, personnelDepartmentList);
        }

        // DELETE: api/PersonnelDepartmentLists/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePersonnelDepartmentList(int id)
        {
            var personnelDepartmentList = await _context.PersonnelDepartmentLists.FindAsync(id);
            if (personnelDepartmentList == null)
            {
                return NotFound();
            }

            _context.PersonnelDepartmentLists.Remove(personnelDepartmentList);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PersonnelDepartmentListExists(int id)
        {
            return _context.PersonnelDepartmentLists.Any(e => e.DepartmentId == id);
        }
    }
}
