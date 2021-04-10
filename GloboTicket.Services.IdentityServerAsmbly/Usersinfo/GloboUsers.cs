using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace GloboTicket.Services.IdentityServerAsmbly.Usersinfo
{
    public class GloboUsers
    {
        private readonly IHttpContextAccessor _GetCurrentConext;
        public GloboUsers(IHttpContextAccessor GetCurrentConext)
        {
            this._GetCurrentConext = GetCurrentConext;

        }
        public string GetValue(string ClaimName)
        {

            var claimsIdentity = this._GetCurrentConext.HttpContext.User as ClaimsPrincipal;
            var claim = claimsIdentity?.FindFirst(c => c.Type == ClaimName);
            if (claim != null)
            {
                return claim.Value;
            }
            return string.Empty;
        }
        public  UsersModel GetUser()
        {
            // validation
            if (!this._GetCurrentConext.HttpContext.User.Identity.IsAuthenticated)
            {
                return null;
            }

            return new UsersModel()
            {
                UserId = Convert.ToInt32(GetValue("userId").ToString()),
                Email = GetValue("userId").ToString(),
            
              


            };

        }
    }
}
