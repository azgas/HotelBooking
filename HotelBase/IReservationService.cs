using System;
using System.Collections.Generic;
using HotelBooking;

namespace HotelBase
{
    public interface IReservationService
    {
        ReservationResult Reserve(DateTime date, double price, int creditCardNumber, string email, List<HotelOperation> operations);
    }
}
