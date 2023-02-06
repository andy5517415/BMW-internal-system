using InternalSystem.Dotos;
using InternalSystem.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace InternalSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[AllowAnonymous] //僅開放登入頁面不需驗證
    public class LoginTestController : ControllerBase
    {
        private readonly MSIT44Context _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LoginTestController(MSIT44Context context) 
        {
            _context = context;
        }

        // api/LoginTest
        [HttpPost]
        
        public async Task<ActionResult<dynamic>> Login(LoginPost value)
        {
            var user = from a in _context.PersonnelProfileDetails //找員工資料表
                       join b in _context.PersonnelDepartmentLists on a.DepartmentId equals b.DepartmentId
                       join c in _context.PersonnelPositions on a.PositionId equals c.PositionId
                       where a.Acount == value.Account  //帳號
                       && a.Password == value.Password  //密碼
                       select new
                       {
                           state = "使用者已登入",
                           DepartmentId = a.DepartmentId,
                           EmployeeId = a.EmployeeId,
                           EmployeeName = a.EmployeeName,
                           EmployeeNumber = a.EmployeeNumber,
                           PositionId = a.PositionId,
                           Depment = b.DepName,
                           PositionName =c.PositionName,
                           Photo = a.Photo
                       };

            //這邊不null判斷了，直接報錯
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                return await user.ToListAsync();
            }
        }
        

        //登出
        [HttpDelete]
        public string logout()
        {
            //SignOut 使用cookie的資訊登出
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return "已登出";
        }
        //未登入
        [HttpGet("NoLogin")]
        public string noLogin()
        {
            return "未登入";
        }
        /*//沒權限
        [HttpGet("NoAccess")]
        public string noAccess()
        {
            return "沒權限啦";
        }
        */
    }

    public class LoginIfo {
        public string state {get;set;}
        public int EmployeeId { get; set; }
        public string EmployeeNumber { get; set; }
        public string EmployeeName { get; set; }
        public int DepartmentId { get; set; }
        public int PositionId { get; set; }
        public string Department { get; set; }
    }
}
