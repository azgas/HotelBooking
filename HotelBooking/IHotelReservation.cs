using System;

namespace HotelBooking
{
    public interface IHotelReservation
    {
        ReservationResult MakeReservation(int hotelId, double price, int creditCardNumber, string email, DateTime date);
    }
}