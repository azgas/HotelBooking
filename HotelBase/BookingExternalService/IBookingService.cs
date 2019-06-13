using System;

namespace HotelBooking.BookingExternalService
{
    public interface IBookingService
    {
        bool Book(DateTime date);
    }
}