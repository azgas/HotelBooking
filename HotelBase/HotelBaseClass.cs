using System;
using System.Collections.Generic;
using HotelBooking;

namespace HotelBase
{
    public abstract class HotelBaseClass
    {
        internal readonly IReservationService ReservationService;

        protected HotelBaseClass(IReservationService reservationService)
        {
            ReservationService = reservationService;
        }

        public abstract List<HotelOperation> Operations { get; }
        
        public ReservationResult Reserve(DateTime date, double price, int creditCardNumber, string email)
        {
            return ReservationService.Reserve(date, price, creditCardNumber, email, Operations);
        }
    }
}