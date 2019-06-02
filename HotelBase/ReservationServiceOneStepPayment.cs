using System;
using System.Collections.Generic;
using System.Linq;
using HotelBooking;

namespace HotelBase
{
    public class ReservationServiceOneStepPayment : IReservationService
    {
        internal readonly IBookingService BookingService;
        internal readonly IPaymentService PaymentService;
        internal readonly ILogger Logger;

        public ReservationServiceOneStepPayment(IBookingService bookingService, IPaymentService paymentService,
            ILogger logger)
        {
            BookingService = bookingService;
            PaymentService = paymentService;
            Logger = logger;
        }

        public virtual ReservationResult Reserve(DateTime date, double price, int creditCardNumber, string email,
            List<HotelOperation> operations)
        {
            bool paymentMade = false;
            bool roomBooked = false;
            bool emailSent = false;
            bool success = true;
            bool priceValid = false;
            string reservationNumber = null;

            List<HotelOperation> sortedOperations = operations.OrderBy(x => x.Order).ToList();

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
                        paymentMade = PaymentService.Pay(creditCardNumber, price);
                        if(!paymentMade)
                            Logger.Write(Messages.PaymentFail);
                        stepSuccess = paymentMade;
                        break;
                    case Operation.SendEmail:
                        emailSent = CommonHotelOperations.SendEmail(email);
                        if(!emailSent)
                            Logger.Write(Messages.InvalidEmail);
                        stepSuccess = emailSent;
                        break;
                    case Operation.GenerateReservationNumber:
                        reservationNumber = GenerateReservationNumber();
                        break;
                    default:
                        Logger.Write(Messages.InvalidOperation);
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

        protected virtual bool BookRoom(DateTime date)
        {
            return BookingService.Book(date);
        }

        protected virtual bool CheckPrice(double price, DateTime date)
        {
            return CommonHotelOperations.CheckPrice(price, date);
        }

        protected virtual string GenerateReservationNumber()
        {
return CommonHotelOperations.GenerateReservationNumber();
        }
    }
}