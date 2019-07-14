using System;

namespace HotelBooking.Logger
{
    public class ConsoleLogger : ILogger
    {
        public void Write(string message)
        {
            if (message != null)
                Console.WriteLine(message);
        }
    }
}