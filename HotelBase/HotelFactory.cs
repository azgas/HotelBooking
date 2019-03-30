using HotelBooking;

namespace HotelBase
{
    public class HotelFactory : IHotelFactory
    {
        public IHotel ReturnHotel(int id, IBookingService bookingService, IPaymentService paymentService,
            ILogger logger)
        {
            switch (id)
            {
                case 1:
                    return new HotelExampleEmailCanFail(bookingService, paymentService, logger);
                case 2:
                    return new HotelExample(bookingService, paymentService, logger);
                default:
                    return null;
            }
        }
    }
}