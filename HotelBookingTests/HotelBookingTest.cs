﻿using System;
using System.Collections.Generic;
using HotelBooking;
using HotelBooking.HotelExamples;
using HotelBooking.HotelManager;
using HotelBooking.ReservationOperationsProvider;
using HotelBooking.ReservationServices;
using NUnit.Framework;
using Rhino.Mocks;

namespace HotelBookingTests
{
    [TestFixture]
    public class HotelBookingTest : MockedServices
    {
        private HotelManager _manager;

        [SetUp]
        public void Setup()
        {
            OperationsProvider = new ReservationOperationsProviderOneStepPayment(BookingService, PaymentService, Logger);
            Service = new ReservationService(OperationsProvider);
            ReservationOperationsFactory reservationOperationsFactory = new ReservationOperationsFactory();
            Factory.Stub(f => f.ReturnHotel(1)).Return(new HotelExampleEmailCanFail());
            Factory.Stub(f => f.ReturnHotel(2)).Return(new HotelExample());
            Factory.Stub(f => f.ReturnHotel(3)).Return(null);
            Factory.Stub(f => f.GetHotelIds()).Return(new List<int> {1, 2});

            _manager = new HotelManager(Factory, PaymentService, BookingService, Logger, reservationOperationsFactory);
        }

        [Test]
        public void ShouldReturnFalseAndCallLoggerWhenHotelIdIsNotAvailable()
        {
            int id = 3;
            ReservationResult result = _manager.MakeReservation(id, 3.0, 3333, "test@gmail.com", DateTime.Now);
            Assert.False(result.Success);
            Logger.AssertWasCalled(l => l.Write($"Couldn't find hotel with ID: {id}"));
        }

        [Test]
        public void ShouldReturnReservationResultForHotelWithCorrectId()
        {
            int id = 1;
            var result = _manager.MakeReservation(id, 3.0, 3333, "test@gmail.com", DateTime.Now);
            Assert.IsInstanceOf(typeof(ReservationResult), result);
            Logger.AssertWasNotCalled(l => l.Write($"Couldn't find hotel with ID: {id}"));
        }

        [Test]
        public void ShouldReturnAvailableHotelIds()
        {
            List<int> listHotels = _manager.PresentAvailableHotels();
            Assert.That(listHotels, Is.EquivalentTo(new List<int> {1, 2}));
        }
    }
}
