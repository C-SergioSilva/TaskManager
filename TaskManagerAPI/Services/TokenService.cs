using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Services
{
    public class TokenService
    {
        public static string CreateToken(User user) 
        {
            var tokenHandle = new JwtSecurityTokenHandler(); 
        }
    }
}
