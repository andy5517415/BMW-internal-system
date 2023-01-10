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
    public class PersonnelOvertimeFormsController : ControllerBase
    {
        private readonly MSIT44Context _context;

        public PersonnelOvertimeFormsController(MSIT44Context context)
        {
            _context = context;
        }

        // GET: api/PersonnelOvertimeForms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonnelOvertimeForm>>> GetPersonnelOvertimeForms()
        {
            return await _context.PersonnelOvertimeForms.ToListAsync();
        }

        // GET: api/PersonnelOvertimeForms/5/mm
        [HttpGet("{id}/{y}-{m}")]
        public async Task<ActionResult<dynamic>> GetPersonnelOvertime(int id,int y,int m)
        {
            var personnelOvertimeForm = from o in _context.PersonnelOvertimeForms
                                        where o.EmployeeId == id && o.StartDate.Month == m && o.StartDate.Year == y
                                        join p in _context.PersonnelProfileDetails on o.EmployeeId equals p.EmployeeId
                                        select new
                                        {
                                            EmployeeId = o.EmployeeId,
                                            EmployeeName = p.EmployeeName,
                                            EmergencyNumber = p.EmergencyNumber,
                                            StartDate = o.StartDate.ToString("yyyy-MM"),
                                            StartTime = o.StartTime,
                                            EndDate = o.EndDate.ToString("yyyy-MM"),
                                            EndTime = o.EndTime

                                        };

            if (personnelOvertimeForm == null)
            {
                return NotFound();
            }

            return await personnelOvertimeForm.ToListAsync();
        }


        // GET: api/PersonnelOvertimeForms/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PersonnelOvertimeForm>> GetPersonnelOvertimeForm(int id)
        {
            var personnelOvertimeForm = await _context.PersonnelOvertimeForms.FindAsync(id);

            if (personnelOvertimeForm == null)
            {
                return NotFound();
            }

            return personnelOvertimeForm;
        }

        // PUT: api/PersonnelOvertimeForms/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPersonnelOvertimeForm(int id, PersonnelOvertimeForm personnelOvertimeForm)
        {
            if (id != personnelOvertimeForm.StartWorkeId)
            {
                return BadRequest();
            }

            _context.Entry(personnelOvertimeForm).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonnelOvertimeFormExists(id))
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

        // POST: api/PersonnelOvertimeForms
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PersonnelOvertimeForm>> PostPersonnelOvertimeForm(PersonnelOvertimeForm personnelOvertimeForm)
        {
            _context.PersonnelOvertimeForms.Add(personnelOvertimeForm);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPersonnelOvertimeForm", new { id = personnelOvertimeForm.StartWorkeId }, personnelOvertimeForm);
        }

        // DELETE: api/PersonnelOvertimeForms/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePersonnelOvertimeForm(int id)
        {
            var personnelOvertimeForm = await _context.PersonnelOvertimeForms.FindAsync(id);
            if (personnelOvertimeForm == null)
            {
                return NotFound();
            }

            _context.PersonnelOvertimeForms.Remove(personnelOvertimeForm);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PersonnelOvertimeFormExists(int id)
        {
            return _context.PersonnelOvertimeForms.Any(e => e.StartWorkeId == id);
        }
    }
}
