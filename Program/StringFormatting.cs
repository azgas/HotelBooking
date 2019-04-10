using System.Text;
using HotelBooking;

namespace Program
{
    public static class StringFormatting
    {
        public static string FormatReservationResult(ReservationResult resResult)
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
