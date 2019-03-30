using System;
using HotelBooking;

namespace HotelBase
{
    public abstract class HotelBaseClass
    {
        private readonly IBookingService _bookingService;
        private readonly IPaymentService _paymentService;
        internal readonly ILogger Logger;

        internal bool MakePayment(int creditCardNumber, double price)
        {
            bool successfulPayment;
            try
            {
                successfulPayment = _paymentService.Pay(creditCardNumber, price);
            }
            catch (Exception e)
            {
                Logger.Write(e.Message);
                throw;
            }

            if (!successfulPayment)
                Logger.Write(Messages.PaymentFail);

            return successfulPayment;
        }

        internal virtual bool CheckPrice(double price, DateTime date)
        {
            return true;
        }

        protected HotelBaseClass(IBookingService bookingService, IPaymentService paymentService, ILogger logger)
        {
            _bookingService = bookingService;
            _paymentService = paymentService;
            Logger = logger;
        }

        internal bool SendEmail(string email)
        {
            bool validEmail = StringHelper.IsValidEmail(email);
            if (validEmail)
                return true;
            Logger.Write(Messages.InvalidEmail);
            return false;
        }

        internal virtual string GenerateReservationNumber()
        {
            return StringHelper.GenerateRandomString("0");
        }

        internal virtual bool BookRoom(DateTime date)
        {
            return BookRoomInExternalService(date);
        }

        internal bool BookRoomInExternalService(DateTime date)
        {
            return _bookingService.Book(date);
        }
    }
}