using System;
using HotelBooking.Logger;
using HotelBooking.PaymentExternalService;
using HotelBooking.ReservationOperationsProvider;

namespace HotelBooking.Operations
{
    class Payment : OperationBase
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger _logger;
        public Payment(DateTime date, double price, int creditCardNumber, string email, IPaymentService paymentService, ILogger logger) : base(date, price, creditCardNumber, email)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        public override bool Execute()
        {
            bool success = _paymentService.Pay(CreditCardNumber, Price);
            if(!success)
                _logger.Write(Messages.PaymentFail);
            return success;
        }
    }
}