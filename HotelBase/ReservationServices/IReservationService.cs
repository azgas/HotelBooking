using System;
using HotelBooking.HotelExamples;

namespace HotelBooking.ReservationServices
{
    public interface IReservationService
    {
        ReservationResult Reserve(DateTime date, double price, int creditCardNumber, string email, IHotel hotel);
    }
}
