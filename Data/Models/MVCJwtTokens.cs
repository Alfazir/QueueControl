using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace QueueControlServer.Models
{
    public class MVCJwtTokens
    {
        public const string Issuer = "MyAuthServer";
        public const string Audience = "MyAuthClient";
        public const string Key = "mysupersecret_secretkey!12743";   


        public const string AuthSchemes = "Identity.Application" + "," + JwtBearerDefaults.AuthenticationScheme;
    }
}
