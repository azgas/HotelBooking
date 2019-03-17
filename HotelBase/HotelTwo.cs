using System;
using HotelBooking;

namespace HotelBase
{
    public class HotelTwo : HotelBaseClass, IHotel 
    {
        public virtual ReservationResult Reserve(DateTime date, double price, int creditCardNumber, string email)
        {
            bool priceValidation = CheckPrice(price, date);
            if (!priceValidation)
                return new ReservationResult(false) { PriceValidationSuccess = false };


            bool paymentMade = MakePayment(creditCardNumber, price);
            if (!paymentMade)
                return new ReservationResult(false)
                    { PriceValidationSuccess = true, PaymentSuccess = false };

            bool roomBooked = BookRoom(date);
            if (!roomBooked)
                return new ReservationResult(false)
                    { PriceValidationSuccess = true, PaymentSuccess = true, ReservationSuccess = false };


            bool emailSent = SendEmail(email);
            if(!emailSent)
                return new ReservationResult(false)
                {
                    PriceValidationSuccess = true, PaymentSuccess = true, ReservationSuccess = true, EmailSentSuccess = false
                };

            return new ReservationResult(true)
            {
                PriceValidationSuccess = true,
                ReservationSuccess = true,
                PaymentSuccess = true,
                EmailSentSuccess = true,
                ReservationNumber = GenerateReservationNumber()
            };
        }




        public HotelTwo(IBookingService bookingService, IPaymentService paymentService) : base(bookingService, paymentService)
        {
        }
    }
}
