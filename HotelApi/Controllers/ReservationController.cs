using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Results;
using HotelBooking;
using HotelBooking.HotelManager;

namespace HotelApi.Controllers
{
    public class ReservationController : ApiController
    {
        private readonly HotelManager _manager;

        public ReservationController(HotelManager manager)
        {
            _manager = manager;
        }

        public JsonResult<List<int>> GetAvailableHotelsIds()
        {
            return Json(_manager.PresentAvailableHotels());
        }

        public JsonResult<ReservationResult> PostReservation([FromBody] ReservationRequest request)
        {
            return Json(_manager.MakeReservation(request.HotelId, request.Price, request.CreditCardNumber,
                request.Email, request.Date));
        }
    }
}