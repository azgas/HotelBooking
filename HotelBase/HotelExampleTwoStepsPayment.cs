using System.Collections.Generic;
using HotelBooking;

namespace HotelBase
{
    public class HotelExampleTwoStepsPayment : HotelBaseClass
    {
        public HotelExampleTwoStepsPayment(IBookingService bookingService, KazexPayment paymentService, ILogger logger) : base(bookingService, paymentService, logger)
        {
            Operations = new List<HotelOperation>
            {
                new HotelOperation(Operation.MakePayment, 1, true),
                new HotelOperation(Operation.CheckPrice, 2, false),
                new HotelOperation(Operation.BookRoom, 3, false),
                new HotelOperation(Operation.MakePayment, 4, true),
                new HotelOperation(Operation.SendEmail, 5, false),
                new HotelOperation(Operation.GenerateReservationNumber, 6, false)
            };
        }

        public override List<HotelOperation> Operations { get; }
    }
}
