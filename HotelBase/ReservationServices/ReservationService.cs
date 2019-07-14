using System;
using System.Collections.Generic;
using System.Linq;
using HotelBooking.HotelExamples;
using HotelBooking.ReservationOperationsProvider;

namespace HotelBooking.ReservationServices
{
    public class ReservationService : IReservationService
    {
        internal readonly IReservationOperationsProvider OperationsProvider;

        public ReservationService(IReservationOperationsProvider operationsProvider)
        {
            OperationsProvider = operationsProvider;
        }

        public virtual ReservationResult Reserve(DateTime date, double price, int creditCardNumber, string email,
            IHotel hotel)
        {
            bool success = true;

            List<KeyValuePair<string, bool>> results = new List<KeyValuePair<string, bool>>();

            List<HotelOperation> sortedOperations = hotel.Operations.OrderBy(x => x.Order).ToList();

            foreach (HotelOperation currentOperation in sortedOperations)
            {
                KeyValuePair<string, bool> result =
                    OperationsProvider.ProcessOperation(date, price, creditCardNumber, email, currentOperation);
                results.Add(result);

                if (!result.Value && currentOperation.ShouldFailWholeProcess)
                {
                    success = false;
                    break;
                }
            }

            return new ReservationResult(success, results);
        }
    }
}