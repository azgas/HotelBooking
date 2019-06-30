using System;
using HotelBooking.ReservationOperationsProvider;

namespace HotelBooking.Operations
{
    class ReservationNumberGeneratorSeason : OperationBase
    {
        public ReservationNumberGeneratorSeason(DateTime date, double price, int creditCardNumber, string email) : base(date, price, creditCardNumber, email)
        {
        }

        public override bool Execute()
        {
            return true;
        }

        public bool Execute(out string reservationNumber)
        {
            reservationNumber = StringHelper.GenerateRandomString("01");
            return true;
        }
    }
}