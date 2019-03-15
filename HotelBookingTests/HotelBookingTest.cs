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
        private HotelManager _manager;

        [SetUp]
        public void Setup()
        {
            _factory.Stub(f => f.ReturnHotel(1)).Return(new HotelOne());
            _factory.Stub(f => f.ReturnHotel(2)).Return(new HotelTwo());
            _factory.Stub(f => f.ReturnHotel(3)).Return(null);
            _manager = new HotelManager(_factory);
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
