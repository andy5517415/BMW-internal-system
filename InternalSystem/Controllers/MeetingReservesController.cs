using InternalSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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
        [HttpGet("RM/{placeId}")]
        
        public object GetMeetingReserves3(int placeId)
        {
            var timeZone = from a in _context.MeetingReserves
                           where a.MeetPlaceId == placeId
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
                startTime[i] = Convert.ToInt32(timeZone2[i].startTime.Substring(0, 2) + timeZone2[i].startTime.Substring(3, 2));
                endTime[i] = Convert.ToInt32(timeZone2[i].endTime.Substring(0, 2) + timeZone2[i].endTime.Substring(3, 2));
            }
            return startTime;
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
                                  orderby a.Date , a.StartTime
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
                                 orderby a.Date, a.StartTime
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

        //查預約會議室
        // GET: api/MeetingReserves/1/20230101/20230106
        [HttpGet("RM/{placeId}/{s}/{e}")]
        public async Task<ActionResult<dynamic>> GetMeetingReserve2(int placeId, int s, int e)
        {
            var sd = s.ToString();
            var ed = e.ToString();

            var sday = DateTime.Parse(sd.Substring(0, 4) + "/" + sd.Substring(4, 2) + "/" + sd.Substring(6, 2));
            var eday = DateTime.Parse(ed.Substring(0, 4) + "/" + ed.Substring(4, 2) + "/" + ed.Substring(6, 2));

            var meetingReserve = from a in _context.MeetingReserves
                                 join b in _context.MeetingRooms on a.MeetPlaceId equals b.MeetingPlaceId
                                 join c in _context.PersonnelDepartmentLists on a.DepId equals c.DepartmentId
                                 join d in _context.PersonnelProfileDetails on a.EmployeeId equals d.EmployeeId
                                 where a.Date >= sday && a.Date <= eday && a.MeetPlaceId == placeId
                                 orderby a.Date, a.StartTime
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

        // POST: api/MeetingReserves/2/1/2023-01-05/15:00-16:00
        [HttpPost("{DepId}/{placeId}/{date}/{s}-{e}")]

        public ActionResult PostMeetingReserves(int DepId, int placeId, string date, string s,string e,[FromBody] MeetingReserve form)
        {
            TimeSpan hm = Convert.ToDateTime(e).Subtract(Convert.ToDateTime(s)); //兩個相減
            double hoursCount = hm.TotalMinutes;  //所有的分鐘

            var dateT = DateTime.Parse(date);
            s = s.Substring(0, 2) + s.Substring(3, 2);
            e = e.Substring(0, 2) + e.Substring(3, 2);
            int stime = Convert.ToInt32(s);
            int etime = Convert.ToInt32(e);
            int state = 0;

            var timeZone = from a in _context.MeetingReserves
                           where a.MeetPlaceId == placeId
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
                startTime[i] = Convert.ToInt32(timeZone2[i].startTime.Substring(0, 2)+ timeZone2[i].startTime.Substring(3, 2));
                endTime[i] = Convert.ToInt32(timeZone2[i].endTime.Substring(0, 2)+ timeZone2[i].endTime.Substring(3, 2));
            }

            //判斷結束時間不能小於起始時間
            if (hoursCount >0)
            {
                //若為同一天
                var sameDay = from a in _context.MeetingReserves
                               where dateT == a.Date
                               select a;
                if (sameDay.ToArray().Length !=0) //就是同一天
                {
                    for (int i = 0; i < startTime.Length; i++)
                    {
                        //時區重複判斷
                        if (stime >= startTime[i] && etime <= endTime[i] || etime >= startTime[i] && etime <= endTime[i]
                    || startTime[i] >= stime && startTime[i] <= etime)
                        {
                            state += 1;
                        }
                    }
                    if (state == 0)
                    {
                        MeetingReserve insert = new MeetingReserve
                        {
                            DepId = DepId,
                            MeetPlaceId = form.MeetPlaceId,
                            Date = form.Date,
                            EmployeeId = form.EmployeeId,
                            StartTime = form.StartTime,
                            EndTime = form.EndTime,
                            MeetType = form.MeetType,
                        };
                        _context.MeetingReserves.Add(insert);
                        _context.SaveChanges();
                        return Content("預約成功!");
                    }
                    else
                    {
                        return Content("選擇時段已有人預約!");
                    }
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
                        EndTime = form.EndTime,
                        MeetType = form.MeetType,
                    };
                    _context.MeetingReserves.Add(insert);
                    _context.SaveChanges();
                    return Content("預約成功!");
                }
            }
            else
            {
                return Content("結束時間要小於開始時間!");
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
