using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BL.DTOs.ItemImages;
using BL.DTOs.Items;

namespace PL.Models
{
    public class ItemCreateViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Images { get; set; }
    }
}