namespace BACKEND.Helper;

public class GeneralHelper
{
    public string HashPassword(string password)
    {
        return  BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool VerifyPassword(string inputPassword, string hashedPassword)
    {
        if (string.IsNullOrEmpty(hashedPassword))
        {
            return false;
        }
        return BCrypt.Net.BCrypt.Verify(inputPassword, hashedPassword);
    }
}