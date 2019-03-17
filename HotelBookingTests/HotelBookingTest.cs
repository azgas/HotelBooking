using HotelBase;
using HotelBooking;
using HotelExceptions;
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
            Factory.Stub(f => f.ReturnHotel(1, BookingService, PaymentService)).Return(new HotelOne(BookingService, PaymentService));
            Factory.Stub(f => f.ReturnHotel(2, BookingService, PaymentService)).Return(new HotelTwo(BookingService, PaymentService));
            Factory.Stub(f => f.ReturnHotel(3, BookingService, PaymentService)).Return(null);
            _manager = new HotelManager(Factory, PaymentService, BookingService);
        }

        [Test]
        public void ShouldThrowExceptionWhenHotelIdIsNotAvailable()
        {
            int id = 3;
            var ex = Assert.Throws<NullHotelException>(() => _manager.FindHotel(id));
            Assert.That(ex.Message, Is.EqualTo($"Couldn't find hotel with specified ID: {id}"));
        }

        [Test]
        public void ShouldReturnHotelWithCorrectId()
        {
            _manager.FindHotel(2);
            Assert.IsInstanceOf(typeof(HotelTwo), _manager.Hotel);
        }

    }
}
