/*using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

public class JwtCookieMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _cookieName = "tasty-cookies";
    private readonly string _signingKey;

    public JwtCookieMiddleware(RequestDelegate next)
    {
        _next = next;
        _signingKey = "mysupersecretultrauniquekeyforjwttokenmysupersecretultrauniquekeyforjwttoken";
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Cookies.TryGetValue(_cookieName, out var token))
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_signingKey);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
                var claimsIdentity = (ClaimsIdentity)principal.Identity;

                if (validatedToken is JwtSecurityToken jwtToken && jwtToken.Claims.Any(c => c.Type == "userId"))
                {
                    var userId = jwtToken.Claims.First(c => c.Type == "userId").Value;
                    claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userId));
                }

                context.User = new ClaimsPrincipal(claimsIdentity);
            }
            catch
            {
                context.Response.StatusCode = 401;
                return;
            }
        }

        await _next(context);
    }
}

public static class JwtCookieMiddlewareExtensions
{
    public static IApplicationBuilder UseJwtCookieMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<JwtCookieMiddleware>();
    }
}
*/