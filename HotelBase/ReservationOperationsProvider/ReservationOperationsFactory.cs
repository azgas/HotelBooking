using HotelBooking.BookingExternalService;
using HotelBooking.Logger;
using HotelBooking.PaymentExternalService;

namespace HotelBooking.ReservationOperationsProvider
{
    public class ReservationOperationsFactory
    {
        public IReservationOperationsProvider ReturnService(int hotelId, IBookingService bookingService,
            IPaymentService paymentService, ILogger logger)
        {
            switch (hotelId)
            {
                case 1:
                    return new ReservationOperationsProviderSeasonPrice(bookingService, paymentService, logger);
                case 2:
                case 3:
                    return new ReservationOperationsProviderOneStepPayment(bookingService, paymentService, logger);
                case 4:
                    return typeof(IPaymentService) == typeof(IPaymentServiceTwoStep)
                        ? new ReservationOperationsProviderTwoStepPayment(bookingService,
                            (IPaymentServiceTwoStep) paymentService, logger)
                        : null;
                default:
                    return null;
            }
        }
    }
}