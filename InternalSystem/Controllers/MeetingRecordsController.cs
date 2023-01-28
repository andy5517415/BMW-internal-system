using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InternalSystem.Models;
using static InternalSystem.Controllers.MeetingReservesController;

namespace InternalSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeetingRecordsController : ControllerBase
    {
        private readonly MSIT44Context _context;

        public MeetingRecordsController(MSIT44Context context)
        {
            _context = context;
        }

        // GET: api/MeetingRecords
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MeetingRecord>>> GetMeetingRecords()
        {
            return await _context.MeetingRecords.ToListAsync();
        }

        //先查成功預約的會議室
        // GET: api/MeetingRecords/1
        [HttpGet("{depId}")]
        public async Task<ActionResult<dynamic>> GetMeetingRecords(int depId)
        {

            var meetingReserve = from a in _context.MeetingReserves
                                 join b in _context.MeetingRooms on a.MeetPlaceId equals b.MeetingPlaceId
                                 join c in _context.PersonnelDepartmentLists on a.DepId equals c.DepartmentId
                                 join d in _context.PersonnelProfileDetails on a.EmployeeId equals d.EmployeeId
                                 where a.DepId == depId
                                 select new
                                 {
                                     BookId = a.BookMeetId,
                                     MeetPlace = b.MeetingRoom1,
                                     Date = a.Date,
                                     Dependent = c.DepName,
                                     EmployeeName = d.EmployeeName,
                                     StartTime = a.StartTime,
                                     EndTime = a.EndTime,
                                     MeetType = a.MeetType,
                                 };

            if (meetingReserve == null)
            {
                return NotFound();
            }
            else
            {
                //return testover;
                return await meetingReserve.ToListAsync();
            }

        }


        //最終紀錄的終結果
        // GET: api/MeetingRecords/1(紀錄表編號)/1(會議編號)
        [HttpGet("{1}/{BkId}")]
        public async Task<ActionResult<dynamic>> GetMeetingRecords(int RcId , int BkId)
        {
            var meetingRecords = from a in _context.MeetingRecords
                                 join b in _context.MeetingRooms on a.MeetingPlaceId equals b.MeetingPlaceId
                                 join c in _context.PersonnelDepartmentLists on a.DepId equals c.DepartmentId
                                 where a.BookMeetId == BkId
                                 select new
                                 {
                                    recordSheetId=a.RecordSheetId,
                                    bookId=a.BookMeetId,
                                    meetingPlaceId=b.MeetingPlaceId,
                                    meetPresident=a.MeetPresident,
                                    recorder=a.Rcorder,
                                    participater=a.Participater,
                                    shouldAttend=a.ShouldAttend,
                                    attend=a.Attend,
                                    noAttend=a.NoAttend,
                                    noAttendPerson=a.NoAttendPerson,
                                    Principal  = a.Principal,
                                    date =   a.Date,                    
                                    agenda=a.Agenda,
                                    record=a.Record,
                                    meetingPlace=b.MeetingRoom1,
                                    depment=c.DepName
                                 };

            if (meetingRecords == null)
            {
                return NotFound();
            }
            else
            {
                //return testover;
                return await meetingRecords.ToListAsync();
            }

        }

        // PUT: api/MeetingRecords/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMeetingRecord(int id, MeetingRecord meetingRecord)
        {
            if (id != meetingRecord.RecordSheetId)
            {
                return BadRequest();
            }

            _context.Entry(meetingRecord).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MeetingRecordExists(id))
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

        // POST: api/MeetingRecords
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MeetingRecord>> PostMeetingRecord(MeetingRecord meetingRecord)
        {
            _context.MeetingRecords.Add(meetingRecord);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (MeetingRecordExists(meetingRecord.RecordSheetId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetMeetingRecord", new { id = meetingRecord.RecordSheetId }, meetingRecord);
        }

        // DELETE: api/MeetingRecords/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMeetingRecord(int id)
        {
            var meetingRecord = await _context.MeetingRecords.FindAsync(id);
            if (meetingRecord == null)
            {
                return NotFound();
            }

            _context.MeetingRecords.Remove(meetingRecord);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MeetingRecordExists(int id)
        {
            return _context.MeetingRecords.Any(e => e.RecordSheetId == id);
        }
    }
}
