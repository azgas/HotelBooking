using System;
using System.Collections.Generic;
using HotelBooking.HotelExamples;

namespace HotelBooking.ReservationOperationsProvider
{
    public interface IReservationOperationsProvider
    {
        KeyValuePair<string, bool> ProcessOperation(DateTime date, 
            double price,
            int creditCardNumber,
            string email,
            HotelOperation operation);
    }
}
