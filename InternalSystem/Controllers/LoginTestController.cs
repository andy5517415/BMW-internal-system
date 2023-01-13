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
        
        public object Login(LoginPost value)
        {
            var user = (from a in _context.PersonnelProfileDetails //找員工資料表
                        where a.Acount == value.Account  //帳號
                        && a.Password == value.Password  //密碼
                        select a).SingleOrDefault();  //帳號唯一值

            //這邊不null判斷了，直接報錯
            if (user == null)
            {
                return "帳號密碼錯誤";
            }
            else
            { //寫驗證
                var claims = new List<Claim>
                {
                    //登入成功獲取使用者資訊(似乎只能為strimg)
                    new Claim("EmployeeId", user.EmployeeId.ToString()),  //流水號
                    new Claim(ClaimTypes.Name, user.Acount),  //ID工號
                    new Claim("EmployeeName", user.EmployeeName), //使用者名字
                    new Claim("DepartmentId", user.DepartmentId.ToString()),  //部門ID
                    new Claim("RankId", user.RankId.ToString()),  //RankID
                    //new Claim(ClaimTypes.Role, "select")  //權限
                };

                //建構
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                //跟他說要用cookie驗證，若執行到這一行，表示使用者已登入
                //SignIn
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));


                //登入後，將使用者資訊存進去
                LoginIfo LoginIo = new LoginIfo()
                {
                    state = "使用者已登入",
                    DepartmentId = user.DepartmentId,
                    EmployeeId = user.EmployeeId,
                    EmployeeName = user.EmployeeName,
                    EmployeeNumber = user.EmployeeNumber,
                    RankId = user.RankId,
                };
                
                return LoginIo;
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
        public int RankId { get; set; }
    }
}
