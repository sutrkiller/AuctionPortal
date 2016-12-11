using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.DTOs.Items
{
    [Serializable]
    public class ItemDTO : BaseDTO
    {
        public long ID { get; set; }
        public long AuctionId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
