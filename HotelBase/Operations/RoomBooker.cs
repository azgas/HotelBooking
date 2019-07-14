using System;
using HotelBooking.BookingExternalService;

namespace HotelBooking.Operations
{
    public class RoomBooker : OperationBase
    {
        private readonly IBookingService _bookingService;
        public RoomBooker(DateTime date, double price, int creditCardNumber, string email, IBookingService bookingService) : base(date, price, creditCardNumber, email)
        {
            _bookingService = bookingService;
        }

        public override bool Execute()
        {
            return _bookingService.Book(Date);
        }
    }
}
