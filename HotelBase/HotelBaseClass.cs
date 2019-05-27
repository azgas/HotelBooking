using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using HotelBooking;

namespace HotelBase
{
    public abstract class HotelBaseClass
    {
        private readonly IBookingService _bookingService;
        internal readonly IPaymentService PaymentService;
        internal readonly ILogger Logger;

        protected HotelBaseClass(IBookingService bookingService, IPaymentService paymentService, ILogger logger)
        {
            _bookingService = bookingService;
            PaymentService = paymentService;
            Logger = logger;
        }

        public abstract List<HotelOperation> Operations { get; }

        internal virtual bool MakePayment(int creditCardNumber, double price, Operation operation = Operation.MakePayment)
        {
            var successfulPayment = PaymentService.Pay(creditCardNumber, price);

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
                    case Operation.Authorization:
                    case Operation.Capture:
                        paymentMade = MakePayment(creditCardNumber, price, currentOperation.Operation);
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