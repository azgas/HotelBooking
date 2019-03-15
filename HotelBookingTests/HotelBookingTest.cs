using HotelBase;
using HotelBooking;
using HotelExceptions;
using NUnit.Framework;
using Rhino.Mocks;

namespace HotelBookingTests
{
    [TestFixture]
    public class HotelBookingTest
    {
        private readonly IHotelFactory _factory = MockRepository.GenerateMock<IHotelFactory>();
        private readonly IPaymentService _paymentService = MockRepository.GenerateMock<IPaymentService>();
        private readonly IBookingService _bookingService = MockRepository.GenerateMock<IBookingService>();

        private HotelManager _manager;

        [SetUp]
        public void Setup()
        {
            _factory.Stub(f => f.ReturnHotel(1, _bookingService, _paymentService)).Return(new HotelOne(_bookingService, _paymentService));
            _factory.Stub(f => f.ReturnHotel(2, _bookingService, _paymentService)).Return(new HotelTwo(_bookingService, _paymentService));
            _factory.Stub(f => f.ReturnHotel(3, _bookingService, _paymentService)).Return(null);
            _manager = new HotelManager(_factory, _paymentService, _bookingService);
        }

        [Test]
        public void ShouldThrowExceptionWhenHotelIDIsNotAvailable()
        {
            int id = 3;
            var ex = Assert.Throws<NullHotelException>(() => _manager.FindHotel(id));
            Assert.That(ex.Message, Is.EqualTo($"Couldn't find hotel with specified ID: {id}"));
        }

        [Test]
        public void ShouldReturnHotelWithCorrectID()
        {
            _manager.FindHotel(2);
            Assert.IsInstanceOf(typeof(HotelTwo), _manager.Hotel);
        }
    }
}
