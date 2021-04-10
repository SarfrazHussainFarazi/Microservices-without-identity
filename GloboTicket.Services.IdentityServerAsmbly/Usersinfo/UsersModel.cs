using System;
using System.Collections.Generic;
using System.Text;

namespace GloboTicket.Services.IdentityServerAsmbly.Usersinfo
{
    public class UsersModel
    {
        public UsersModel()
        {

        }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Image { get; set; }
        public string RoleName { get; set; }
        public int UserGroupId { get; set; }
        public string UserUnqiueKey { get; set; }
        public string UserGroupName { get; set; }
        public int UserId { get; set; }
      
    }
}
