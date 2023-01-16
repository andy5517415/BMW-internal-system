using InternalSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternalSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeetingReservesController : ControllerBase
    {
        private readonly MSIT44Context _context;

        public MeetingReservesController(MSIT44Context context)
        {
            _context = context;
        }

        // GET: api/MeetingReserves
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MeetingReserve>>> GetMeetingReserves()
        {
            return await _context.MeetingReserves.ToListAsync();
        }

        // GET: api/MeetingReserves/5
        [HttpGet("{depId}/{s}/{e}")]
         public async Task<ActionResult<dynamic>> GetMeetingReserve(int depId,int s, int e)
         {
             var sd =s.ToString();
             var ed =e.ToString();

             var sday = DateTime.Parse(sd.Substring(0,4)+"/"+ sd.Substring(4, 2) + "/"+ sd.Substring(6, 2));
             var eday = DateTime.Parse(ed.Substring(0, 4) + "/" + ed.Substring(4, 2) + "/" + ed.Substring(6, 2));

            test testover = new test()
             {
                sday = sday,
                eday = eday
            };

             var meetingReserve = from a in _context.MeetingReserves
                                  where a.Date >= sday && a.Date <= eday &&
                                  a.DepId== depId
                                  select a;

             if (meetingReserve == null)
             {
                 return NotFound();
             }
             else {
                 //return testover;
                 return await meetingReserve.ToListAsync();
             }

         }
         
        // POST: api/MeetingReserves
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MeetingReserve>> PostMeetingReserve(MeetingReserve meetingReserve)
        {
            _context.MeetingReserves.Add(meetingReserve);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (MeetingReserveExists(meetingReserve.BookMeetId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetMeetingReserve", new { id = meetingReserve.BookMeetId }, meetingReserve);
        }

        // DELETE: api/MeetingReserves/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMeetingReserve(int id)
        {
            var meetingReserve = await _context.MeetingReserves.FindAsync(id);
            if (meetingReserve == null)
            {
                return NotFound();
            }

            _context.MeetingReserves.Remove(meetingReserve);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MeetingReserveExists(int id)
        {
            return _context.MeetingReserves.Any(e => e.BookMeetId == id);
        }

        public class test
        {
            public DateTime sday { get; set; }
            public DateTime eday { get; set; }

        }

        public class MeetReserveInfo
        {
            public string MeetPlaceId { get; set; }
            public string Date { get; set; }
            public string DepId { get; set; }
            public string RecorderId { get; set; }
            public string StarTime{ get; set; }
            public string EndTime { get; set; }

        }
    }
}
