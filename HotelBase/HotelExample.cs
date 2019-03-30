using System;
using HotelBooking;

namespace HotelBase
{
    public class HotelExample : HotelBaseClass, IHotel
    {
        public virtual ReservationResult Reserve(DateTime date, double price, int creditCardNumber, string email)
        {
            bool priceValidation = CheckPrice(price, date);
            if (!priceValidation)
                return new ReservationResult(false);


            bool paymentMade = MakePayment(creditCardNumber, price);
            if (!paymentMade)
                return new ReservationResult(false, priceValidationSuccess: priceValidation);

            bool roomBooked = BookRoom(date);
            if (!roomBooked)
                return new ReservationResult(false, priceValidationSuccess: priceValidation,
                    paymentSuccess: paymentMade);

            bool emailSent = SendEmail(email);
            if (!emailSent)
                return new ReservationResult(false, priceValidationSuccess: priceValidation,
                    paymentSuccess: paymentMade, reservationSuccess: roomBooked);

            return new ReservationResult(true, priceValidationSuccess: priceValidation,
                paymentSuccess: paymentMade, reservationSuccess: roomBooked, emailSentSuccess: emailSent,
                reservationNumber: GenerateReservationNumber());
        }

        public HotelExample(IBookingService bookingService, IPaymentService paymentService, ILogger logger) : base(
            bookingService, paymentService, logger)
        {
        }
    }
}