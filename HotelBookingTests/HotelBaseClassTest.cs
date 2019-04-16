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

        [SetUp]
        public void Setup()
        {
            _hotel = MockRepository.GenerateMock<HotelBaseClass>(BookingService, PaymentService,
                Logger);
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

        [Test]
        public void ShouldProcessPartialSteps()
        {
            DateTime date = DateTime.Now;
            double price = 0;
            int creditCardNumber = 22;
            string email = "test";
            PaymentService.Stub(x => x.Pay(creditCardNumber, price)).Return(true);

            _hotel.Stub(action => action.Operations).Return(new List<HotelOperation>
            {
                new HotelOperation(Operation.CheckPrice, 1, true),
                new HotelOperation(Operation.MakePayment, 2, true),
                new HotelOperation(Operation.BookRoom, 3, false),
                new HotelOperation(Operation.SendEmail, 4, true),
                new HotelOperation(Operation.GenerateReservationNumber, 5, false)
            });

            ReservationResult result = _hotel.Reserve(date, price, creditCardNumber, email);
            Assert.False(result.Success);
            Assert.True(result.PriceValidationSuccess);
            Assert.True(result.PaymentSuccess);
            Assert.False(result.ReservationSuccess);
            Assert.False(result.EmailSentSuccess);

        }
    }
}