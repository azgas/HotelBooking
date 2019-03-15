using System;
using HotelBooking;

namespace HotelBase
{
    public class HotelOne : HotelBaseClass, IHotel
    {
        public override ReservationResult Reserve(DateTime date, double price, int creditCardNumber, string email)
        {
            bool priceValidation = CheckPrice(price, date);
            if (!priceValidation)
                return new ReservationResult(false) {PriceValidationSuccess = false};

            bool roomBooked = BookRoom(date);
            if (!roomBooked)
                return new ReservationResult(false)
                    {PriceValidationSuccess = true, ReservationSuccess = false};

            bool paymentMade = MakePayment(creditCardNumber, price);
            if (!paymentMade)
                return new ReservationResult(false)
                    {PriceValidationSuccess = true, ReservationSuccess = true, PaymentSuccess = false};

            bool emailSent = SendEmail(email);

            return new ReservationResult(true)
            {
                PriceValidationSuccess = true, ReservationSuccess = true, PaymentSuccess = true,
                EmailSentSuccess = emailSent, ReservationNumber = GenerateReservationNumber()
            };
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

        public HotelOne(IBookingService bookingService, IPaymentService paymentService) : base(bookingService,
            paymentService)
        {
        }
    }
}