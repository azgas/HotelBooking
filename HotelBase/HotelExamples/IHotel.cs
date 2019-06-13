using System.Collections.Generic;

namespace HotelBooking.HotelExamples
{
    public interface IHotel
    {
        List<HotelOperation> Operations { get; }
    }
}