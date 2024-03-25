using Microsoft.AspNetCore.Identity;

namespace SolarWatch.Service.Token;

public interface ITokenService
{
    string CreateToken(IdentityUser user, string role);
}