using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace HotelBase
{
    public static class StringHelper
    {
        public static bool IsValidEmail(string email)
        {
            Regex validEmailFormat = new Regex(@"^\w+@\w+\.\w+$");
            return validEmailFormat.Match(email).Success;
        }

        public static string GenerateRandomString(string prefix)
        {
            Random random = new Random();
            const string chars = "A0123456789";
            string randomString = new string( Enumerable.Repeat(chars, 8).Select(s => s[random.Next(s.Length)]).ToArray());
            return prefix + randomString;
            
        }
    }
}
