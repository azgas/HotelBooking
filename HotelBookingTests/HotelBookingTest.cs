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
        public void ShouldReturnFalseWhenHotelIdIsNotAvailable()
        {
            int id = 3;
            bool hotelFound = _manager.FindHotel(id);
            Assert.False(hotelFound);
        }

        [Test]
        public void ShouldReturnHotelWithCorrectId()
        {
            _manager.FindHotel(2);
            Assert.IsInstanceOf(typeof(HotelExample), _manager.Hotel);
        }

    }
}
