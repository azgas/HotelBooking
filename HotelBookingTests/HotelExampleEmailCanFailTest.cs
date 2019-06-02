using System;
using HotelBase;
using HotelBooking;
using NUnit.Framework;
using Rhino.Mocks;

namespace HotelBookingTests
{
    [TestFixture]
    public class HotelExampleEmailCanFailTest : MockedServices
    {
        private HotelExampleEmailCanFail _hotel;
        private int dummyCreditCard = 2222;
        private string dummyEmail = "test@test.gmail.com@";
        private double dummyPrice = 200;

        [SetUp]
        public void Setup()
        {
            Service = new ReservationServiceCheckSeasonPrice(BookingService, PaymentService, Logger);
            _hotel = new HotelExampleEmailCanFail(Service);
        }

        [Test]
        public void ShouldReturnFalseIfPriceChanged()
        {
            DateTime date = DateTime.Parse("01.06.2019");
            double price = 200;
            ReservationResult result = _hotel.Reserve(date, price, dummyCreditCard, dummyEmail);
            Assert.AreEqual(result.PriceValidationSuccess, false);

        }

        [Test]
        public void ShouldReturnTrueIfPriceIsCorrect()
        {
            DateTime date = DateTime.Parse("02.01.2020");
            double price = 200;
            ReservationResult result = _hotel.Reserve(date, price, dummyCreditCard, dummyEmail);
            Assert.AreEqual(result.PriceValidationSuccess, true);
        }

        [Test]
        public void GeneratedReservationNumberShouldBeginWith01()
        {
            string email = "test@test.com";
            DateTime date = DateTime.Parse("01.01.2020");
            int creditCardNumber = 1234567;
            double price = 200;
            BookingService.Stub(x => x.Book(date)).Return(true).Repeat.Once();
            PaymentService.Stub(x => x.Pay(creditCardNumber, price)).Return(true).Repeat.Once();

            ReservationResult result = _hotel.Reserve(date, price, creditCardNumber, email);
            Assert.That(result.ReservationNumber.StartsWith("01"));
        }

        [Test]
        public void ShouldBookARoom()
        {
            DateTime date = DateTime.Parse("01.01.2020");
            BookingService.Stub(x => x.Book(date)).Return(true).Repeat.Once();
            PaymentService.Stub(x => x.Pay(dummyCreditCard, dummyPrice)).Return(true).Repeat.Once();
            ReservationResult result = _hotel.Reserve(date, dummyPrice, dummyCreditCard, dummyEmail);
            Assert.AreEqual(result.ReservationSuccess, true);

        }

        [Test]
        public void ShouldNotBookARoomIfDateIsTooEarly()
        {
            DateTime date2 = DateTime.Now.AddDays(4);
            BookingService.Stub(x => x.Book(date2)).Return(true).Repeat.Once();
            PaymentService.Stub(x => x.Pay(dummyCreditCard, dummyPrice)).Return(true).Repeat.Once();
            ReservationResult result = _hotel.Reserve(date2, dummyPrice, dummyCreditCard, dummyEmail);
            Assert.AreEqual(result.ReservationSuccess, false);
        }

       [Test]
        public void ShouldProcessWholeReservation()
        {
            string email = "test@test.com";
            DateTime date = DateTime.Parse("01.01.2020");
            int creditCardNumber = 1234567;
            double price = 200;
            BookingService.Stub(x => x.Book(date)).Return(true).Repeat.Once();
            PaymentService.Stub(x => x.Pay(creditCardNumber, price)).Return(true).Repeat.Once();

            ReservationResult result = _hotel.Reserve(date, price, creditCardNumber, email);
            Assert.True(result.Success);
        }

        [Test]
        public void ShouldProcessMakePaymentBeforeBookRoom()
        {
            double price = 200;
            DateTime date = DateTime.Parse("01.01.2020");
            string email = "test@test.com";
            int creditCardNumber = 1234567;

            BookingService.Stub(x => x.Book(date)).Return(true).Repeat.Once();
            PaymentService.Stub(x => x.Pay(creditCardNumber, price)).Return(false).Repeat.Once();

            ReservationResult result = _hotel.Reserve(date, price, creditCardNumber, email);
            Assert.False(result.ReservationSuccess);
        }
    }
}
