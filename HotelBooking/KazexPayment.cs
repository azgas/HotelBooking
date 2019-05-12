namespace HotelBooking
{
    public class KazexPayment : IPaymentService
    {
        private bool _authorized = false;

        public bool Pay(int creditCardNumber, double price)
        {
            if (!_authorized)
            {
                _authorized = true;
                return Authorize(creditCardNumber);
            }

            return Capture(price);
        }

        private bool Authorize(int creditCardNumber)
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