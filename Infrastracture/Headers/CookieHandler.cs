using Microsoft.AspNetCore.Http;

public class CookieHandler : ICookieHandler
{
    private readonly IHttpContextAccessor httpContextAccessor;

    public CookieHandler(IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor;
    }

    public void SetCookie(string key, string value, int expiryDays)
    {
        var options = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            Expires = DateTime.UtcNow.AddDays(expiryDays)
        };

        httpContextAccessor.HttpContext?.Response.Cookies.Append(key, value, options);
    }

    public string GetCookie(string key)
    {
        return httpContextAccessor.HttpContext?.Request.Cookies[key];
    }

    public void RemoveCookie(string key)
    {
        httpContextAccessor.HttpContext?.Response.Cookies.Delete(key);
    }
}