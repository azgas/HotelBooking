namespace HotelBooking.PaymentExternalService
{
    public class KazexPayment : IPaymentServiceTwoStep
    {

        public bool Pay(int creditCardNumber, double price)
        {
            if(!Authorize(creditCardNumber)) return false;
            return Capture(price);
        }

        public bool Authorize(int creditCardNumber)
        {
            if (creditCardNumber.ToString().Length > 7)
                return true;
            return false;
        }

        public bool Capture(double price)
        {
            if (price < 10)
                return false;
            return true;
        }
    }
}