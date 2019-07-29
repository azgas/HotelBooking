using System.Web.Http;
using System.Web.Http.Results;

namespace HotelApi.Controllers
{
    public class InfoController : ApiController
    {
        [HttpGet]
        public JsonResult<string> Get()
        {
            return Json("Hello world");
        }
    }
}
