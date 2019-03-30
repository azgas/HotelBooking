using System;
using HotelBooking;

namespace HotelBase
{
    public class HotelExampleEmailCanFail : HotelBaseClass, IHotel
    {
        private const double SeasonPrice = 250;
        private const double PriceAfterSeason = 200;
        private const string Id = "01";

        public virtual ReservationResult Reserve(DateTime date, double price, int creditCardNumber, string email)
        {
            bool priceValidation = CheckPrice(price, date);
            if (!priceValidation)
                return new ReservationResult(false);

            bool roomBooked = BookRoom(date);
            if (!roomBooked)
                return new ReservationResult(false, priceValidationSuccess: priceValidation);

            bool paymentMade = MakePayment(creditCardNumber, price);
            if (!paymentMade)
                return new ReservationResult(false, priceValidationSuccess: priceValidation,
                    reservationSuccess: roomBooked);

            bool emailSent = SendEmail(email);

            return new ReservationResult(true, priceValidationSuccess: priceValidation,
                paymentSuccess: paymentMade, reservationSuccess: roomBooked, emailSentSuccess: emailSent,
                reservationNumber: GenerateReservationNumber());
        }

        internal override bool CheckPrice(double price, DateTime date)
        {
            double actualPrice;
            if (date.Month > 5 && date.Month < 9)
                actualPrice = SeasonPrice;
            else
            {
                actualPrice = PriceAfterSeason;
            }

            return Math.Abs(price - actualPrice) < 0.01;
        }

        internal override string GenerateReservationNumber()
        {
            return StringHelper.GenerateRandomString(Id);
        }

        internal override bool BookRoom(DateTime date)
        {
            if (date < DateTime.Now.AddDays(5))
            {
                Logger.Write(Messages.DateTooEarly);
                return false;
            }

            bool roomBooked = BookRoomInExternalService(date);
            if (!roomBooked) return false;
            Logger.Write(Messages.BookedRoom + Id);
            return true;
        }

        public HotelExampleEmailCanFail(IBookingService bookingService, IPaymentService paymentService, ILogger logger)
            : base(bookingService,
                paymentService, logger)
        {
        }
    }
}