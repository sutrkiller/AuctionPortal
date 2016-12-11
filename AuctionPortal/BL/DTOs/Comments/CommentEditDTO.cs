using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.DTOs.Comments
{
    [Serializable]
    public class CommentEditDTO : BaseDTO
    {
        public long ID { get; set; }

        [Required]
        [StringLength(2048,MinimumLength = 2)]
        public string Text { get; set; }
        
    }
}
