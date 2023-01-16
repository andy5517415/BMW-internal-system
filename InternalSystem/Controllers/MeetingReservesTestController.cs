using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InternalSystem.Models;
using System.Security.Principal;
using System.Runtime.Intrinsics.X86;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InternalSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeetingReservesTestController : ControllerBase
    {
        private readonly MSIT44Context _context;

        public MeetingReservesTestController(MSIT44Context context)
        {
            _context = context;
        }
        
        // GET: api/<MeetingReservesTestController>
        [HttpGet]
        
        /*
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        */
        // GET api/<MeetingReservesTestController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<MeetingReservesTestController>
        [HttpPost]

        public string test() {
            return "123";
        }

        /*
        public object MeetReserver(MeetingGet value)
        {
            var user = (from a in _context.PersonnelProfileDetails //找員工資料表
                        where a.Acount == value.Account  //帳號
                        && a.Password == value.Password  //密碼
                        select a).SingleOrDefault();  //帳號唯一值
            
            var Search = (from a in _context.MeetingReserves //找員工資料表
                          where a.StartTime == value.Startday //開始時間
                            && a.EndTime == value.Endday//結束時間
                            && a.DepId == value.Dep  //部門
                          select a);
            

            if (user == null)
            {
                return "沒有資料";
            }
            else
            {
                return user;
            }
        }

        /*
        public void Post([FromBody] string value)
        {
        }
        */

        // PUT api/<MeetingReservesTestController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<MeetingReservesTestController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        public class MeetingGet{
            public string Account { get; set; }
            public string Password { get; set; }

            /*
            public DateTime Startday { get; set; }
            public DateTime Endday { get; set; }
            public int Dep { get; set; }
            */
        }

        public class MeetRInfo 
        {
            
        }

    }
}
