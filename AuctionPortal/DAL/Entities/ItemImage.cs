using System.ComponentModel.DataAnnotations;
using Riganti.Utils.Infrastructure.Core;

namespace DAL.Entities
{
    public class ItemImage : IEntity<long>
    {
        public long ID { get; set; }
        //public long ItemId { get; set; }
        //[Required]
        //public byte[] Image { get; set; }

        public string  ImagePath { get; set; }

        public virtual Item Item { get; set; }

    }
}