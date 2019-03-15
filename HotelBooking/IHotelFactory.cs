namespace HotelBooking
{
    public interface IHotelFactory
    {
        IHotel ReturnHotel(int id);
    }
}
