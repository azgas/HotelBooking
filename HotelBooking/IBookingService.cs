using System;

namespace HotelBooking
{
    public interface IBookingService
    {
        bool Book(DateTime date);
    }
}