using System;
using HotelBase;
using HotelBooking;
using NUnit.Framework;
using Rhino.Mocks;

namespace HotelBookingTests
{
    [TestFixture]
    public class HotelOneTest : MockedServices
    {
        private HotelOne _hotel; 

        [SetUp]
        public void Setup()
        {
            _hotel = new HotelOne(BookingService, PaymentService);
        }

        [Test]
        public void ShouldReturnFalseIfPriceChanged()
        {
            DateTime date = DateTime.Parse("01.06.2019");
            double price = 200;
            Assert.That(_hotel.CheckPrice(price, date), Is.EqualTo(false));

        }

        [Test]
        public void ShouldReturnTrueIfPriceIsCorrect()
        {
            DateTime date = DateTime.Parse("02.01.2020");
            double price = 200;
            Assert.That(_hotel.CheckPrice(price, date), Is.EqualTo(true));
        }

        [Test]
        public void GeneratedReservationNumberShouldBeginWith01()
        {
            Assert.That(_hotel.GenerateReservationNumber().StartsWith("01"));
        }

        [Test]
        public void ShouldBookARoom()
        {
            DateTime date = DateTime.Parse("01.01.2020");
            BookingService.Stub(x => x.Book(date)).Return(true);
            Assert.That(_hotel.BookRoom(date), Is.EqualTo(true));
        }

        [Test]
        public void ShouldProcessWholeReservation()
        {
            string email = "test@test.com";
            DateTime date = DateTime.Parse("01.01.2020");
            int creditCardNumber = 1234567;
            double price = 200;
            BookingService.Stub(x => x.Book(date)).Return(true);
            PaymentService.Stub(x => x.Pay(creditCardNumber, price)).Return(true);

            ReservationResult result = _hotel.Reserve(date, price, creditCardNumber, email);
            Assert.True(result.EmailSentSuccess);
            Assert.True(result.PaymentSuccess);
            Assert.True(result.PriceValidationSuccess);
            Assert.True(result.ReservationSuccess);
            Assert.True(result.Success);
        }
    }
}
