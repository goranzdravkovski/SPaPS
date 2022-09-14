using PasswordGenerator;

namespace SPaPS.Helpers
{
    public class Shared
    {
        public static string GeneratePassword(int passwordLength)
        {
            var password = new Password(passwordLength).IncludeLowercase().IncludeUppercase().IncludeNumeric().IncludeSpecial().Next();
            return password;
        }
    }
}
