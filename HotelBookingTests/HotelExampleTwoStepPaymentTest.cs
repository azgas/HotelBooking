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
        private double _dummyPrice = 20;
        private int _dummyCreditCardNumber = 222;
        private readonly IPaymentServiceTwoStep _paymentService = MockRepository.GenerateMock<IPaymentServiceTwoStep>();

        [SetUp]
        public void Setup()
        {

            _hotel = new HotelExampleTwoStepsPayment(BookingService, _paymentService, Logger);
        }

        [Test]
        public void ShouldFailProcessIfAuthorizationUnsuccessful()
        {
            _paymentService.Stub(x => x.Authorize(_dummyCreditCardNumber)).Return(false).Repeat.Once();
            BookingService.Stub(x => x.Book(_dummyDate)).Return(true).Repeat.Once();

            ReservationResult result = _hotel.Reserve(_dummyDate, _dummyPrice, _dummyCreditCardNumber, _dummyEmail);
            Assert.False(result.Success);
            Assert.False(result.ReservationSuccess);
        }

        [Test]
        public void ShouldFailProcessIfCaptureUnsuccessful()
        {
            _paymentService.Stub(x => x.Authorize(_dummyCreditCardNumber)).Return(true).Repeat.Once();
            _paymentService.Stub(x => x.Capture(_dummyPrice)).Return(false).Repeat.Once();
            BookingService.Stub(x => x.Book(_dummyDate)).Return(true).Repeat.Once();

            ReservationResult result = _hotel.Reserve(_dummyDate, _dummyPrice, _dummyCreditCardNumber, _dummyEmail);
            Assert.False(result.Success);
            Assert.True(result.ReservationSuccess);
        }

        [Test]
        public void ShouldMakeSuccessfulReservationIfPaymentSuccessful()
        {
            _paymentService.Stub(x => x.Authorize(_dummyCreditCardNumber)).Return(true).Repeat.Once();
            _paymentService.Stub(x => x.Capture(_dummyPrice)).Return(true).Repeat.Once();

            ReservationResult result = _hotel.Reserve(_dummyDate, _dummyPrice, _dummyCreditCardNumber, _dummyEmail);
            Assert.True(result.Success);
            Assert.True(result.PaymentSuccess);
        }

        [Test]
        public void ShouldProcessWholeReservation_WhenNoOperationFails()
        {
            _paymentService.Stub(x => x.Authorize(_dummyCreditCardNumber)).Return(true).Repeat.Once();
            _paymentService.Stub(x => x.Capture(_dummyPrice)).Return(true).Repeat.Once();
            BookingService.Stub(x => x.Book(_dummyDate)).Return(true).Repeat.Once();

            ReservationResult result = _hotel.Reserve(_dummyDate, _dummyPrice, _dummyCreditCardNumber, _dummyEmail);
            Assert.True(result.Success);
            Assert.True(result.PriceValidationSuccess);
            Assert.True(result.PaymentSuccess);
            Assert.True(result.ReservationSuccess);
            Assert.True(result.EmailSentSuccess);
        }
    }
}
