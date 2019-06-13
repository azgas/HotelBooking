using System;
using HotelBooking;
using HotelBooking.HotelExamples;
using HotelBooking.ReservationOperationsProvider;
using HotelBooking.ReservationService;
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
            OperationsProvider =
                new ReservationOperationsProviderOneStepPayment(BookingService, PaymentService, Logger);
            Service = new ReservationService(OperationsProvider);
            _hotel = new HotelExampleBookRoomCantFail();
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

            ReservationResult result = Service.Reserve(date, price, creditCardNumber, email, _hotel);

            Assert.False(result.Success);
        }

        [Test]
        public void ShouldReturnSuccessfulReservationResultIfBookRoomSuccess()
        {
            string email = "";
            DateTime date = default(DateTime);
            int creditCardNumber = 1234567;
            double price = double.NaN;
            BookingService.Stub(x => x.Book(date)).Return(true).Repeat.Once();
            PaymentService.Stub(x => x.Pay(creditCardNumber, price)).Return(false).Repeat.Once();

            ReservationResult result = Service.Reserve(date, price, creditCardNumber, email, _hotel);

            Assert.True(result.Success);
        }
    }
}