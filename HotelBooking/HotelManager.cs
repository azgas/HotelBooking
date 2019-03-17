﻿using System;
using HotelExceptions;

namespace HotelBooking
{
    public class HotelManager : IHotelReservation
    {
        private readonly IHotelFactory _factory;
        private readonly IPaymentService _paymentService;
        private readonly IBookingService _bookingService;

        public IHotel Hotel { get; set; }
        public HotelManager(IHotelFactory factory, IPaymentService paymentService, IBookingService bookingService)
        {
            _factory = factory;
            _paymentService = paymentService;
            _bookingService = bookingService;

        }

        public ReservationResult MakeReservation(int hotelId, double price, int creditCardNumber,
            string email)
        {
            try
            {
                FindHotel(hotelId);
            }
            catch (NullHotelException e)
            {
                Console.WriteLine(e.Message);
                return new ReservationResult( false);
            }
            DateTime date = DateTime.Today;

            return Hotel.Reserve(date, price, creditCardNumber, email);

        }

        internal void FindHotel(int hotelId)
        {
            Hotel = _factory.ReturnHotel(hotelId, _bookingService, _paymentService);
            if (Hotel == null)
            {
                throw new NullHotelException($"Couldn't find hotel with specified ID: {hotelId}");
            }
        }
    }
}