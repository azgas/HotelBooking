﻿using System.Collections.Generic;

namespace HotelBooking.HotelExamples
{
    public class HotelExampleBookRoomCantFail : IHotel
    {
        public HotelExampleBookRoomCantFail()
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

        public List<HotelOperation> Operations { get; }
    }
}
