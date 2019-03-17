using HotelBooking;

namespace HotelBase
{
    public class HotelFactory : IHotelFactory
    {

        public IHotel ReturnHotel(int id, IBookingService bookingService, IPaymentService paymentService)
        {
            switch (id)
            {
                case 1:
                    return new HotelOne(bookingService, paymentService);
                case 2:
                    return new HotelTwo(bookingService, paymentService);
                default:
                    return null;
            }
        }
    }
}