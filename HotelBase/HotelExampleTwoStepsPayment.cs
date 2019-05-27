using System.Collections.Generic;
using System.Net;
using HotelBooking;

namespace HotelBase
{
    public class HotelExampleTwoStepsPayment : HotelBaseClass
    {
        public HotelExampleTwoStepsPayment(IBookingService bookingService, IPaymentServiceTwoStep paymentService, ILogger logger) : base(bookingService, paymentService, logger)
        {
            Operations = new List<HotelOperation>
            {
                new HotelOperation(Operation.Authorization, 1, true),
                new HotelOperation(Operation.CheckPrice, 2, false),
                new HotelOperation(Operation.BookRoom, 3, false),
                new HotelOperation(Operation.Capture, 4, true),
                new HotelOperation(Operation.SendEmail, 5, false),
                new HotelOperation(Operation.GenerateReservationNumber, 6, false)
            };
        }

        public override List<HotelOperation> Operations { get; }

        internal override bool MakePayment(int creditCardNumber, double price, Operation operation = Operation.MakePayment)
        {
            if (!(PaymentService is IPaymentServiceTwoStep paymentService)) return false;

            switch (operation)
            {
                case Operation.Authorization:
                    return paymentService.Authorize(creditCardNumber);
                case Operation.Capture:
                    return paymentService.Capture(price);
                default:
                    return false;
            }
        }
    }
}
