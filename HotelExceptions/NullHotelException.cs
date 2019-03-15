using System;

namespace HotelExceptions
{
    public class NullHotelException : Exception
    {
        public NullHotelException(string message) : base(message)
        {
        }
    }
}