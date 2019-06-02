using System;

namespace HotelBase
{
    public static class CommonHotelOperations
    {
        public static bool SendEmail(string email)
        {
            bool validEmail = StringHelper.IsValidEmail(email);
            if (validEmail)
                return true;
/*            Logger.Write(Messages.InvalidEmail);*/
            return false;
        }

        public static string GenerateReservationNumber()
        {
            return StringHelper.GenerateRandomString("0");
        }

        public static bool CheckPrice(double price, DateTime date)
        {
            return true;
        }

    }
}
