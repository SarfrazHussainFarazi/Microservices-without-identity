using System;
using System.Collections.Generic;
using System.Text;

namespace GloboTicket.Services.IdentityServerAsmbly.Jwt
{
    public class JwtOptions
    {
        public string Secret { get; set; }
        public int ExpiryMinutes { get; set; }
    }
}
