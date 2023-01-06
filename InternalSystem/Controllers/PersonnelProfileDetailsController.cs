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
