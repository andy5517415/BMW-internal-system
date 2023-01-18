using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InternalSystem.Models;
using Microsoft.CodeAnalysis;

namespace InternalSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessOptionalsController : ControllerBase
    {
        private readonly MSIT44Context _context;

        public BusinessOptionalsController(MSIT44Context context)
        {
            _context = context;
        }






        //category分流
        // GET: api/BusinessOptionals/info/1
        //[ActionName("info")]
        [HttpGet("info/{idx}")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetType(int idx)
        {

            var q = from o in _context.BusinessOptionals
                    where o.CategoryId == idx
                    select o;
            return await q.ToListAsync();
        }

        //取得代理商位置
        // GET: api/BusinessOptionals/agent
        [HttpGet("agent")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetAgent()
        {
            var q = from o in _context.BusinessAreas
                    select o;
            return await q.ToListAsync();
        }













        // GET: api/BusinessOptionals
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BusinessOptional>>> GetBusinessOptionals()
        {
            return await _context.BusinessOptionals.ToListAsync();
        }

        // GET: api/BusinessOptionals/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BusinessOptional>> GetBusinessOptional(int id)
        {
            var businessOptional = await _context.BusinessOptionals.FindAsync(id);

            if (businessOptional == null)
            {
                return NotFound();
            }

            return businessOptional;
        }

        // PUT: api/BusinessOptionals/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBusinessOptional(int id, BusinessOptional businessOptional)
        {
            if (id != businessOptional.OptionalId)
            {
                return BadRequest();
            }

            _context.Entry(businessOptional).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BusinessOptionalExists(id))
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

        // POST: api/BusinessOptionals
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BusinessOptional>> PostBusinessOptional(BusinessOptional businessOptional)
        {
            _context.BusinessOptionals.Add(businessOptional);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (BusinessOptionalExists(businessOptional.OptionalId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetBusinessOptional", new { id = businessOptional.OptionalId }, businessOptional);
        }

        // DELETE: api/BusinessOptionals/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBusinessOptional(int id)
        {
            var businessOptional = await _context.BusinessOptionals.FindAsync(id);
            if (businessOptional == null)
            {
                return NotFound();
            }

            _context.BusinessOptionals.Remove(businessOptional);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BusinessOptionalExists(int id)
        {
            return _context.BusinessOptionals.Any(e => e.OptionalId == id);
        }
    }
}
