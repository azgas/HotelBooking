namespace HotelBooking
{
    public interface IHotelFactory
    {
        IHotel ReturnHotel(int id, IBookingService bookingService, IPaymentService paymentService, ILogger logger);
    }
}
