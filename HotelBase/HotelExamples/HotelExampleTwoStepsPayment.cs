using System.Collections.Generic;

namespace HotelBooking.HotelExamples
{
    public class HotelExampleTwoStepsPayment : IHotel
    {
        public HotelExampleTwoStepsPayment()
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

        public List<HotelOperation> Operations { get; }
    }
}
