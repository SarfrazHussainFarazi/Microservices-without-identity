using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using GloboTicket.Services.IdentityServerAsmbly.Jwt;
using GloboTicket.Web.Bff.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GloboTicket.Web.Bff.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpContextAccessor _GetCurrentConext;
        
        public AccountController( IHttpContextAccessor IHttpContextAccessor)
        {
          
            _GetCurrentConext = IHttpContextAccessor;

        }
        // GET: AccountController
        public ActionResult Signin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Signin(UsersDto UserDto)
        {
            string baseUrl = "http://localhost:50482/";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            HttpResponseMessage response = client.GetAsync
 ("api/IdentityServer/Login?UserName=" + UserDto.UserName + "&password=" + UserDto.Password).Result;
            string stringJWT = response.Content.ReadAsStringAsync().Result;

          this._GetCurrentConext.HttpContext.Session.SetString("token", stringJWT);
            return RedirectToAction("Index", "EventCatalog");
          
        }
        public ActionResult signup()
        {
            return View();
        }
        [HttpPost]
        public ActionResult signup(UsersDto UserDto)
        {
            string baseUrl = "http://localhost:50482/";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            string stringData = JsonConvert.SerializeObject(UserDto);
            var contentData = new StringContent(stringData,
        System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync
 ("api/IdentityServer/SignUp", contentData).Result;
            string stringJWT = response.Content.ReadAsStringAsync().Result;
            this._GetCurrentConext.HttpContext.Session.SetString("token", stringJWT);
            return RedirectToAction("Index", "EventCatalog");
        }
        // GET: AccountController/Details/5
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("token");
            ViewBag.Message = "User logged out successfully!";
          return  RedirectToAction("signin");
        }
        public ActionResult Unathorized(string msg)
        {
            ViewBag.message = msg;
            return View();
        }
    }
}
