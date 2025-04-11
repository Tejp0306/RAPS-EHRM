using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace EHRM.Infrastructure.Configurations
{
    public class SessionExpirationMiddleware
    {
        private readonly RequestDelegate _next;

        public SessionExpirationMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var jwtToken = context.Session.GetString("JwtToken");
            var path = context.Request.Path.Value?.ToLower();
            if (jwtToken != null)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                try
                {
                    var token = tokenHandler.ReadJwtToken(jwtToken);
                    var expirationDate = token.ValidTo;
                    if (expirationDate >= DateTime.UtcNow && path.Contains("/dashboard/dashboard"))
                    {
                        await _next(context);
                        return;
                    }
                }
                catch (Exception)
                {
                    context.Response.Redirect("/Account/Login");
                    return;
                }
            }
            else
            {
                if (path == "/" ||
                   path.Contains("/account/login") ||
                   path.Contains("/account/savelogin") ||
                   path.Contains("/account/otp"))
                {
                    await _next(context);
                    return;
                }
                else
                {
                    context.Response.Redirect("/Account/Login");
                    return;
                }
            }
            await _next(context);
        }

    }


}
