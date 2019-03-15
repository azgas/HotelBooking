using HotelBase;
using NUnit.Framework;

namespace HotelBookingTests
{
    [TestFixture]
    public class StringHelperTest
    {
        [Test]
        public void ShouldValidateEmail()
        {
            Assert.That(!StringHelper.IsValidEmail("test"));
            Assert.That(StringHelper.IsValidEmail("test@test.gmail"));
            
        }

    }
}
