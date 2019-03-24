namespace HotelBooking
{
    public interface ILogger
    {
        void Write(string message);
        void WaitForUserInput();
    }
}