using System;
using HotelBase;
using HotelBooking;
using NUnit.Framework;
using Rhino.Mocks;

namespace HotelBookingTests
{
    [TestFixture]
    public class HotelExampleBookRoomCantFailTest : MockedServices
    {
        private HotelExampleBookRoomCantFail _hotel;

        [SetUp]
        public void Setup()
        {
            _hotel = new HotelExampleBookRoomCantFail(BookingService, PaymentService, Logger);
        }

        [Test]
        public void ShouldFailReservationResultIfBookRoomFails()
        {
            string email = "test@test.com";
            DateTime date = DateTime.Parse("01.01.2020");
            int creditCardNumber = 1234567;
            double price = 200;
            BookingService.Stub(x => x.Book(date)).Return(false).Repeat.Once();
            PaymentService.Stub(x => x.Pay(creditCardNumber, price)).Return(true).Repeat.Once();

            ReservationResult result = _hotel.Reserve(date, price, creditCardNumber, email);

            Assert.False(result.Success);
            Assert.True(result.PriceValidationSuccess);
            Assert.True(result.EmailSentSuccess);
            Assert.True(result.PaymentSuccess);
            Assert.False(result.ReservationSuccess);
        }

        [Test]
        public void ShouldReturnSuccessfulReservationResultIfBookRoomSuccess()
        {
            string email = null;
            DateTime date = default(DateTime);
            int creditCardNumber = 1234567;
            double price = double.NaN;
            BookingService.Stub(x => x.Book(date)).Return(true).Repeat.Once();
            PaymentService.Stub(x => x.Pay(creditCardNumber, price)).Return(false).Repeat.Once();

            ReservationResult result = _hotel.Reserve(date, price, creditCardNumber, email);

            Assert.True(result.Success);
            Assert.False(result.PriceValidationSuccess);
            Assert.False(result.EmailSentSuccess);
            Assert.False(result.PaymentSuccess);
            Assert.True(result.ReservationSuccess);
        }
    }
}
