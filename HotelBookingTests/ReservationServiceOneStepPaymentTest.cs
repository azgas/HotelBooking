using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using HotelBooking;
using HotelBooking.HotelExamples;
using HotelBooking.ReservationOperationsProvider;
using HotelBooking.ReservationService;
using NUnit.Framework;
using Rhino.Mocks;

namespace HotelBookingTests
{
    [TestFixture]
    public class ReservationServiceOneStepPaymentTest : MockedServices
    {
        private readonly double _dummyPrice = 0;
        private readonly DateTime _dummyDate = DateTime.Now;
        private readonly int _dummyCreditCardNumber = 0;
        private readonly string _dummyEmailAddress = "test@gmail.com";
        private IHotel _hotel;

        [SetUp]
        public void Setup()
        {
            OperationsProvider =
                new ReservationOperationsProviderOneStepPayment(BookingService, PaymentService, Logger);
            Service = new ReservationService(OperationsProvider);
            _hotel = MockRepository.GenerateMock<IHotel>();
            _hotel.Stub(x => x.Operations).Return(new List<HotelOperation>
            {
                new HotelOperation(Operation.CheckPrice, 1, false),
                new HotelOperation(Operation.BookRoom, 3, false),
                new HotelOperation(Operation.MakePayment, 2, false),
                new HotelOperation(Operation.SendEmail, 4, false),
                new HotelOperation(Operation.GenerateReservationNumber, 5, false)
            });
        }

        [Test]
        public void ShouldSendEmailIfCorrectAddress()
        {
            ReservationResult result =
                Service.Reserve(_dummyDate, _dummyPrice, _dummyCreditCardNumber, "test@gmail.com", _hotel);
            Assert.True(result.OperationsResult.Any(x => x.Key == OperationDescriptions.Email && x.Value));
        }

        [Test]
        public void ShouldNotSendEmailIfIncorrectAddress()
        {
            ReservationResult result =
                Service.Reserve(_dummyDate, _dummyPrice, _dummyCreditCardNumber, "test", _hotel);
            Assert.That(result.OperationsResult.Any(x => x.Key == OperationDescriptions.Email && !x.Value));
        }

        [Test]
        public void ShouldReturnFalseIfPaymentFailed()
        {
            PaymentService.Stub(x => x.Pay(245, 0.99)).Return(false);

            ReservationResult result = Service.Reserve(_dummyDate, 0.99, 245, _dummyEmailAddress, _hotel);
            Assert.That(result.OperationsResult.Any(x => x.Key == OperationDescriptions.Payment && !x.Value));
        }

        [Test]
        public void ShouldReturnTrueIfPaymentSucceeded()
        {
            PaymentService.Stub(x => x.Pay(250, 200)).Return(true);
            ReservationResult result = Service.Reserve(_dummyDate, 200, 250, _dummyEmailAddress, _hotel);
            Assert.That(result.OperationsResult.Any(x => x.Key == OperationDescriptions.Payment && x.Value));
        }

        [Test]
        public void ShouldIncludeSendEmailOperationResult()
        {
            ReservationResult result =
                Service.Reserve(_dummyDate, _dummyPrice, _dummyCreditCardNumber, _dummyEmailAddress, _hotel);
            Assert.That(result.OperationsResult.Any(x => x.Key == OperationDescriptions.Email));
        }

        [Test]
        public void ShouldIncludeBookRoomOperationResult()
        {
            BookingService.Stub(x => x.Book(_dummyDate)).Return(true).Repeat.Once();

            ReservationResult result =
                Service.Reserve(_dummyDate, _dummyPrice, _dummyCreditCardNumber, _dummyEmailAddress, _hotel);
            Assert.That(result.OperationsResult.Any(x => x.Key == OperationDescriptions.Booking));
        }

        [Test]
        public void ShouldIncludeCheckPriceOperationResult()
        {
            ReservationResult result =
                Service.Reserve(_dummyDate, _dummyPrice, _dummyCreditCardNumber, _dummyEmailAddress, _hotel);
            Assert.That(result.OperationsResult.Any(x => x.Key == OperationDescriptions.PriceValidation));
        }

        [Test]
        public void ShouldIncludeMakePaymentOperationResult()
        {
            PaymentService.Stub(x => x.Pay(_dummyCreditCardNumber, _dummyPrice)).Return(true).Repeat.Once();

            ReservationResult result =
                Service.Reserve(_dummyDate, _dummyPrice, _dummyCreditCardNumber, _dummyEmailAddress, _hotel);
            Assert.That(result.OperationsResult.Any(x => x.Key == OperationDescriptions.Payment));
        }

        [Test]
        public void ShouldCallGenerateReservationNumberMethod()
        {
            Regex reservationNumberRegex = new Regex(@"Generated reservation number: \d");

            ReservationResult result =
                Service.Reserve(_dummyDate, _dummyPrice, _dummyCreditCardNumber, _dummyEmailAddress, _hotel);
            Assert.That(result.OperationsResult.Any(x => reservationNumberRegex.IsMatch(x.Key)));
        }

        [Test]
        public void WhenOperationShouldStopOnFailAndItFailed_ShouldNotExecuteOtherOperations()
        {
            PaymentService.Stub(x => x.Pay(_dummyCreditCardNumber, _dummyPrice)).Return(false).Repeat.Once();
            BookingService.Stub(x => x.Book(_dummyDate)).Return(true).Repeat.Once();
            IHotel hotel2 = MockRepository.GenerateMock<IHotel>();
            hotel2.Stub(x => x.Operations).Return(new List<HotelOperation>
            {
                new HotelOperation(Operation.CheckPrice, 1, true),
                new HotelOperation(Operation.MakePayment, 2, true),
                new HotelOperation(Operation.BookRoom, 3, false),
                new HotelOperation(Operation.SendEmail, 4, false),
                new HotelOperation(Operation.GenerateReservationNumber, 5, false)
            });

            ReservationResult result = Service.Reserve(_dummyDate, _dummyPrice, _dummyCreditCardNumber,
                _dummyEmailAddress, hotel2);
            Assert.False(result.Success);
            Assert.That(result.OperationsResult.Any(x => x.Key == OperationDescriptions.Payment));
            Assert.False(result.OperationsResult.Any(x => x.Key == OperationDescriptions.Booking));
            Assert.False(result.OperationsResult.Any(x => x.Key == OperationDescriptions.Email));
        }

        [Test]
        public void WhenEachOperationExecutesWithoutError_ShouldPerformAllOperations()
        {
            DateTime date = DateTime.Now;
            double price = 0;
            int creditCardNumber = 22;
            string email = "test";
            PaymentService.Stub(x => x.Pay(creditCardNumber, price)).Return(true).Repeat.Once();
            BookingService.Stub(x => x.Book(_dummyDate)).Return(false).Repeat.Once();

            ReservationResult result = Service.Reserve(date, price, creditCardNumber, email, _hotel);
            Assert.True(result.Success);
            Assert.That(result.OperationsResult.Any(x => x.Key == OperationDescriptions.PriceValidation && x.Value));
            Assert.That(result.OperationsResult.Any(x => x.Key == OperationDescriptions.Payment && x.Value));
            Assert.That(result.OperationsResult.Any(x => x.Key == OperationDescriptions.Booking && !x.Value));
            Assert.That(result.OperationsResult.Any(x => x.Key == OperationDescriptions.Email && !x.Value));
        }
    }
}