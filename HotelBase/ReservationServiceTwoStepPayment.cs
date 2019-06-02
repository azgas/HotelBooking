using System;
using System.Collections.Generic;
using System.Linq;
using HotelBooking;

namespace HotelBase
{
    class ReservationServiceTwoStepPayment : IReservationService
    {
        internal readonly IBookingService BookingService;
        internal readonly IPaymentServiceTwoStep PaymentService;
        internal readonly ILogger Logger;

        public ReservationServiceTwoStepPayment(IBookingService bookingService, IPaymentServiceTwoStep paymentService, ILogger logger)
        {
            BookingService = bookingService;
            PaymentService = paymentService;
            Logger = logger;
        }
        public  ReservationResult Reserve(DateTime date, double price, int creditCardNumber, string email, List<HotelOperation> operations)
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
                        roomBooked = BookingService.Book(date);
                        stepSuccess = roomBooked;
                        break;
                    case Operation.CheckPrice:
                        priceValid = CommonHotelOperations.CheckPrice(price, date);
                        stepSuccess = priceValid;
                        break;
                    case Operation.Authorization:
                        paymentMade = PaymentService.Authorize(creditCardNumber);
                        stepSuccess = paymentMade;
                        break;
                    case Operation.Capture:
                        paymentMade = PaymentService.Capture(price);
                        stepSuccess = paymentMade;
                        break;
                    case Operation.SendEmail:
                        emailSent = CommonHotelOperations.SendEmail(email);
                        stepSuccess = emailSent;
                        break;
                    case Operation.GenerateReservationNumber:
                        reservationNumber = CommonHotelOperations.GenerateReservationNumber();
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
    }
}