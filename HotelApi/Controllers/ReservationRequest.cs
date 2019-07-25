using System;
using System.Runtime.Serialization;

namespace HotelApi.Controllers
{
    [DataContract]
    public class ReservationRequest
    {
        [DataMember(Name = "hotel_id", IsRequired = true)]
        public int HotelId { get; set; }

        [DataMember(Name = "price", IsRequired = true)]
        public double Price { get; set; }

        [DataMember(Name = "credit_card_number", IsRequired = true)]
        public int CreditCardNumber { get; set; }

        [DataMember(Name = "email", IsRequired = true)]
        public string Email { get; set; }

        [DataMember(Name = "date", IsRequired = true)]
        public DateTime Date { get; set; }
    }
}