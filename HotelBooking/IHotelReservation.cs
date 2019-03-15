using System;

namespace HotelBooking
{
    public interface IHotelReservation
    {
        ReservationResult MakeReservation(int hotelID, DateTime date, double price, int creditCardNumber, string email);
    }
}