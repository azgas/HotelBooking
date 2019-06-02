using System.Collections.Generic;
using HotelBooking;

namespace HotelBase
{
    public class HotelExampleBookRoomCantFail : HotelBaseClass, IHotel
    {
        public HotelExampleBookRoomCantFail(IReservationService service) : base(service)
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
    }
}
