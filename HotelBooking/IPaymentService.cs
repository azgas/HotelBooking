namespace HotelBooking
{
    public interface IPaymentService
    {
        bool Pay(int creditCardNumber, double price);
    }
}