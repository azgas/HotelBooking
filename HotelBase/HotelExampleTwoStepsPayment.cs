using System.Collections.Generic;

namespace HotelBase
{
    public class HotelExampleTwoStepsPayment : HotelBaseClass
    {
        public HotelExampleTwoStepsPayment(IReservationService service) : base(service)
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
    }
}
