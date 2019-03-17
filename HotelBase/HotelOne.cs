using System;
using HotelBooking;

namespace HotelBase
{
    public class HotelOne : HotelBaseClass, IHotel
    {
        private readonly double _seasonPrice = 250;
        private readonly double _priceAfterSeason = 200;
        private readonly string _ID = "01";

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
            double actualPrice;
            if (date.Month > 5 && date.Month < 9)
                actualPrice = _seasonPrice;
            else
            {
                actualPrice = _priceAfterSeason;
            }

            return (Math.Abs(price - actualPrice) < 0.01);
        }


        internal override string GenerateReservationNumber()
        {
            return StringHelper.GenerateRandomString(_ID);

        }

        internal override bool BookRoom(DateTime date)
        {

            bool roomBooked = BookRoomInExternalService(date);
            if (roomBooked)
            {
                Console.WriteLine(Messages.BookedRoom + _ID);
                return true;
            }

            return false;
        }

        public HotelOne(IBookingService bookingService, IPaymentService paymentService) : base(bookingService,
            paymentService)
        {
        }
    }
}