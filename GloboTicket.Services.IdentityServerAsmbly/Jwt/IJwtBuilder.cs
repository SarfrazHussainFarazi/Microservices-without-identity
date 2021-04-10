using System;
using System.Collections.Generic;
using System.Text;

namespace GloboTicket.Services.IdentityServerAsmbly.Jwt
{
    public interface IJwtBuilder
    {
        string GetToken(UsersDto _UsersDto);
        Guid ValidateToken(string token);
    }
}
