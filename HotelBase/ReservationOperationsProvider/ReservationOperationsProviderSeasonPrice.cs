using System;
using System.Collections.Generic;
using HotelBooking.BookingExternalService;
using HotelBooking.HotelExamples;
using HotelBooking.Logger;
using HotelBooking.Operations;
using HotelBooking.PaymentExternalService;

namespace HotelBooking.ReservationOperationsProvider
{
    class ReservationOperationsProviderSeasonPrice : IReservationOperationsProvider
    {
        internal readonly IBookingService BookingService;
        internal readonly IPaymentService PaymentService;
        internal readonly ILogger Logger;

        public ReservationOperationsProviderSeasonPrice(IBookingService bookingService,
            IPaymentService paymentService,
            ILogger logger)
        {
            BookingService = bookingService;
            PaymentService = paymentService;
            Logger = logger;
        }

        public KeyValuePair<string, bool> ProcessOperation(DateTime date,
            double price,
            int creditCardNumber,
            string email,
            HotelOperation operation)
        {
            bool stepSuccess = false;
            string operationDescription;
            switch (operation.Operation)
            {
                case Operation.BookRoom:
                    stepSuccess =
                        new RoomBookerSeason(date, price, creditCardNumber, email, BookingService, Logger).Execute();
                    operationDescription = OperationDescriptions.Booking;
                    break;
                case Operation.CheckPrice:
                    stepSuccess = new PriceCheckerSeason(date, price, creditCardNumber, email).Execute();
                    operationDescription = OperationDescriptions.PriceValidation;
                    break;
                case Operation.MakePayment:
                    stepSuccess = new Payment(date, price, creditCardNumber, email, PaymentService, Logger).Execute();
                    operationDescription = OperationDescriptions.Payment;
                    break;
                case Operation.SendEmail:
                    stepSuccess = new EmailSender(date, price, creditCardNumber, email, Logger).Execute();
                    operationDescription = OperationDescriptions.Email;
                    break;
                case Operation.GenerateReservationNumber:
                    stepSuccess = new ReservationNumberGeneratorSeason(date, price, creditCardNumber, email).Execute(out string reservationNumber);
                    operationDescription = OperationDescriptions.ReservationNumber + reservationNumber;
                    break;
                default:
                    Logger.Write(Messages.InvalidOperation);
                    operationDescription = OperationDescriptions.InvalidOperation;
                    break;
            }

            return new KeyValuePair<string, bool>(operationDescription, stepSuccess);
        }
    }
}