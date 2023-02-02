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

        //有紀錄的才會顯示
        // GET: api/MeetingRecords/1(部門編號)
        [HttpGet("Rc/{depId}")]
        public async Task<ActionResult<dynamic>> GetMeetingRecords_DepId(int depId)
        {
            var meetingBookMeetId = from a in _context.MeetingRecords
                                    join b in _context.MeetingReserves on a.BookMeetId equals b.BookMeetId
                                    join c in _context.MeetingRooms on b.MeetPlaceId equals c.MeetingPlaceId
                                    join d in _context.PersonnelDepartmentLists on b.DepId equals d.DepartmentId
                                    join e in _context.PersonnelProfileDetails on b.EmployeeId equals e.EmployeeId
                                    where b.DepId == depId
                                    orderby b.Date, b.StartTime
                                    select new
                                    {
                                        BookId = a.BookMeetId,
                                        MeetPlace = c.MeetingRoom1,
                                        Date = b.Date,
                                        Dependent = d.DepName,
                                        EmployeeName = e.EmployeeName,
                                        StartTime = b.StartTime,
                                        EndTime = b.EndTime,
                                        MeetType = b.MeetType,
                                    };

            if (meetingBookMeetId == null)
            {
                return NotFound();
            }
            else
            {
                //return testover;
                return await meetingBookMeetId.ToListAsync();
            }
        }

        //BookID有才會回傳
        // GET: api/MeetingRecords/1
        [HttpGet("BkId/{BookMeetId}")]
        public async Task<ActionResult<dynamic>> GetMeetingRecords_BookMeetId(int BookMeetId)
        {
            var meetingBookMeetId = from a in _context.MeetingRecords
                                    where a.BookMeetId == BookMeetId
                                    select new
                                    {
                                        BookMeetId = a.BookMeetId,
                                    };

            if (meetingBookMeetId == null)
            {
                return NotFound();
            }
            else
            {
                //return testover;
                return await meetingBookMeetId.ToListAsync();
            }
        }




        //最終紀錄的終結果
        // GET: api/MeetingRecords/1(紀錄表編號)/1(會議編號)
        [HttpGet("Record/{BkId}")]
        public async Task<ActionResult<dynamic>> GetMeetingRecords(int RcId, int BkId)
        {
            var meetingRecords = from a in _context.MeetingRecords
                                 where a.BookMeetId == BkId
                                 select new
                                 {
                                     recordSheetId = a.RecordSheetId,
                                     bookId = a.BookMeetId,
                                     meetPresident = a.MeetPresident,
                                     recorder = a.Rcorder,
                                     participater = a.Participater,
                                     shouldAttend = a.ShouldAttend,
                                     attend = a.Attend,
                                     noAttend = a.NoAttend,
                                     noAttendPerson = a.NoAttendPerson,
                                     date = a.Date,
                                     agenda = a.Agenda,
                                     record = a.Record,
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

        //全部查詢
        public async Task<ActionResult<IEnumerable<MeetingRecord>>> GetMeetingRecords()
        {
            return await _context.MeetingRecords.ToListAsync();
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


        //建立會議記錄
        // POST: api/BusinessOrderDetails/1(BookMeetId)
        [HttpPost("{BookMeetId}")]
        public string PostMeetingRecords(int BookMeetId, [FromBody] MeetingRecord b)
        {
            //判斷外鍵關聯存不存在
            if (!_context.MeetingReserves.Any(a => a.BookMeetId == BookMeetId))
            {
                return "沒有該筆資料";
            }
            //判斷裡面是不是已經有資料了(有該外鍵)
            if (_context.MeetingRecords.Any(a => a.BookMeetId == BookMeetId))
            {
                return "已有資料，不可重複寫入!";
            }

            MeetingRecord insert = new MeetingRecord
            {
                BookMeetId = BookMeetId,
                MeetPresident = b.MeetPresident,
                Rcorder = b.Rcorder,
                Participater = b.Participater,
                ShouldAttend = b.ShouldAttend,
                Attend = b.Attend,
                NoAttend = b.NoAttend,
                NoAttendPerson = b.NoAttendPerson,
                Date = DateTime.Now,
                Agenda = b.Agenda,
                Record = b.Record,
            };

            _context.MeetingRecords.Add(insert);
            _context.SaveChanges();

            return "會議記錄新增成功";
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
