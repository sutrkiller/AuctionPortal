using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.DTOs.Categories
{
    [Serializable]
    public class CategoryDTO : BaseDTO
    {
        public long ID { get; set; }

        [Required]
        [MaxLength(255)]
        [Display(Name = "Category")]
        public string Name { get; set; }

        public bool IsActive { get; set; }
    }
}
