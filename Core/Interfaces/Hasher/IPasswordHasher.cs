namespace Infrastracture.Logic
{
    public interface IPasswordHasher
    {
        string Generate(string password);
        bool Verify(string password, string storedHash);
    }
}