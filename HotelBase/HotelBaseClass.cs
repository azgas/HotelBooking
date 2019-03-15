using System;
using System.Linq.Expressions;
using HotelBooking;

namespace HotelBase
{
    public abstract class HotelBaseClass
    {
        private IBookingService _bookingService;
        private readonly IPaymentService _paymentService;

        internal bool MakePayment(int creditCardNumber, double price)
        {
            bool success;
            try
            {
                success = _paymentService.Pay(creditCardNumber, price);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

            if (!success)
                Console.WriteLine(Messages.PaymentFail);

            
            return success;
        }
        internal abstract bool CheckPrice(double price, DateTime date);

        protected HotelBaseClass(IBookingService bookingService, IPaymentService paymentService)
        {
            _bookingService = bookingService;
            _paymentService = paymentService;
        }

        internal bool SendEmail(string email)
        {
            bool validEmail = StringHelper.IsValidEmail(email);
            if(validEmail)
            return true;
            Console.WriteLine(Messages.InvalidEmail);
            return false;
        }

        public abstract ReservationResult Reserve(DateTime date, double price, int creditCardNumber, string email);
        internal abstract string GenerateReservationNumber();
        internal abstract bool BookRoom(DateTime date);
    }
}
