using HotelBase;
using HotelBooking;
using NUnit.Framework;
using Rhino.Mocks;

namespace HotelBookingTests
{
    [TestFixture]
    public class HotelBaseClassTest : MockedServices
    {
        
        private HotelBaseClass _hotel ;
        [SetUp]
        public void Setup()
        {
            _hotel = MockRepository.GeneratePartialMock<HotelOne>(BookingService, PaymentService);

            }
        [Test]
        public void ShouldValidateEmail()
        {
            Assert.That(_hotel.SendEmail("test"), Is.EqualTo(false));
            Assert.That(_hotel.SendEmail("test@gmail.com"), Is.EqualTo(true));
        }

        [Test]
        public void ShouldReturnFalseIfPaymentFailed()
        {
            PaymentService.Stub(x => x.Pay(245, 0.99)).Return(false);
            PaymentService.Stub(x => x.Pay(250, 200)).Return(true);
            Assert.That(_hotel.MakePayment(245, 0.99), Is.EqualTo(false));
            Assert.That(_hotel.MakePayment(250, 200), Is.EqualTo(true));
            Assert.That(_hotel.MakePayment(250, 00), Is.EqualTo(false));

        }

    }
}
