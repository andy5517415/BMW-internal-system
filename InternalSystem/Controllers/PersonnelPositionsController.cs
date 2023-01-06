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
    public class PersonnelPositionsController : ControllerBase
    {
        private readonly MSIT44Context _context;

        public PersonnelPositionsController(MSIT44Context context)
        {
            _context = context;
        }

        // GET: api/PersonnelPositions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonnelPosition>>> GetPersonnelPositions()
        {
            return await _context.PersonnelPositions.ToListAsync();
        }

        // GET: api/PersonnelPositions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PersonnelPosition>> GetPersonnelPosition(int id)
        {
            var personnelPosition = await _context.PersonnelPositions.FindAsync(id);

            if (personnelPosition == null)
            {
                return NotFound();
            }

            return personnelPosition;
        }

        // PUT: api/PersonnelPositions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPersonnelPosition(int id, PersonnelPosition personnelPosition)
        {
            if (id != personnelPosition.PositionId)
            {
                return BadRequest();
            }

            _context.Entry(personnelPosition).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonnelPositionExists(id))
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

        // POST: api/PersonnelPositions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PersonnelPosition>> PostPersonnelPosition(PersonnelPosition personnelPosition)
        {
            _context.PersonnelPositions.Add(personnelPosition);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PersonnelPositionExists(personnelPosition.PositionId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPersonnelPosition", new { id = personnelPosition.PositionId }, personnelPosition);
        }

        // DELETE: api/PersonnelPositions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePersonnelPosition(int id)
        {
            var personnelPosition = await _context.PersonnelPositions.FindAsync(id);
            if (personnelPosition == null)
            {
                return NotFound();
            }

            _context.PersonnelPositions.Remove(personnelPosition);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PersonnelPositionExists(int id)
        {
            return _context.PersonnelPositions.Any(e => e.PositionId == id);
        }
    }
}
