using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.DTOs.Comments
{
    [Serializable]
    public class CommentDTO : BaseDTO
    {
        public long ID { get; set; }
        public long AuctionId { get; set; }
        public long AuthorId { get; set; }
        public long? ParentId { get; set; }

        [Required]
        [StringLength(2048,MinimumLength = 1)]
        public string Text { get; set; }
        
        [Required]
        public DateTime Time { get; set; }
        public bool HasParent => ParentId != null;
        public string AuthorName { get; set; }
    }
}
