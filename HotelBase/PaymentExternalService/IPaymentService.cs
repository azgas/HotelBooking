namespace HotelBooking.PaymentExternalService
{
    public interface IPaymentService
    {
        bool Pay(int creditCardNumber, double price);
    }
}