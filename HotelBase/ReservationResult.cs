using System.Collections.Generic;

namespace HotelBooking
{
    public class ReservationResult
    {
        public bool Success { get; }
        public List<KeyValuePair<string, bool>> OperationsResult { get; }

        public ReservationResult(bool success, List<KeyValuePair<string, bool>> operationsResult)
        {
            Success = success;
            OperationsResult = operationsResult;
        }

        public ReservationResult(bool success)
        {
            Success = success;
        }
    }
}