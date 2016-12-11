using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.DTOs.ItemImages;

namespace BL.DTOs.Items
{
    public class ItemCreateDTO
    {
        public ItemDTO Item { get; set; }
        public ICollection<ItemImageDTO> Images { get; set; }
    }
}
