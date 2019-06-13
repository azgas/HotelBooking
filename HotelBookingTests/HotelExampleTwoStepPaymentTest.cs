using System;
using System.Linq;
using HotelBooking;
using HotelBooking.HotelExamples;
using HotelBooking.PaymentExternalService;
using HotelBooking.ReservationOperationsProvider;
using HotelBooking.ReservationService;
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
            OperationsProvider =
                new ReservationOperationsProviderTwoStepPayment(BookingService, _paymentService, Logger);
            Service = new ReservationService(OperationsProvider);
            _hotel = new HotelExampleTwoStepsPayment();
        }

        [Test]
        public void ShouldFailProcessIfAuthorizationUnsuccessful()
        {
            _paymentService.Stub(x => x.Authorize(_dummyCreditCardNumber)).Return(false).Repeat.Once();
            BookingService.Stub(x => x.Book(_dummyDate)).Return(true).Repeat.Once();

            ReservationResult result =
                Service.Reserve(_dummyDate, _dummyPrice, _dummyCreditCardNumber, _dummyEmail, _hotel);
            Assert.False(result.Success);
            Assert.False(result.OperationsResult.Any(x => x.Key == OperationDescriptions.PriceValidation));
        }

        [Test]
        public void ShouldFailProcessIfCaptureUnsuccessful()
        {
            _paymentService.Stub(x => x.Authorize(_dummyCreditCardNumber)).Return(true).Repeat.Once();
            _paymentService.Stub(x => x.Capture(_dummyPrice)).Return(false).Repeat.Once();
            BookingService.Stub(x => x.Book(_dummyDate)).Return(true).Repeat.Once();

            ReservationResult result =
                Service.Reserve(_dummyDate, _dummyPrice, _dummyCreditCardNumber, _dummyEmail, _hotel);
            Assert.False(result.Success);
            Assert.False(result.OperationsResult.Any(x => x.Key == OperationDescriptions.Email));
        }

        [Test]
        public void ShouldMakeSuccessfulReservationIfPaymentSuccessful()
        {
            _paymentService.Stub(x => x.Authorize(_dummyCreditCardNumber)).Return(true).Repeat.Once();
            _paymentService.Stub(x => x.Capture(_dummyPrice)).Return(true).Repeat.Once();

            ReservationResult result =
                Service.Reserve(_dummyDate, _dummyPrice, _dummyCreditCardNumber, _dummyEmail, _hotel);
            Assert.True(result.Success);
            Assert.True(result.OperationsResult.Any(x => x.Key == OperationDescriptions.Capture && x.Value));
        }

        [Test]
        public void ShouldProcessWholeReservation_WhenNoOperationFails()
        {
            _paymentService.Stub(x => x.Authorize(_dummyCreditCardNumber)).Return(true).Repeat.Once();
            _paymentService.Stub(x => x.Capture(_dummyPrice)).Return(true).Repeat.Once();
            BookingService.Stub(x => x.Book(_dummyDate)).Return(true).Repeat.Once();

            ReservationResult result =
                Service.Reserve(_dummyDate, _dummyPrice, _dummyCreditCardNumber, _dummyEmail, _hotel);
            Assert.True(result.Success);
            Assert.True(result.OperationsResult.Any(x => x.Key == OperationDescriptions.PriceValidation && x.Value));
            Assert.True(result.OperationsResult.Any(x => x.Key == OperationDescriptions.Capture && x.Value));
            Assert.True(result.OperationsResult.Any(x => x.Key == OperationDescriptions.Authorization && x.Value));
            Assert.True(result.OperationsResult.Any(x => x.Key == OperationDescriptions.Booking && x.Value));
            Assert.True(result.OperationsResult.Any(x => x.Key == OperationDescriptions.Email && x.Value));
        }
    }
}