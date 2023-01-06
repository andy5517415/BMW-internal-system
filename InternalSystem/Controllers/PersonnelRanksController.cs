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
    public class PersonnelRanksController : ControllerBase
    {
        private readonly MSIT44Context _context;

        public PersonnelRanksController(MSIT44Context context)
        {
            _context = context;
        }

        // GET: api/PersonnelRanks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonnelRank>>> GetPersonnelRanks()
        {
            return await _context.PersonnelRanks.ToListAsync();
        }

        // GET: api/PersonnelRanks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PersonnelRank>> GetPersonnelRank(int id)
        {
            var personnelRank = await _context.PersonnelRanks.FindAsync(id);

            if (personnelRank == null)
            {
                return NotFound();
            }

            return personnelRank;
        }

        // PUT: api/PersonnelRanks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPersonnelRank(int id, PersonnelRank personnelRank)
        {
            if (id != personnelRank.RankId)
            {
                return BadRequest();
            }

            _context.Entry(personnelRank).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonnelRankExists(id))
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

        // POST: api/PersonnelRanks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PersonnelRank>> PostPersonnelRank(PersonnelRank personnelRank)
        {
            _context.PersonnelRanks.Add(personnelRank);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PersonnelRankExists(personnelRank.RankId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPersonnelRank", new { id = personnelRank.RankId }, personnelRank);
        }

        // DELETE: api/PersonnelRanks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePersonnelRank(int id)
        {
            var personnelRank = await _context.PersonnelRanks.FindAsync(id);
            if (personnelRank == null)
            {
                return NotFound();
            }

            _context.PersonnelRanks.Remove(personnelRank);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PersonnelRankExists(int id)
        {
            return _context.PersonnelRanks.Any(e => e.RankId == id);
        }
    }
}
