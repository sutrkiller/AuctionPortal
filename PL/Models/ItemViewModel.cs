using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BL.DTOs.ItemImages;
using BL.DTOs.Items;

namespace PL.Models
{
    public class ItemViewModel
    {
        public ItemDTO Item { get; set; }
        public IList<ItemImageDTO> Images { get; set; }
    }
}