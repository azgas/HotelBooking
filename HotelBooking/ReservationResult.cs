namespace HotelBooking
{
    public class ReservationResult
    {
        public bool Success { get; }
        public string ReservationNumber { get; }
        public bool PriceValidationSuccess { get; }
        public bool ReservationSuccess { get; }
        public bool PaymentSuccess { get; }
        public bool EmailSentSuccess { get; }

        public ReservationResult(bool success, string reservationNumber = null, bool priceValidationSuccess = false,
            bool reservationSuccess = false, bool paymentSuccess = false, bool emailSentSuccess = false)
        {
            Success = success;
            ReservationNumber = reservationNumber;
            PriceValidationSuccess = priceValidationSuccess;
            ReservationSuccess = reservationSuccess;
            PaymentSuccess = paymentSuccess;
            EmailSentSuccess = emailSentSuccess;
        }
    }
}