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
    public class PersonnelCityListsController : ControllerBase
    {
        private readonly MSIT44Context _context;

        public PersonnelCityListsController(MSIT44Context context)
        {
            _context = context;
        }

        // GET: api/PersonnelCityLists
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonnelCityList>>> GetPersonnelCityLists()
        {
            return await _context.PersonnelCityLists.ToListAsync();
        }

        // GET: api/PersonnelCityLists/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PersonnelCityList>> GetPersonnelCityList(int id)
        {
            var personnelCityList = await _context.PersonnelCityLists.FindAsync(id);

            if (personnelCityList == null)
            {
                return NotFound();
            }

            return personnelCityList;
        }

        // PUT: api/PersonnelCityLists/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPersonnelCityList(int id, PersonnelCityList personnelCityList)
        {
            if (id != personnelCityList.CityId)
            {
                return BadRequest();
            }

            _context.Entry(personnelCityList).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonnelCityListExists(id))
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

        // POST: api/PersonnelCityLists
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PersonnelCityList>> PostPersonnelCityList(PersonnelCityList personnelCityList)
        {
            _context.PersonnelCityLists.Add(personnelCityList);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PersonnelCityListExists(personnelCityList.CityId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPersonnelCityList", new { id = personnelCityList.CityId }, personnelCityList);
        }

        // DELETE: api/PersonnelCityLists/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePersonnelCityList(int id)
        {
            var personnelCityList = await _context.PersonnelCityLists.FindAsync(id);
            if (personnelCityList == null)
            {
                return NotFound();
            }

            _context.PersonnelCityLists.Remove(personnelCityList);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PersonnelCityListExists(int id)
        {
            return _context.PersonnelCityLists.Any(e => e.CityId == id);
        }
    }
}
