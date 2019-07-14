﻿using System;
using System.Collections.Generic;
using HotelBooking.BookingExternalService;
using HotelBooking.HotelExamples;
using HotelBooking.Logger;
using HotelBooking.Operations;
using HotelBooking.PaymentExternalService;

namespace HotelBooking.ReservationOperationsProvider
{
    class ReservationOperationsProviderTwoStepPayment : IReservationOperationsProvider
    {
        internal readonly IBookingService BookingService;
        internal readonly IPaymentServiceTwoStep PaymentService;
        internal readonly ILogger Logger;

        public ReservationOperationsProviderTwoStepPayment(IBookingService bookingService,
            IPaymentServiceTwoStep paymentService,
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
                    stepSuccess = new RoomBooker(date, price, creditCardNumber, email, BookingService).Execute();
                    operationDescription = OperationDescriptions.Booking;
                    break;
                case Operation.CheckPrice:
                    stepSuccess = new PriceChecker(date, price, creditCardNumber, email).Execute();
                    operationDescription = OperationDescriptions.PriceValidation;
                    break;
                case Operation.Authorization:
                    stepSuccess = new PaymentAuthorization(date, price, creditCardNumber, email, PaymentService).Execute();
                    operationDescription = OperationDescriptions.Authorization;
                    break;
                case Operation.Capture:
                    stepSuccess = new PaymentCapture(date, price, creditCardNumber, email, PaymentService).Execute();
                    operationDescription = OperationDescriptions.Capture;
                    break;
                case Operation.SendEmail:
                    stepSuccess = new EmailSender(date, price, creditCardNumber, email, Logger).Execute();
                    operationDescription = OperationDescriptions.Email;
                    break;
                case Operation.GenerateReservationNumber:
                    stepSuccess = new ReservationNumberGenerator(date, price, creditCardNumber, email).Execute(out string reservationNumber);
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