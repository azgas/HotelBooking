using System;
using HotelExceptions;

namespace HotelBooking
{
    public class HotelManager : IHotelReservation
    {
        private readonly IHotelFactory _factory;

        public IHotel Hotel { get; set; }
        public HotelManager(IHotelFactory factory)
        {
            _factory = factory;
        }

        public ReservationResult MakeReservation(int hotelID, DateTime date, double price, int creditCardNumber,
            string email)
        {
            try
            {
                FindHotel(hotelID);
            }
            catch (NullHotelException)
            {
                return new ReservationResult {Success = false};
            }

            Hotel.Reserve(date, price, creditCardNumber, email);

            return new ReservationResult{Success = true};
        }

        internal void FindHotel(int hotelID)
        {
            Hotel = _factory.ReturnHotel(hotelID);
            if (Hotel == null)
            {
                throw new NullHotelException($"Couldn't find hotel with specified ID: {hotelID}");
            }
        }
    }
}