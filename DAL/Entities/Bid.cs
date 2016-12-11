using System;
using System.ComponentModel.DataAnnotations;
using Riganti.Utils.Infrastructure.Core;

namespace DAL.Entities
{
    public class Bid : IEntity<long>
    {
        public long ID { get; set; }
       // public long AuctionId { get; set; }
        //public long BidderId { get; set; }
        [Required]
        public DateTime BidTime { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        public decimal Value { get; set; }

        public Auction Auction { get; set; }
        public User User { get; set; }
    }
}
