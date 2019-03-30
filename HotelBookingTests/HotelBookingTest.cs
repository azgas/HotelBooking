using System;
using HotelBase;
using HotelBooking;
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
            Factory.Stub(f => f.ReturnHotel(1, BookingService, PaymentService, Logger)).Return(new HotelExampleEmailCanFail(BookingService, PaymentService, Logger));
            Factory.Stub(f => f.ReturnHotel(2, BookingService, PaymentService, Logger)).Return(new HotelExample(BookingService, PaymentService, Logger));
            Factory.Stub(f => f.ReturnHotel(3, BookingService, PaymentService, Logger)).Return(null);
            _manager = new HotelManager(Factory, PaymentService, BookingService, Logger);
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
            var result = _manager.MakeReservation(1, 3.0, 3333, "test@gmail.com", DateTime.Now);
            Assert.IsInstanceOf(typeof(ReservationResult), result);
        }

    }
}
