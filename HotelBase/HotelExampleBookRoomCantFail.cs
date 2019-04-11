using System;
using HotelBooking;

namespace HotelBase
{
    public class HotelExampleBookRoomCantFail : HotelBaseClass, IHotel
    {
        public HotelExampleBookRoomCantFail(IBookingService bookingService, IPaymentService paymentService, ILogger logger) : base(bookingService, paymentService, logger)
        {

        }

        internal override bool CheckPrice(double price, DateTime date)
        {
            if (double.IsNaN(price) || date == default(DateTime))
                return false;
            return true;
        }

        internal override bool SendEmail(string email)
        {
            if (email == null)
                return false;
            return true;
        }

        public ReservationResult Reserve(DateTime date, double price, int creditCardNumber, string email)
        {
            bool priceValid = CheckPrice(price, date);
            bool paymentMade = MakePayment(creditCardNumber, price);
            bool emailSent = SendEmail(email);
            bool roomBooked = BookRoom(date);
            string reservationNumber;
            bool success;
            if (roomBooked)
            {
                reservationNumber = GenerateReservationNumber();
                success = true;
            }
            else
            {
                reservationNumber = null;
                success = false;
            }

            return new ReservationResult(success, reservationNumber, priceValid, roomBooked, paymentMade, emailSent);
        }
    }
}
