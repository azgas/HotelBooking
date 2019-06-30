using System;
using HotelBooking.Logger;
using HotelBooking.ReservationOperationsProvider;

namespace HotelBooking.Operations
{
    class EmailSender : OperationBase
    {
        private readonly ILogger _logger;
        public EmailSender(DateTime date, double price, int creditCardNumber, string email, ILogger logger) : base(date, price, creditCardNumber, email)
        {
            _logger = logger;
        }

        public override bool Execute()
        {
            bool validEmail = StringHelper.IsValidEmail(Email);
            if (!validEmail)
                _logger.Write(Messages.InvalidEmail);
            return validEmail;
        }
    }
}