using Authentication.Entities;
using System.Collections.Generic;

namespace Authentication.Services
{
    public interface ITokenService
    {
        string GenerateJwt(User user, IList<string> roles);
    }
}
