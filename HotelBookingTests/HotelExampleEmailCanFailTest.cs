using System;
using System.Linq;
using HotelBooking;
using HotelBooking.HotelExamples;
using HotelBooking.ReservationOperationsProvider;
using HotelBooking.ReservationService;
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
            OperationsProvider = new ReservationOperationsProviderSeasonPrice(BookingService, PaymentService, Logger);
            Service = new ReservationService(OperationsProvider);
            _hotel = new HotelExampleEmailCanFail();
        }

        [Test]
        public void ShouldReturnFalseIfPriceChanged()
        {
            DateTime date = DateTime.Parse("01.06.2019");
            double price = 200;
            ReservationResult result = Service.Reserve(date, price, dummyCreditCard, dummyEmail, _hotel);
            Assert.True(result.OperationsResult.Any(x => x.Key == OperationDescriptions.PriceValidation && !x.Value));
        }

        [Test]
        public void ShouldReturnTrueIfPriceIsCorrect()
        {
            DateTime date = DateTime.Parse("02.01.2020");
            double price = 200;
            ReservationResult result = Service.Reserve(date, price, dummyCreditCard, dummyEmail, _hotel);
            Assert.True(result.OperationsResult.Any(x => x.Key == OperationDescriptions.PriceValidation && x.Value));
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

            ReservationResult result = Service.Reserve(date, price, creditCardNumber, email, _hotel);
            Assert.That(result.OperationsResult.Any(x => x.Key.StartsWith("Generated reservation number: 01")));
        }

        [Test]
        public void ShouldBookARoom()
        {
            DateTime date = DateTime.Parse("01.01.2020");
            BookingService.Stub(x => x.Book(date)).Return(true).Repeat.Once();
            PaymentService.Stub(x => x.Pay(dummyCreditCard, dummyPrice)).Return(true).Repeat.Once();
            ReservationResult result = Service.Reserve(date, dummyPrice, dummyCreditCard, dummyEmail, _hotel);
            Assert.True(result.OperationsResult.Any(x => x.Key == OperationDescriptions.Booking && x.Value));
        }

        [Test]
        public void ShouldNotBookARoomIfDateIsTooEarly()
        {
            DateTime date2 = DateTime.Now.AddDays(4);
            BookingService.Stub(x => x.Book(date2)).Return(true).Repeat.Once();
            PaymentService.Stub(x => x.Pay(dummyCreditCard, 250)).Return(true).Repeat.Once();
            ReservationResult result = Service.Reserve(date2, 250, dummyCreditCard, dummyEmail, _hotel);
            Assert.True(result.OperationsResult.Any(x => x.Key == OperationDescriptions.Booking && !x.Value));
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

            ReservationResult result = Service.Reserve(date, price, creditCardNumber, email, _hotel);
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

            ReservationResult result = Service.Reserve(date, price, creditCardNumber, email, _hotel);
            Assert.False(result.OperationsResult.Any(x => x.Key == OperationDescriptions.Booking));
        }
    }
}