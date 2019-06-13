using System;
using HotelBooking.HotelExamples;

namespace HotelBooking.ReservationService
{
    public interface IReservationService
    {
        ReservationResult Reserve(DateTime date, double price, int creditCardNumber, string email, IHotel hotel);
    }
}
