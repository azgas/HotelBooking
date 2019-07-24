using System.Reflection;
using System.Web.Http;
using HotelBooking.BookingExternalService;
using HotelBooking.HotelFactory;
using HotelBooking.HotelManager;
using HotelBooking.Logger;
using HotelBooking.PaymentExternalService;
using Microsoft.Owin;
using Ninject;
using Ninject.Web.Common.OwinHost;
using Owin;
using Ninject.Web.WebApi.OwinHost;

[assembly: OwinStartup(typeof(HotelApi.Startup))]

namespace HotelApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}",
                defaults: new {controller = "Info"});
            app.UseWebApi(config);
            app.UseNinjectMiddleware(CreateKernel).UseNinjectWebApi(config);
        }

        private static StandardKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());
            kernel.Bind<IHotelFactory>().To<HotelFactory>();
            kernel.Bind<IPaymentService>().To<PaymentService>();
            kernel.Bind<IBookingService>().To<BookingService>();
            kernel.Bind<ILogger>().To<ConsoleLogger>();
            kernel.Bind<HotelManager>().ToProvider(new HotelManagerProvider());
            return kernel;
        }
    }
}