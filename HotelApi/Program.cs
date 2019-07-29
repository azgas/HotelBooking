using System;
using Microsoft.Owin.Hosting;

namespace HotelApi
{
    class Program
    {
        static void Main(string[] args)
        {
            string Url = "http://localhost:9000";

            using (WebApp.Start<Startup>(Url))
            {
                Console.WriteLine("Server started at: " + Url);
                Console.ReadKey();
            }

        }
    }
}
