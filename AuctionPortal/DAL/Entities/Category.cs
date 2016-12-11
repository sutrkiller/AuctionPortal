using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using Riganti.Utils.Infrastructure.Core;

namespace DAL.Entities
{
    public class Category : IEntity<long>
    {
        public long ID { get; set; }
        [Required]
        [StringLength(30)]
        [Index(IsUnique = true)]
        public string Name { get; set; }
        public virtual ICollection<Auction> Auctions { get; set; } = new HashSet<Auction>();

    }
}
