using System;
using HotelBooking;

namespace HotelBase
{
    public class HotelOne : HotelBaseClass, IHotel
    {
        
        public ReservationResult Reserve(DateTime date, double price, int creditCardNumber, string email)
        {
            throw new NotImplementedException();
            
        }

        internal override bool MakePayment(string creditCardNumber)
        {
            throw new NotImplementedException();
        }

        internal override bool CheckPrice(double price, DateTime date)
        {
            throw new NotImplementedException();
        }


        internal override string GenerateReservationNumber()
        {
            throw new NotImplementedException();
        }

        internal override bool BookRoom(DateTime date)
        {
            throw new NotImplementedException();
        }
    }
}
