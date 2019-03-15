using System;

namespace HotelBase
{
    public abstract class HotelBaseClass
    {
        internal abstract bool MakePayment(string creditCardNumber);
        internal abstract bool CheckPrice(double price, DateTime date);

        internal bool SendEmail(string email)
        {
            return true;
        }

    internal abstract string GenerateReservationNumber();
        internal abstract bool BookRoom(DateTime date);
    }
}
