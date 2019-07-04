using System;
using Microsoft.Owin.Hosting;

namespace HotelApi
{
    class Program
    {
        const string Url = "http://localhost:9000";
        static void Main(string[] args)
        {
            using (WebApp.Start<Startup>(Url))
            {
                Console.WriteLine("Server started at: " + Url);
                Console.ReadKey();
            }

        }
    }
}
