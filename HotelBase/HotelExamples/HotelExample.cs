using System.Collections.Generic;

namespace HotelBooking.HotelExamples
{
    public class HotelExample : IHotel
    {
        public HotelExample()
        {
            Operations = new List<HotelOperation>
            {
                new HotelOperation(Operation.CheckPrice, 1, true),
                new HotelOperation(Operation.MakePayment, 2, true),
                new HotelOperation(Operation.BookRoom, 3, true),
                new HotelOperation(Operation.SendEmail, 4, true),
                new HotelOperation(Operation.GenerateReservationNumber, 5, false)
            };
        }

        public List<HotelOperation> Operations { get; }
    }
}