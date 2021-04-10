using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GloboTicket.Services.IdentityServer.DbContexts;
using GloboTicket.Services.IdentityServer.Entities;
using GloboTicket.Services.IdentityServer.Models;
using GloboTicket.Services.IdentityServerAsmbly.Jwt;
using GloboTicket.Services.IdentityServerAsmbly.Usersinfo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GloboTicket.Services.IdentityServer.Controlles
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class IdentityServerController : ControllerBase
    {
        private readonly IdentityDbContext db;
        private readonly IJwtBuilder _jwtBuilder;
        private readonly GloboUsers _AircodUsers;
        private readonly IWebHostEnvironment _IWebHostEnvironment;
        public IdentityServerController(IJwtBuilder jwtBuilder, GloboUsers AircodUsers, IWebHostEnvironment IWebHostEnvironment, IdentityDbContext _db)
        {
            this._jwtBuilder = jwtBuilder;
            this._AircodUsers = AircodUsers;
            this._IWebHostEnvironment = IWebHostEnvironment;
            this.db = _db;

        }


       
        [HttpGet]
        public ActionResult<string> Login(string UserName, String password)
        {
            var Token = "username or password wrong";
            if (UserName != "" && password != "")
            {
                var getUser = db.Users.FirstOrDefault(x => x.UserName == UserName && x.Password == password);
                if (getUser !=null)
                {
                    UsersDto userObj = new UsersDto();
                    userObj.Email = getUser.Email;
                    userObj.Password = getUser.Password;
                    userObj.RoleName = getUser.RoleName;
                    userObj.UserName = getUser.UserName;
                    userObj.UserId = getUser.UserId;
                     Token = this._jwtBuilder.GetToken(userObj);
                }
            }
           
            
            return Ok(Token);
        }
        [HttpPost]
        public ActionResult<string> SignUp(UsersDto _UsersDto)
        {
            Users userObj = new Users();
            userObj.Email = _UsersDto.Email;
            userObj.Password = _UsersDto.Password;
            userObj.RoleName = _UsersDto.RoleName;
            userObj.UserName = _UsersDto.UserName;

            db.Users.Add(userObj);
            db.SaveChanges();
            var Token = this._jwtBuilder.GetToken(_UsersDto);
            return Ok(Token);
        }
        [Authorize]
        [HttpGet]
        public ActionResult<string> CheckIsLoginOrNot()
        {
            var getuserid = this._AircodUsers.GetValue("userId");
            var claimsIdentity = HttpContext.User as ClaimsPrincipal;
            var claim = claimsIdentity?.FindFirst(c => c.Type == "userId");
            if (claim != null)
            {
                return Ok("authorized");
            }


            return Ok("authorized");
        }
    }
}
