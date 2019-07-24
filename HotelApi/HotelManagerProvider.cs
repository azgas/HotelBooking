using HotelBooking.BookingExternalService;
using HotelBooking.HotelFactory;
using HotelBooking.HotelManager;
using HotelBooking.Logger;
using HotelBooking.PaymentExternalService;
using Ninject;
using Ninject.Activation;

namespace HotelApi
{
    class HotelManagerProvider : Provider<HotelManager>
    {
        protected override HotelManager CreateInstance(IContext context)
        {
            return new HotelManager(context.Kernel.Get<IHotelFactory>(), context.Kernel.Get<IPaymentService>(),
                context.Kernel.Get<IBookingService>(), context.Kernel.Get<ILogger>());
        }
    }
}