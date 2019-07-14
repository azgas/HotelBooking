using System;

namespace HotelBooking.Operations
{
    public abstract class OperationBase
    {
        protected readonly DateTime Date;
        protected readonly double Price;
        protected readonly int CreditCardNumber;
        protected readonly string Email;
        protected OperationBase(DateTime date,
            double price,
            int creditCardNumber,
            string email)
        {
            Date = date;
            Price = price;
            CreditCardNumber = creditCardNumber;
            Email = email;
        }

        public abstract bool Execute();
    }
}
