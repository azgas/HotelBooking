using System;
using System.Collections.Generic;
using HotelBooking.BookingExternalService;
using HotelBooking.HotelExamples;
using HotelBooking.Logger;
using HotelBooking.PaymentExternalService;

namespace HotelBooking.ReservationOperationsProvider
{
    class ReservationOperationsProviderTwoStepPayment : ReservationOperationsProviderCommonFunctions,
        IReservationOperationsProvider
    {
        internal readonly IBookingService BookingService;
        internal readonly IPaymentServiceTwoStep PaymentService;
        internal readonly ILogger Logger;

        public ReservationOperationsProviderTwoStepPayment(IBookingService bookingService,
            IPaymentServiceTwoStep paymentService, ILogger logger)
        {
            BookingService = bookingService;
            PaymentService = paymentService;
            Logger = logger;
        }

        public KeyValuePair<string, bool> ProcessOperation(DateTime date, double price, int creditCardNumber,
            string email,
            HotelOperation operation)
        {
            bool stepSuccess = false;
            string operationDescription = "";
            switch (operation.Operation)
            {
                case Operation.BookRoom:
                    stepSuccess = BookRoom(date);
                    operationDescription = OperationDescriptions.Booking;
                    break;
                case Operation.CheckPrice:
                    stepSuccess = CheckPrice(price, date);
                    operationDescription = OperationDescriptions.PriceValidation;
                    break;
                case Operation.Authorization:
                    stepSuccess = PaymentService.Authorize(creditCardNumber);
                    operationDescription = OperationDescriptions.Authorization;
                    break;
                case Operation.Capture:
                    stepSuccess = PaymentService.Capture(price);
                    operationDescription = OperationDescriptions.Capture;
                    break;
                case Operation.SendEmail:
                    stepSuccess = SendEmail(email);
                    if (!stepSuccess)
                        Logger.Write(Messages.InvalidEmail);
                    operationDescription = OperationDescriptions.Email;
                    break;
                case Operation.GenerateReservationNumber:
                    //TODO
                    /*reservationNumber = GenerateReservationNumber();*/
                    break;
                default:
                    Logger.Write(Messages.InvalidOperation);
                    operationDescription = OperationDescriptions.InvalidOperation;
                    break;
            }

            return new KeyValuePair<string, bool>(operationDescription, stepSuccess);
        }

        private bool BookRoom(DateTime date)
        {
            return BookingService.Book(date);
        }

        private string GenerateReservationNumber()
        {
            return StringHelper.GenerateRandomString("0");
        }
    }
}