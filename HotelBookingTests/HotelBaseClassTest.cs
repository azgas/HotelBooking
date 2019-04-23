using System;
using System.Collections.Generic;
using HotelBase;
using HotelBooking;
using NUnit.Framework;
using Rhino.Mocks;

namespace HotelBookingTests
{
    [TestFixture]
    public class HotelBaseClassTest : MockedServices
    {
        private HotelBaseClass _hotel;
        private readonly double _dummyPrice = 0;
        private readonly DateTime _dummyDate = DateTime.Now;
        private readonly int _dummyCreditCardNumber = 0;
        private readonly string _dummyEmailAddress = "test@test.gmail.com";

        [SetUp]
        public void Setup()
        {
            _hotel = MockRepository.GeneratePartialMock<HotelBaseClass>(BookingService, PaymentService,
                Logger);
        }

        [Test]
        public void ShouldSendEmailIfCorrectAddress()
        {
            Assert.That(_hotel.SendEmail("test@gmail.com"), Is.EqualTo(true));
        }

        [Test]
        public void ShouldNotSendEmailIfIncorrectAddress()
        {
            Assert.That(_hotel.SendEmail("test"), Is.EqualTo(false));
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

        [Test]
        public void ShouldIncludeSendEmailOperationResult()
        {
            _hotel.Stub(action => action.SendEmail(_dummyEmailAddress)).Return(true).Repeat.Once();
            _hotel.Stub(action => action.Operations).Return(new List<HotelOperation>
            {
                new HotelOperation(Operation.SendEmail, 1, false)
            }).Repeat.Once();

            ReservationResult result =
                _hotel.Reserve(_dummyDate, _dummyPrice, _dummyCreditCardNumber, _dummyEmailAddress);
            Assert.True(result.EmailSentSuccess);
        }

        [Test]
        public void ShouldIncludeBookRoomOperationResult()
        {
            _hotel.Stub(action => action.BookRoom(_dummyDate)).Return(true).Repeat.Once();
            _hotel.Stub(action => action.Operations).Return(new List<HotelOperation>
            {
                new HotelOperation(Operation.BookRoom, 1, false)
            }).Repeat.Once();

            ReservationResult result =
                _hotel.Reserve(_dummyDate, _dummyPrice, _dummyCreditCardNumber, _dummyEmailAddress);
            Assert.True(result.ReservationSuccess);
        }

        [Test]
        public void ShouldIncludeCheckPriceOperationResult()
        {
            _hotel.Stub(action => action.CheckPrice(_dummyPrice, _dummyDate)).Return(true).Repeat.Once();
            _hotel.Stub(action => action.Operations).Return(new List<HotelOperation>
            {
                new HotelOperation(Operation.CheckPrice, 1, false)
            }).Repeat.Once();

            ReservationResult result =
                _hotel.Reserve(_dummyDate, _dummyPrice, _dummyCreditCardNumber, _dummyEmailAddress);
            Assert.True(result.PriceValidationSuccess);
        }

        [Test]
        public void ShouldIncludeMakePaymentOperationResult()
        {
            PaymentService.Stub(x => x.Pay(_dummyCreditCardNumber, _dummyPrice)).Return(true).Repeat.Once();
            _hotel.Stub(action => action.Operations).Return(new List<HotelOperation>
            {
                new HotelOperation(Operation.MakePayment, 1, false)
            }).Repeat.Once();

            ReservationResult result =
                _hotel.Reserve(_dummyDate, _dummyPrice, _dummyCreditCardNumber, _dummyEmailAddress);
            Assert.True(result.PaymentSuccess);
        }

        [Test]
        public void ShouldCallGenerateReservationNumberMethod()
        {
            _hotel.Stub(action => action.Operations).Return(new List<HotelOperation>
            {
                new HotelOperation(Operation.GenerateReservationNumber, 1, false)
            }).Repeat.Once();

            _hotel.Reserve(_dummyDate, _dummyPrice, _dummyCreditCardNumber, _dummyEmailAddress);
            _hotel.AssertWasCalled(o => o.GenerateReservationNumber());
        }

        [Test]
        public void WhenOperationShouldStopOnFailAndItFailed_ShouldNotExecuteOtherOperations()
        {
            PaymentService.Stub(x => x.Pay(_dummyCreditCardNumber, _dummyPrice)).Return(false).Repeat.Once();

            _hotel.Stub(action => action.CheckPrice(_dummyPrice, _dummyDate)).Return(true).Repeat.Once();
            _hotel.Stub(action => action.BookRoom(_dummyDate)).Return(false).Repeat.Once();
            _hotel.Stub(action => action.SendEmail(_dummyEmailAddress)).Return(false).Repeat.Once();

            _hotel.Stub(action => action.Operations).Return(new List<HotelOperation>
            {
                new HotelOperation(Operation.CheckPrice, 1, true),
                new HotelOperation(Operation.MakePayment, 2, true),
                new HotelOperation(Operation.BookRoom, 3, false),
                new HotelOperation(Operation.SendEmail, 4, true),
                new HotelOperation(Operation.GenerateReservationNumber, 5, false)
            }).Repeat.Once();

            _hotel.Reserve(_dummyDate, _dummyPrice, _dummyCreditCardNumber, _dummyEmailAddress);

            _hotel.AssertWasCalled(o => o.CheckPrice(_dummyPrice, _dummyDate));
            _hotel.AssertWasNotCalled(o => o.BookRoom(_dummyDate));
            _hotel.AssertWasNotCalled(o => o.SendEmail(_dummyEmailAddress));
            _hotel.AssertWasNotCalled(o => o.GenerateReservationNumber());
        }

        [Test]
        public void WhenEachOperationExecutesWithoutError_ShouldPerformAllOperations()
        {
            DateTime date = DateTime.Now;
            double price = 0;
            int creditCardNumber = 22;
            string email = "test";
            PaymentService.Stub(x => x.Pay(creditCardNumber, price)).Return(true).Repeat.Once();

            _hotel.Stub(action => action.CheckPrice(price, date)).Return(true).Repeat.Once();
            _hotel.Stub(action => action.BookRoom(date)).Return(false).Repeat.Once();
            _hotel.Stub(action => action.SendEmail(email)).Return(false).Repeat.Once();

            _hotel.Stub(action => action.Operations).Return(new List<HotelOperation>
            {
                new HotelOperation(Operation.CheckPrice, 1, false),
                new HotelOperation(Operation.MakePayment, 2, false),
                new HotelOperation(Operation.BookRoom, 3, false),
                new HotelOperation(Operation.SendEmail, 4, false),
                new HotelOperation(Operation.GenerateReservationNumber, 5, false)
            }).Repeat.Once();

            ReservationResult result = _hotel.Reserve(date, price, creditCardNumber, email);
            Assert.True(result.Success);
            Assert.True(result.PriceValidationSuccess);
            Assert.True(result.PaymentSuccess);
            Assert.False(result.ReservationSuccess);
            Assert.False(result.EmailSentSuccess);
        }
    }
}