using System;
using System.Collections.Generic;
using HotelBooking.BookingExternalService;
using HotelBooking.HotelExamples;
using HotelBooking.HotelFactory;
using HotelBooking.Logger;
using HotelBooking.PaymentExternalService;
using HotelBooking.ReservationService;

namespace HotelBooking.HotelManager
{
    public class HotelManager
    {
        private readonly IHotelFactory _factory;
        private readonly IPaymentService _paymentService;
        private readonly IBookingService _bookingService;
        private readonly ILogger _logger;
        private readonly ReservationServiceFactory _serviceFactory;

        private IHotel _hotel;
        private IReservationService _reservationService;

        public HotelManager(IHotelFactory factory, IPaymentService paymentService, IBookingService bookingService,
            ILogger logger)
        {
            _factory = factory;
            _paymentService = paymentService;
            _bookingService = bookingService;
            _logger = logger;
            _serviceFactory = new ReservationServiceFactory();
        }

        public ReservationResult MakeReservation(int hotelId, double price, int creditCardNumber,
            string email, DateTime date)
        {
            bool hotelFound = HotelExists(hotelId);

            if (!hotelFound)
            {
                _logger.Write($"Couldn't find hotel with ID: {hotelId}");
                return new ReservationResult(false);
            }

            _reservationService = _serviceFactory.ReturnService(hotelId, _bookingService, _paymentService, _logger);
            return _reservationService.Reserve(date, price, creditCardNumber, email, _hotel);
        }

        public List<int> PresentAvailableHotels()
        {
            return _factory.GetHotelIds();
        }

        private bool HotelExists(int hotelId)
        {
            _hotel = _factory.ReturnHotel(hotelId);
            return _hotel != null;
        }
    }
}