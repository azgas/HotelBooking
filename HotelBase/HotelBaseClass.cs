using System;
using System.Collections.Generic;
using System.Linq;
using HotelBooking;

namespace HotelBase
{
    public abstract class HotelBaseClass
    {
        private readonly IBookingService _bookingService;
        private readonly IPaymentService _paymentService;
        internal readonly ILogger Logger;

        protected HotelBaseClass(IBookingService bookingService, IPaymentService paymentService, ILogger logger)
        {
            _bookingService = bookingService;
            _paymentService = paymentService;
            Logger = logger;
        }

        public abstract List<HotelOperation> Operations { get; }

        internal bool MakePayment(int creditCardNumber, double price)
        {
            var successfulPayment = _paymentService.Pay(creditCardNumber, price);

            if (!successfulPayment)
                Logger.Write(Messages.PaymentFail);

            return successfulPayment;
        }

        internal virtual bool CheckPrice(double price, DateTime date)
        {
            return true;
        }

        internal virtual bool SendEmail(string email)
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

        public ReservationResult Reserve(DateTime date, double price, int creditCardNumber, string email)
        {
            bool paymentMade = false;
            bool roomBooked = false;
            bool emailSent = false;
            bool success = true;
            bool priceValid = false;
            string reservationNumber = null;

            List<HotelOperation> sortedOperations = Operations.OrderBy(x => x.Order).ToList();

            foreach (HotelOperation currentOperation in sortedOperations)
            {
                bool stepSuccess = true;

                switch (currentOperation.Operation)
                {
                    case Operation.BookRoom:
                        roomBooked = BookRoom(date);
                        stepSuccess = roomBooked;
                        break;
                    case Operation.CheckPrice:
                        priceValid = CheckPrice(price, date);
                        stepSuccess = priceValid;
                        break;
                    case Operation.MakePayment:
                        paymentMade = MakePayment(creditCardNumber, price);
                        stepSuccess = paymentMade;
                        break;
                    case Operation.SendEmail:
                        emailSent = SendEmail(email);
                        stepSuccess = emailSent;
                        break;
                    case Operation.GenerateReservationNumber:
                        reservationNumber = GenerateReservationNumber();
                        break;
                }

                if (!stepSuccess && currentOperation.ShouldFailWholeProcess)
                {
                    success = false;
                    break;
                }
            }
            
            return new ReservationResult(success, reservationNumber, priceValid, roomBooked, paymentMade, emailSent);
        }
    }
}