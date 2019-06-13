using HotelBooking.BookingExternalService;
using HotelBooking.Logger;
using HotelBooking.PaymentExternalService;
using HotelBooking.ReservationOperationsProvider;

namespace HotelBooking.ReservationService
{
    public class ReservationServiceFactory
    {
        public IReservationService ReturnService(int hotelId, IBookingService bookingService, IPaymentService paymentService, ILogger logger)
        {
            switch (hotelId)
            {
                case 1:
                case 2:
                    return new ReservationService(new ReservationOperationsProviderOneStepPayment(bookingService, paymentService, logger));
                default:
                    return null;
            }
        }
    }
}
