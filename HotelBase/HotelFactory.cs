using HotelBooking;

namespace HotelBase
{
    public class HotelFactory : IHotelFactory
    {
        public IHotel ReturnHotel(int id)
        {
            switch (id)
            {
                case 1:
                    return new HotelOne();
                case 2:
                    return new HotelTwo();
                default:
                    return null;
            }
        }
    }
}