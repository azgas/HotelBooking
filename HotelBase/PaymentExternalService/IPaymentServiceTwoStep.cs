namespace HotelBooking.PaymentExternalService
{
    public interface IPaymentServiceTwoStep : IPaymentService
    {
        bool Capture(double price);

        bool Authorize(int creditCardNumber);
    }
}