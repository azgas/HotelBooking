using System;

namespace HotelBooking.ReservationOperationsProvider
{
    public abstract class ReservationOperationsProviderCommonFunctions
    {
        protected virtual bool CheckPrice(double price, DateTime date)
        {
            return true;
        }

        protected virtual bool SendEmail(string email)
        {
            bool validEmail = StringHelper.IsValidEmail(email);
            if (validEmail)
                return true;
            return false;
        }
    }
}
