namespace HotelBooking
{
    public class ReservationResult
    {
        public bool Success { get; set; }
        public string ReservationNumber { get; set; }
        public bool PriceValidationSuccess { get; set; }
        public bool ReservationSuccess { get; set; }
        public bool PaymentSuccess { get; set; }
        public bool EmailSentSuccess { get; set; }

        public ReservationResult(bool success)
        {
            Success = success;
        }
    }
}