using System;
using System.Text;
using HotelBase;
using HotelBooking;

namespace Program
{
    class Program
    {
        static void Main(string[] args)
        {
            IHotelFactory factory = new HotelFactory();
            IPaymentService paymentService = new PaymentService();
            IBookingService bookingService = new BookingService();
            IHotelReservation manager = new HotelManager(factory, paymentService, bookingService);

            ReservationResult result = manager.MakeReservation(20, 200, 2222, "test@test2.com");
            Console.WriteLine(FormatReservationResult(result));
            Console.ReadLine();
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

            return result.ToString();
        }
    }
}
