using System;
using System.Collections.Generic;
using HotelBooking;

namespace HotelBase
{
    public class HotelExampleEmailCanFail : HotelBaseClass, IHotel
    {
        private const double SeasonPrice = 250;
        private const double PriceAfterSeason = 200;
        private const string Id = "01";

        public HotelExampleEmailCanFail(IBookingService bookingService, IPaymentService paymentService, ILogger logger)
            : base(bookingService,
                paymentService, logger)
        {
            Operations = new List<HotelOperation>
            {
                new HotelOperation(Operation.CheckPrice, 1, false),
                new HotelOperation(Operation.MakePayment, 2, false),
                new HotelOperation(Operation.SendEmail, 3, false),
                new HotelOperation(Operation.BookRoom, 4, true),
                new HotelOperation(Operation.GenerateReservationNumber, 5, false)
            };
        }

        public override List<HotelOperation> Operations { get; }

        internal override bool CheckPrice(double price, DateTime date)
        {
            double actualPrice;
            if (date.Month > 5 && date.Month < 9)
                actualPrice = SeasonPrice;
            else
                actualPrice = PriceAfterSeason;

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
    }
}