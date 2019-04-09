using System;
using System.Collections.Generic;
using System.Text;
using HotelBase;
using HotelBooking;

namespace Program
{
    class Program
    {
        private static HotelManager _manager;
        private static ILogger _logger;

        static void Main(string[] args)
        {
            SetupObjects();
            bool correctIdFormat = Int32.TryParse(GetUserChoice(), out int id);
            if (correctIdFormat)
            {
                MakeReservation(id);
                return;
            }

            Console.WriteLine("ID must be a number.");
            Console.ReadKey();
        }

        private static void MakeReservation(int id)
        {
          try
          {
            ReservationResult result = _manager.MakeReservation(id, 200, 2222, "test@test2.com", DateTime.Today);
            Console.WriteLine(FormatReservationResult(result));
          }
          catch (Exception e)
          {
            Console.WriteLine(e.Message);
          }
          
          Console.ReadKey();
        }

        private static void SetupObjects()
        {
            IHotelFactory factory = new HotelFactory();
            IPaymentService paymentService = new PaymentService();
            IBookingService bookingService = new BookingService();
            _logger = new ConsoleLogger();
            _manager = new HotelManager(factory, paymentService, bookingService, _logger);
        }

        private static string GetUserChoice()
        {
            List<int> availableIds = _manager.PresentAvailableHotels();
            List<string> idsStringList = availableIds.ConvertAll(x => x.ToString());
            string idsString = string.Join(", ", idsStringList.ToArray());
            Console.WriteLine("Please write hotel ID, available hotels in base: " + idsString);
            return Console.ReadLine();
        }

        private static string FormatReservationResult(ReservationResult resResult)
        {
            StringBuilder result = new StringBuilder();
            result.AppendLine();
            result.Append("Process result: ");
            result.Append(resResult.Success);
            result.AppendLine();
            result.Append("Payment made: ");
            result.Append(resResult.PaymentSuccess);
            result.AppendLine();
            result.Append("Email sent: ");
            result.Append(resResult.EmailSentSuccess);
            result.AppendLine();
            result.Append("Price valid: ");
            result.Append(resResult.PriceValidationSuccess);
            result.AppendLine();
            result.Append("Room booked: ");
            result.Append(resResult.ReservationSuccess);
            result.AppendLine();
            if (resResult.ReservationNumber != null)
            {
                result.Append("Reservation number: ");
                result.Append(resResult.ReservationNumber);
            }

            return result.ToString();
        }
    }
}