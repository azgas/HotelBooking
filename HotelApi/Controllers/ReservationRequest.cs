using System;
using System.Runtime.Serialization;

namespace HotelApi.Controllers
{
    [DataContract]
    public class ReservationRequest
    {
        [DataMember(Name = "hotel_id")]
        public int HotelId { get; set; }

        [DataMember(Name = "price")]
        public double Price { get; set; }

        [DataMember(Name = "credit_card_number")]
        public int CreditCardNumber { get; set; }

        [DataMember(Name = "email")]
        public string Email { get; set; }

        [DataMember(Name = "date")]
        public DateTime Date { get; set; }
    }
}