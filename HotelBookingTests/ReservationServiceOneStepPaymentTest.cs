using System;
using System.Collections.Generic;
using HotelBase;
using HotelBooking;
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
        private IReservationService _service;

        [SetUp]
        public void Setup()
        {
            _service = new ReservationServiceOneStepPayment(BookingService, PaymentService, Logger);
        }

        [Test]
        public void ShouldSendEmailIfCorrectAddress()
        {
            Assert.That(CommonHotelOperations.SendEmail("test@gmail.com"), Is.EqualTo(true));
        }

        [Test]
        public void ShouldNotSendEmailIfIncorrectAddress()
        {
            Assert.That(CommonHotelOperations.SendEmail("test"), Is.EqualTo(false));
        }

        [Test]
        public void ShouldReturnFalseIfPaymentFailed()
        {
            PaymentService.Stub(x => x.Pay(245, 0.99)).Return(false);
            List<HotelOperation> operations = new List<HotelOperation>
            {
                new HotelOperation(Operation.MakePayment, 1, false)
            };
            ReservationResult result = _service.Reserve(_dummyDate, 0.99, 245, _dummyEmailAddress, operations);
            Assert.False(result.PaymentSuccess);
        }
        [Test]
        public void ShouldReturnTrueIfPaymentSucceeded()
        {
            PaymentService.Stub(x => x.Pay(250, 200)).Return(true);
            List<HotelOperation> operations = new List<HotelOperation>
            {
                new HotelOperation(Operation.MakePayment, 1, false)
            };
            ReservationResult result = _service.Reserve(_dummyDate, 200, 250, _dummyEmailAddress, operations);
            Assert.True(result.PaymentSuccess);
        }

        [Test]
        public void ShouldIncludeSendEmailOperationResult()
        {
            List<HotelOperation> operations = new List<HotelOperation>
            {
                new HotelOperation(Operation.SendEmail, 1, false)
            };

            ReservationResult result =
                _service.Reserve(_dummyDate, _dummyPrice, _dummyCreditCardNumber, _dummyEmailAddress, operations);
            Assert.True(result.EmailSentSuccess);
        }

        [Test]
        public void ShouldIncludeBookRoomOperationResult()
        {
            BookingService.Stub(x => x.Book(_dummyDate)).Return(true).Repeat.Once();

            List<HotelOperation> operations = new List<HotelOperation>
            {
                new HotelOperation(Operation.BookRoom, 1, false)
            };

            ReservationResult result =
                _service.Reserve(_dummyDate, _dummyPrice, _dummyCreditCardNumber, _dummyEmailAddress, operations);
            Assert.True(result.ReservationSuccess);
        }

        [Test]
        public void ShouldIncludeCheckPriceOperationResult()
        {
            List<HotelOperation> operations = new List<HotelOperation>
            {
                new HotelOperation(Operation.CheckPrice, 1, false)
            };

            ReservationResult result =
                _service.Reserve(_dummyDate, _dummyPrice, _dummyCreditCardNumber, _dummyEmailAddress, operations);
            Assert.True(result.PriceValidationSuccess);
        }

        [Test]
        public void ShouldIncludeMakePaymentOperationResult()
        {
            PaymentService.Stub(x => x.Pay(_dummyCreditCardNumber, _dummyPrice)).Return(true).Repeat.Once();

            List<HotelOperation> operations = new List<HotelOperation>
            {
                new HotelOperation(Operation.MakePayment, 1, false)
            };

            ReservationResult result =
                _service.Reserve(_dummyDate, _dummyPrice, _dummyCreditCardNumber, _dummyEmailAddress, operations);
            Assert.True(result.PaymentSuccess);
        }

        [Test]
        public void ShouldCallGenerateReservationNumberMethod()
        {

            List<HotelOperation> operations = new List<HotelOperation>
            {
                new HotelOperation(Operation.GenerateReservationNumber, 1, false)
            };

            ReservationResult result = _service.Reserve(_dummyDate, _dummyPrice, _dummyCreditCardNumber, _dummyEmailAddress, operations);
            Assert.NotNull(result.ReservationNumber);
        }

        [Test]
        public void WhenOperationShouldStopOnFailAndItFailed_ShouldNotExecuteOtherOperations()
        {
            PaymentService.Stub(x => x.Pay(_dummyCreditCardNumber, _dummyPrice)).Return(false).Repeat.Once();
            BookingService.Stub(x => x.Book(_dummyDate)).Return(true).Repeat.Once();

            List<HotelOperation> operations = new List<HotelOperation>
            {
                new HotelOperation(Operation.CheckPrice, 1, true),
                new HotelOperation(Operation.MakePayment, 2, true),
                new HotelOperation(Operation.BookRoom, 3, false),
                new HotelOperation(Operation.SendEmail, 4, false),
                new HotelOperation(Operation.GenerateReservationNumber, 5, false)
            };

            ReservationResult result = _service.Reserve(_dummyDate, _dummyPrice, _dummyCreditCardNumber, _dummyEmailAddress, operations);
            Assert.IsTrue(result.PriceValidationSuccess);
            Assert.False(result.ReservationSuccess);
            Assert.False(result.EmailSentSuccess);
            Assert.IsNull(result.ReservationNumber);
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

            List<HotelOperation> operations = new List<HotelOperation>
            {
                new HotelOperation(Operation.CheckPrice, 1, false),
                new HotelOperation(Operation.MakePayment, 2, false),
                new HotelOperation(Operation.BookRoom, 3, false),
                new HotelOperation(Operation.SendEmail, 4, false),
                new HotelOperation(Operation.GenerateReservationNumber, 5, false)
            };

            ReservationResult result = _service.Reserve(date, price, creditCardNumber, email, operations);
            Assert.True(result.Success);
            Assert.True(result.PriceValidationSuccess);
            Assert.True(result.PaymentSuccess);
            Assert.False(result.ReservationSuccess);
            Assert.False(result.EmailSentSuccess);
        }
    }
}