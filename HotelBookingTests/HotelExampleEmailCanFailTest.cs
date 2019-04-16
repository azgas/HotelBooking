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

        [SetUp]
        public void Setup()
        {
            _hotel = new HotelExampleEmailCanFail(BookingService, PaymentService, Logger);
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
            BookingService.Stub(x => x.Book(date)).Return(true).Repeat.Once();
            Assert.That(_hotel.BookRoom(date), Is.EqualTo(true));

        }

        [Test]
        public void ShouldNotBookARoomIfDateIsTooEarly()
        {

            DateTime date2 = DateTime.Now.AddDays(4);
            BookingService.Stub(x => x.Book(date2)).Return(true).Repeat.Once();
            Assert.That(_hotel.BookRoom(date2), Is.EqualTo(false));
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
    }
}
