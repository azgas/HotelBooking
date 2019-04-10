using System;
using System.Collections.Generic;
using HotelBooking;

namespace HotelBase
{
    public class HotelFactory : IHotelFactory
    {
        private readonly Dictionary<int, Type> _hotelTypesDictionary = new Dictionary<int, Type>
            {{1, typeof(HotelExampleEmailCanFail)}, {2, typeof(HotelExample)}};

        public IHotel ReturnHotel(int id, IBookingService bookingService, IPaymentService paymentService,
            ILogger logger)
        {
            if (_hotelTypesDictionary.TryGetValue(id, out var type))
            {
                return (IHotel) Activator.CreateInstance(type, bookingService, paymentService, logger);
            }

            return null;
        }

        public List<int> GetHotelIds()
        {
            return new List<int>(_hotelTypesDictionary.Keys);
        }
    }
}