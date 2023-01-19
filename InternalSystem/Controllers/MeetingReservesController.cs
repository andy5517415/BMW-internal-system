using InternalSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

        //查預約會議室
        // GET: api/MeetingReserves/1/20230101/20230106
        [HttpGet("{depId}/{s}/{e}")]
         public async Task<ActionResult<dynamic>> GetMeetingReserve(int depId,int s, int e)
         {
             var sd =s.ToString();
             var ed =e.ToString();

             var sday = DateTime.Parse(sd.Substring(0,4)+"/"+ sd.Substring(4, 2) + "/"+ sd.Substring(6, 2));
             var eday = DateTime.Parse(ed.Substring(0, 4) + "/" + ed.Substring(4, 2) + "/" + ed.Substring(6, 2));

             var meetingReserve = from a in _context.MeetingReserves
                                  join b in _context.MeetingRooms on a.MeetPlaceId equals b.MeetingPlaceId
                                  join c in _context.PersonnelDepartmentLists on a.DepId equals c.DepartmentId
                                  join d in _context.PersonnelProfileDetails on a.EmployeeId equals d.EmployeeId
                                  where a.Date >= sday && a.Date <= eday && a.DepId== depId
                                  select new {
                                      BookId= a.BookMeetId,
                                      MeetPlace = b.MeetingRoom1,
                                      Date = a.Date,
                                      Dependent =c.DepName,
                                      EmployeeName = d.EmployeeName,
                                      StartTime = a.StartTime,
                                      EndTime = a.EndTime,
                                      MeetType= a.MeetType,
                                  };

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
        [HttpDelete("{BookId}")]
        public void Delete(int BookId)
        {
            var delete = (from a in _context.MeetingReserves
                          where a.BookMeetId == BookId
                          select a).Include(c => c.MeetingRecords).SingleOrDefault(); //預約會議被刪除，同時記錄表也會被刪除(以防萬一)

            if (delete != null)
            {
                _context.MeetingReserves.Remove(delete);
                _context.SaveChanges();
            }
        }

        /*
        public async Task<IActionResult> DeleteMeetingReserve(int BookId)
        {
            
            var delete = (from a in _context.PersonnelDepartmentLists
                          where a.DepartmentId == BookId
                          select a).Include(c => c.PersonnelProfileDetails).SingleOrDefault();

            if (delete != null)
            {
                _context.PersonnelDepartmentLists.Remove(delete);
                _context.SaveChanges();
            }
            
            var meetingReserve = await _context.PersonnelDepartmentLists.FindAsync(BookId);
            if (meetingReserve == null)
            {
                return NotFound();
            }

            _context.PersonnelDepartmentLists.Remove(meetingReserve);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        */
        private bool MeetingReserveExists(int BookId)
        {
            return _context.PersonnelDepartmentLists.Any(e => e.DepartmentId == BookId);
        }
        
    }
}
