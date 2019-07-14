using System;
using HotelBooking.BookingExternalService;
using HotelBooking.Logger;
using HotelBooking.ReservationOperationsProvider;

namespace HotelBooking.Operations
{
    public class RoomBookerSeason : OperationBase
    {
        private readonly IBookingService _bookingService;
        private readonly ILogger _logger;
        private const string Id = "01";

        public RoomBookerSeason(DateTime date, double price, int creditCardNumber, string email,
            IBookingService bookingService, ILogger logger) : base(date, price, creditCardNumber, email)
        {
            _bookingService = bookingService;
            _logger = logger;
        }

        public override bool Execute()
        {
            if (Date < DateTime.Now.AddDays(5))
            {
                _logger.Write(Messages.DateTooEarly);
                return false;
            }

            bool roomBooked = _bookingService.Book(Date);
            if (!roomBooked) return false;
            _logger.Write(Messages.BookedRoom + Id);
            return true;
        }
    }
}