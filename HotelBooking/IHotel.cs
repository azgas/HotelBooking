using System;

namespace HotelBooking
{
    public interface IHotel
    {
        ReservationResult Reserve(DateTime date, double price, int creditCardNumber, string email);
    }
}