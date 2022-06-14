using QueueControlServer.Models;
using System.Security.Claims;
using System.Threading.Tasks;


namespace QueueControlServer.Interfaces
{
    public interface IGetUserClaim
    {
        string GetUserClaim(string ClaimName, ClaimsIdentity userIdentity);
        string GetUserRole(ClaimsIdentity userIdentity);
    }
}
