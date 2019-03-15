namespace HotelBooking
{
    public interface IHotelReservation
    {
        ReservationResult MakeReservation(int hotelID, double price, int creditCardNumber, string email);
    }
}