using System;
using HotelBooking.PaymentExternalService;

namespace HotelBooking.Operations
{
    class PaymentAuthorization : OperationBase
    {
        private readonly IPaymentServiceTwoStep _paymentService;
        public PaymentAuthorization(DateTime date, double price, int creditCardNumber, string email, IPaymentServiceTwoStep paymentService) : base(date, price, creditCardNumber, email)
        {
            _paymentService = paymentService;
        }

        public override bool Execute()
        {
            return _paymentService.Authorize(CreditCardNumber);
        }
    }
}