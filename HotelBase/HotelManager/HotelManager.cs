using System;
using System.Collections.Generic;
using HotelBooking.BookingExternalService;
using HotelBooking.HotelExamples;
using HotelBooking.HotelFactory;
using HotelBooking.Logger;
using HotelBooking.PaymentExternalService;
using HotelBooking.ReservationOperationsProvider;
using HotelBooking.ReservationService;

namespace HotelBooking.HotelManager
{
    public class HotelManager
    {
        private readonly IHotelFactory _factory;
        private readonly IPaymentService _paymentService;
        private readonly IBookingService _bookingService;
        private readonly ILogger _logger;
        private readonly ReservationOperationsFactory _operationsFactory;

        private IHotel _hotel;
        private IReservationService _reservationService;

        public HotelManager(IHotelFactory factory, 
            IPaymentService paymentService, 
            IBookingService bookingService,
            ILogger logger)
        {
            _factory = factory;
            _paymentService = paymentService;
            _bookingService = bookingService;
            _logger = logger;
            _operationsFactory = new ReservationOperationsFactory();
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

            IReservationOperationsProvider operationsProvider = _operationsFactory.ReturnService(hotelId, _bookingService, _paymentService, _logger);
            _reservationService =
                new ReservationService.ReservationService(operationsProvider);
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