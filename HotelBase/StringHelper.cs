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
    }
}
