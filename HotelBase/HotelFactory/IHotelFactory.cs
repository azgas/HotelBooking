using System.Collections.Generic;
using HotelBooking.HotelExamples;

namespace HotelBooking.HotelFactory
{
    public interface IHotelFactory
    {
        IHotel ReturnHotel(int id);

        List<int> GetHotelIds();
    }
}