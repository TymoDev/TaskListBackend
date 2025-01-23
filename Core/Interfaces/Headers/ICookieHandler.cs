public interface ICookieHandler
{
    string GetCookie(string key);
    void RemoveCookie(string key);
    void SetCookie(string key, string value, int expiryDays);
}