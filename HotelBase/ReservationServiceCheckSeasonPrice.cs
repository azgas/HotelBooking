using System;
using HotelBooking;

namespace HotelBase
{
    class ReservationServiceCheckSeasonPrice : ReservationServiceOneStepPayment
    {
        private const double SeasonPrice = 250;
        private const double PriceAfterSeason = 200;
        private const string Id = "01";

        public ReservationServiceCheckSeasonPrice(IBookingService bookingService, IPaymentService paymentService, ILogger logger) : base(bookingService, paymentService, logger)
        {
        }

        protected override bool CheckPrice(double price, DateTime date)
        {
            double actualPrice;
            if (date.Month > 5 && date.Month < 9)
                actualPrice = SeasonPrice;
            else
                actualPrice = PriceAfterSeason;

            return Math.Abs(price - actualPrice) < 0.01;
        }

        protected override string GenerateReservationNumber()
        {
            return StringHelper.GenerateRandomString(Id);
        }

        protected override bool BookRoom(DateTime date)
        {
            if (date < DateTime.Now.AddDays(5))
            {
                Logger.Write(Messages.DateTooEarly);
                return false;
            }

            bool roomBooked = BookingService.Book(date);
            if (!roomBooked) return false;
            Logger.Write(Messages.BookedRoom + Id);
            return true;
        }
    }
}