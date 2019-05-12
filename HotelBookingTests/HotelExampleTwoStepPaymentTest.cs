using System;
using HotelBase;
using HotelBooking;
using NUnit.Framework;
using Rhino.Mocks;

namespace HotelBookingTests
{
    [TestFixture]
    class HotelExampleTwoStepPaymentTest : MockedServices
    {
        private HotelExampleTwoStepsPayment _hotel;
        private readonly DateTime _dummyDate = DateTime.Now;
        private string _dummyEmail = "test@test.com";

        [SetUp]
        public void Setup()
        {
            KazexPayment paymentService = new KazexPayment();
            _hotel = new HotelExampleTwoStepsPayment(BookingService, paymentService, Logger);
        }

        [Test]
        public void ShouldFailProcessIfAuthorizationUnsuccessful()
        {
            double price = 20;
            int creditCardNumber = 222;
            BookingService.Stub(x => x.Book(_dummyDate)).Return(true).Repeat.Once();

            ReservationResult result = _hotel.Reserve(_dummyDate, price, creditCardNumber, _dummyEmail);
            Assert.False(result.Success);
            Assert.False(result.ReservationSuccess);
        }

        [Test]
        public void ShouldFailProcessIfCaptureUnsuccessful()
        {
            double price = 2;
            int creditCardNumber = 222543765;
            BookingService.Stub(x => x.Book(_dummyDate)).Return(true).Repeat.Once();

            ReservationResult result = _hotel.Reserve(_dummyDate, price, creditCardNumber, _dummyEmail);
            Assert.False(result.Success);
            Assert.True(result.ReservationSuccess);
        }

        [Test]
        public void ShouldMakeSuccessfulReservationIfPaymentSuccessful()
        {
            double price = 20;
            int creditCardNumber = 222543765;

            ReservationResult result = _hotel.Reserve(_dummyDate, price, creditCardNumber, _dummyEmail);
            Assert.True(result.Success);
            Assert.True(result.PaymentSuccess);
        }
    }
}
