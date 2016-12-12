using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.DTOs.Comments
{
    [Serializable]
    public class CommentCreateDTO : BaseDTO
    {
        [Required]
        public long AuctionId { get; set; }
        [Required]
        public long AuthorId { get; set; }
        public long? ParentId { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public DateTime Time { get; set; }
        public bool HasParent => ParentId != null;
        public long ReturnPage { get; set; }
    }
}
