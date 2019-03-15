using System;
using HotelBooking;

namespace HotelBase
{
    public class HotelTwo : HotelBaseClass, IHotel 
    {
        public override ReservationResult Reserve(DateTime date, double price, int creditCardNumber, string email)
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

        public HotelTwo(IBookingService bookingService, IPaymentService paymentService) : base(bookingService, paymentService)
        {
        }
    }
}
