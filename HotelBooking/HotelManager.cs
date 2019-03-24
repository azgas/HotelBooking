using System;

namespace HotelBooking
{
    public class HotelManager : IHotelReservation
    {
        private readonly IHotelFactory _factory;
        private readonly IPaymentService _paymentService;
        private readonly IBookingService _bookingService;
        private readonly ILogger _logger;

        public IHotel Hotel { get; set; }
        public HotelManager(IHotelFactory factory, IPaymentService paymentService, IBookingService bookingService, ILogger logger)
        {
            _factory = factory;
            _paymentService = paymentService;
            _bookingService = bookingService;
            _logger = logger;
        }

        public ReservationResult MakeReservation(int hotelId, double price, int creditCardNumber,
            string email, DateTime date)
        {

             bool hotelFound =  FindHotel(hotelId);

            if(!hotelFound)
            {
                _logger.Write($"Couldn't find hotel with ID: {hotelId}");
                return new ReservationResult( false);
            }

            return Hotel.Reserve(date, price, creditCardNumber, email);

        }

        internal bool FindHotel(int hotelId)
        {
            Hotel = _factory.ReturnHotel(hotelId, _bookingService, _paymentService, _logger);
            return Hotel != null;
        }
    }
}