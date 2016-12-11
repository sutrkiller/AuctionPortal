using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.DTOs.ItemImages
{
    [Serializable]
    public class ItemImageDTO : BaseDTO
    {
        public long ID { get; set; }
        public long ItemId { get; set; }
        public string ImagePath { get; set; }
    }
}
