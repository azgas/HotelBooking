using System.Collections.Generic;
using HotelBooking;

namespace HotelBase
{
    public class HotelExample : HotelBaseClass, IHotel
    {
        public HotelExample(IReservationService service) : base(
            service)
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

        public sealed override List<HotelOperation> Operations { get; }
    }
}