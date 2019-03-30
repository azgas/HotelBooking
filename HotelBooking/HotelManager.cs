using System;
using System.Collections.Generic;

namespace HotelBooking
{
    public class HotelManager
    {
        private readonly IHotelFactory _factory;
        private readonly IPaymentService _paymentService;
        private readonly IBookingService _bookingService;
        private readonly ILogger _logger;

        private IHotel _hotel;

        public HotelManager(IHotelFactory factory, IPaymentService paymentService, IBookingService bookingService,
            ILogger logger)
        {
            _factory = factory;
            _paymentService = paymentService;
            _bookingService = bookingService;
            _logger = logger;
        }

        public ReservationResult MakeReservation(int hotelId, double price, int creditCardNumber,
            string email, DateTime date)
        {
            bool hotelFound = FindHotel(hotelId);

            if (!hotelFound)
            {
                _logger.Write($"Couldn't find hotel with ID: {hotelId}");
                return new ReservationResult(false);
            }

            return _hotel.Reserve(date, price, creditCardNumber, email);
        }

        public List<int> PresentAvailableHotels()
        {
            return _factory.PresentAvailableIds();
        }

        private bool FindHotel(int hotelId)
        {
            _hotel = _factory.ReturnHotel(hotelId, _bookingService, _paymentService, _logger);
            return _hotel != null;
        }
    }
}