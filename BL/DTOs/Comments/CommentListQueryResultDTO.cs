using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.DTOs.Common;
using BL.DTOs.Filters;

namespace BL.DTOs.Comments
{
    [Serializable]
    public class CommentListQueryResultDTO : PagedListQueryResultDTO<CommentDTO>
    {
        public CommentFilter Filter { get; set; }
    }
}
