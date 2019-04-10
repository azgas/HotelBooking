using System;
using System.Collections.Generic;
using HotelBase;
using HotelBooking;

namespace Program
{
    public class ReservationOperations
    {
        private static HotelManager _manager;

        public ReservationOperations()
        {
            IHotelFactory factory = new HotelFactory();
            IPaymentService paymentService = new PaymentService();
            IBookingService bookingService = new BookingService();
            ILogger logger = new ConsoleLogger();
            _manager = new HotelManager(factory, paymentService, bookingService, logger);
        }

        public string GetAvailableHotelIds()
        {
            List<int> availableIds = _manager.PresentAvailableHotels();
            List<string> idsStringList = availableIds.ConvertAll(x => x.ToString());
            string idsString = string.Join(", ", idsStringList.ToArray());
            return idsString;
        }

        public string MakeReservation(int id)
        {
            try
            {
                ReservationResult result = _manager.MakeReservation(id, 200, 2222, "test@test2.com", DateTime.Today);
                return StringFormatting.FormatReservationResult(result);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}