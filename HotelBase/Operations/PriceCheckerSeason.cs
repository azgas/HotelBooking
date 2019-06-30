using System;

namespace HotelBooking.Operations
{
    public class PriceCheckerSeason : OperationBase
    {
        private const double SeasonPrice = 250;
        private const double PriceAfterSeason = 200;
        public PriceCheckerSeason(DateTime date, double price, int creditCardNumber, string email) : base(date, price, creditCardNumber, email)
        {
        }

        public override bool Execute()
        {
            var actualPrice = PriceAfterSeason;

            if (Date.Month > 5 && Date.Month < 9)
                actualPrice = SeasonPrice;

            return Math.Abs(Price - actualPrice) < 0.01;
        }
    }
}