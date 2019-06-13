using System.Text;
using HotelBooking;

namespace Program
{
    public static class ReservationResultTextFormatter
    {
        public static string FormatReservationResult(ReservationResult resResult)
        {
            StringBuilder result = new StringBuilder();
            result.AppendLine();
            result.Append("Process result: ");
            result.Append(resResult.Success);
            foreach (var operationResult in resResult.OperationsResult)
            {
                result.AppendLine();
                result.Append(operationResult.Key + ": ");
                result.Append(operationResult.Value);
            }
            return result.ToString();
        }
    }
}
