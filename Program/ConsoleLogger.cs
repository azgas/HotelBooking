using System;
using HotelBooking;

namespace Program
{
    class ConsoleLogger : ILogger
    {
        public void Write(string message)
        {
            if(message!= null)
                Console.WriteLine(message);
        }

        public void WaitForUserInput()
        {
            Console.ReadLine();
        }
    }
}
