using InternalSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Policy;
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
        [HttpGet("{s}-{e}")]
        public object GetMeetingReserves(string s,string e)
        {
            int stime=Convert.ToInt32(s.Substring(0,2));
            int etime = Convert.ToInt32(e.Substring(0, 2));

            var timeZone = from a in _context.MeetingReserves
                           select new
                           {
                               startTime=a.StartTime,
                               endTime=a.EndTime,

                           };
            var timeZone2 = timeZone.ToArray();

            int[] startTime = new int[timeZone2.Length];
            int[] endTime = new int[timeZone2.Length];
            for (int i = 0; i < startTime.Length; i++)
            {
                startTime[i] = Convert.ToInt32( timeZone2[i].startTime.Substring(0,2));
                endTime[i] = Convert.ToInt32(timeZone2[i].endTime.Substring(0, 2));
            }

            for (int i = 0; i < endTime.Length; i++)
            {
                if (stime < endTime[i])
                {
                    return "我比你小";
                }
            }
           

            return endTime;
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

        //查預約會議室(部門版本)
        // GET: api/MeetingReserves/1
        [HttpGet("{depId}")]
        public async Task<ActionResult<dynamic>> GetMeetingReserve(int depId)
        {
            var meetingReserve = from a in _context.MeetingReserves
                                 join b in _context.MeetingRooms on a.MeetPlaceId equals b.MeetingPlaceId
                                 join c in _context.PersonnelDepartmentLists on a.DepId equals c.DepartmentId
                                 join d in _context.PersonnelProfileDetails on a.EmployeeId equals d.EmployeeId
                                 where  a.DepId == depId
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

        //查預約會議室(會議室編號版本)
        // GET: api/MeetingReserves/MeetRoom/1
        [HttpGet("MeetRoom/{placeId}")]
        public async Task<ActionResult<dynamic>> GetMeetingReserve_MeetRoom(int placeId)
        {
            var meetingReserve = from a in _context.MeetingReserves
                                 join b in _context.MeetingRooms on a.MeetPlaceId equals b.MeetingPlaceId
                                 join c in _context.PersonnelDepartmentLists on a.DepId equals c.DepartmentId
                                 join d in _context.PersonnelProfileDetails on a.EmployeeId equals d.EmployeeId
                                 where a.MeetPlaceId == placeId
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

        // POST: api/MeetingReserves/2/2023-01-05/15:00-16:00
        [HttpPost("{DepId}/{date}/{s}-{e}")]

        public ActionResult PostMeetingReserves(int DepId, string date, string s,string e,[FromBody] MeetingReserve form)
        {
            TimeSpan hm = Convert.ToDateTime(e).Subtract(Convert.ToDateTime(s)); //兩個相減
            double hoursCount = hm.TotalMinutes;  //所有的分鐘

            var dateT = DateTime.Parse(date);

            int star2 = Convert.ToInt32(s.Substring(0, 2));
            int end2 = Convert.ToInt32(e.Substring(0, 2));

            var timeZone = from a in _context.MeetingReserves
                           select new
                           {
                               startTime = a.StartTime,
                               endTime = a.EndTime,

                           };
            var timeZone2 = timeZone.ToArray();

            int[] startTime = new int[timeZone2.Length];
            int[] endTime = new int[timeZone2.Length];
            for (int i = 0; i < startTime.Length; i++)
            {
                startTime[i] = Convert.ToInt32(timeZone2[i].startTime.Substring(0, 2));
                endTime[i] = Convert.ToInt32(timeZone2[i].endTime.Substring(0, 2));
            }

            //判斷結束時間不能小於起始時間
            if (hoursCount >0)
            {
                //若為同一天
                var sameDay = from a in _context.MeetingReserves
                               where dateT == a.Date
                               select a;
                if (sameDay != null) //就是同一天
                {
                    for (int i = 0; i < startTime.Length; i++)
                    {
                        //時區重複判斷
                        if (star2 >= startTime[i] && star2 <= endTime[i] || end2 >= startTime[i] && end2 <= endTime[i]
                                         || startTime[i] >= star2 && startTime[i] <end2)
                        {
                            return Content("選擇時段已有人預約");
                        }
                        else
                        {
                            MeetingReserve insert = new MeetingReserve
                            {
                                DepId = DepId,
                                MeetPlaceId = form.MeetPlaceId,
                                Date = form.Date,
                                EmployeeId = form.EmployeeId,
                                StartTime = form.StartTime,
                                EndTime = form.StartTime,
                                MeetType = form.MeetType,
                            };
                            _context.MeetingReserves.Add(insert);
                            _context.SaveChanges();
                            return Content("預約成功");
                        }
                    }
                    return NoContent();
                }
                else
                {
                    MeetingReserve insert = new MeetingReserve
                    {
                        DepId = DepId,
                        MeetPlaceId = form.MeetPlaceId,
                        Date = form.Date,
                        EmployeeId = form.EmployeeId,
                        StartTime = form.StartTime,
                        EndTime = form.StartTime,
                        MeetType = form.MeetType,
                    };
                    _context.MeetingReserves.Add(insert);
                    _context.SaveChanges();
                    return Content("預約成功");
                }
            }
            else
            {
                return Content("選擇預約時間錯誤!");
            }
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

        private bool MeetingReserveExists(int BookId)
        {
            return _context.PersonnelDepartmentLists.Any(e => e.DepartmentId == BookId);
        }
        
    }
}
