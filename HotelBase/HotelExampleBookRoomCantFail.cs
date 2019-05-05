using System;
using System.Collections.Generic;
using HotelBooking;

namespace HotelBase
{
    public class HotelExampleBookRoomCantFail : HotelBaseClass, IHotel
    {
        public HotelExampleBookRoomCantFail(IBookingService bookingService, IPaymentService paymentService, ILogger logger) : base(bookingService, paymentService, logger)
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
    }
}
