using DataAccess.ViewModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interface
{
    public interface IJwtService
    {
        public string GenerateJwtToken(SessionUser user);

        public bool ValidateToken(string token, out JwtSecurityToken jwtSecurityToken);

    }
}
