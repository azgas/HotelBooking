using HotelBooking.BookingExternalService;
using HotelBooking.Logger;
using HotelBooking.PaymentExternalService;

namespace HotelBooking.ReservationOperationsProvider
{
    public class ReservationOperationsFactory
    {
        public IReservationOperationsProvider ReturnService(int hotelId, IBookingService bookingService, IPaymentService paymentService, ILogger logger)
        {
            switch (hotelId)
            {
                case 1:
                case 2:
                    return new ReservationOperationsProviderOneStepPayment(bookingService, paymentService, logger);
                default:
                    return null;
            }
        }
    }
}
