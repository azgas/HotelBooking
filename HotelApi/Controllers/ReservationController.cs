using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Results;
using HotelBooking;
using HotelBooking.BookingExternalService;
using HotelBooking.HotelFactory;
using HotelBooking.HotelManager;
using HotelBooking.Logger;
using HotelBooking.PaymentExternalService;

namespace HotelApi.Controllers
{
    public class ReservationController : ApiController
    {
        private readonly HotelManager _manager;

        public ReservationController()
        {
            IHotelFactory factory = new HotelFactory();
            IPaymentService paymentService = new PaymentService();
            IBookingService bookingService = new BookingService();
            ILogger logger = new ConsoleLogger();
            _manager = new HotelManager(factory, paymentService, bookingService, logger);
        }

        [HttpGet]
        public JsonResult<List<int>> GetAvailableHotelsIds()
        {
            return Json(_manager.PresentAvailableHotels());
        }

        [HttpPost]
        public JsonResult<ReservationResult> MakeReservation(int hotelId, double price, int creditCardNumber,
            string email, DateTime date)
        {
            return Json(_manager.MakeReservation(hotelId, price, creditCardNumber, email, date));
        }
    }
}