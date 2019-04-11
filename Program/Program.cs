using System;

namespace Program
{
    class Program
    {
        static void Main(string[] args)
        {
            ReservationOperations reservation = new ReservationOperations();

            string idsString = reservation.GetAvailableHotelIds();
            Console.WriteLine("Please write hotel ID, available hotels in base: " + idsString);
            bool correctIdFormat = Int32.TryParse(Console.ReadLine(), out int id);

            if (correctIdFormat)
            {
                string result = reservation.MakeReservation(id);
                Console.WriteLine(result);
                Console.ReadKey();
                return;
            }

            Console.WriteLine("ID must be a number.");
            Console.ReadKey();
        }
    }
}