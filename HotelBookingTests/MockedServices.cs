using HotelBooking;
using Rhino.Mocks;

namespace HotelBookingTests
{
    public class MockedServices
    {
        protected readonly IHotelFactory Factory = MockRepository.GenerateMock<IHotelFactory>();
        protected readonly IPaymentService PaymentService = MockRepository.GenerateMock<IPaymentService>();
        protected readonly IBookingService BookingService = MockRepository.GenerateMock<IBookingService>();
        protected readonly ILogger Logger = MockRepository.GenerateMock<ILogger>();
    }
}