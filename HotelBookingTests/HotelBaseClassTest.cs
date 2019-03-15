using HotelBase;
using HotelBooking;
using NUnit.Framework;
using Rhino.Mocks;

namespace HotelBookingTests
{
    [TestFixture]
    public class HotelBaseClassTest
    {
        private readonly IPaymentService _paymentService = MockRepository.GenerateMock<IPaymentService>();
        private readonly IBookingService _bookingService = MockRepository.GenerateMock <IBookingService > ();
        private HotelBaseClass _hotel ;
        [SetUp]
        public void Setup()
        {
            _hotel = MockRepository.GeneratePartialMock<HotelOne>(_bookingService, _paymentService);

            }
        [Test]
        public void ShouldValidateEmail()
        {
            Assert.That(_hotel.SendEmail("test"), Is.EqualTo(false));
            Assert.That(_hotel.SendEmail("test@gmail.com"), Is.EqualTo(true));
        }

    }
}
