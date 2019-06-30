using System;

namespace HotelBooking.Operations
{
    public class PriceChecker : OperationBase
    {
        public PriceChecker(DateTime date, double price, int creditCardNumber, string email) : base(date, price, creditCardNumber, email)
        {
        }
        public override bool Execute()
        {
            return true;
        }
    }
}