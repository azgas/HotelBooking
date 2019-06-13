namespace HotelBooking.HotelExamples
{
    public class HotelOperation
    {
        public HotelOperation(Operation operation, int order, bool shouldFailWholeProcess)
        {
            Operation = operation;
            Order = order;
            ShouldFailWholeProcess = shouldFailWholeProcess;
        }

        public Operation Operation { get; }
        public int Order { get; }
        public bool ShouldFailWholeProcess { get; }
    }

    public enum Operation { BookRoom, SendEmail, MakePayment, CheckPrice, GenerateReservationNumber, Capture, Authorization}
}
