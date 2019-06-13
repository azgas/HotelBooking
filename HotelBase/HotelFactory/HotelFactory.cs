using System;
using System.Collections.Generic;
using HotelBooking.HotelExamples;

namespace HotelBooking.HotelFactory
{
    public class HotelFactory : IHotelFactory
    {
        private readonly Dictionary<int, Type> _hotelTypesDictionary = new Dictionary<int, Type>
            {{1, typeof(HotelExampleEmailCanFail)}, {2, typeof(HotelExample)}};

        public IHotel ReturnHotel(int id)
        {
            if (_hotelTypesDictionary.TryGetValue(id, out var type))
            {
                return (IHotel) Activator.CreateInstance(type);
            }

            return null;
        }

        public List<int> GetHotelIds()
        {
            return new List<int>(_hotelTypesDictionary.Keys);
        }
    }
}