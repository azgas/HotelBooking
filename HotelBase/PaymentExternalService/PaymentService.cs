namespace HotelBooking.PaymentExternalService
{
    public class PaymentService : IPaymentService
    {
        public bool Pay(int creditCardNumber, double price)
        {
            return true;
        }
    }
}
