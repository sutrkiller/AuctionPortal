using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.DTOs.Comments;
using BL.DTOs.Filters;

namespace BL.Services.Comments
{
    public interface ICommentService
    {
        int CommentsPageSize { get; }

        void CreateComment(CommentCreateDTO commentDTO);

        void EditComment(CommentEditDTO commentDTO);

        void DeleteComment(long commentId);

        CommentDTO GetComment(long commentId);

        //IEnumerable<CommentDTO> GetAllComments();
        //IEnumerable<CommentDTO> GetAllCommentsOfAuthor(long authorId);
        //IEnumerable<CommentDTO> GetAllCommentsOfAuction(long auctionId);
        //IEnumerable<CommentDTO> GetAllChildComments(long parentId);
        //IEnumerable<CommentDTO> GetAllCommentsContainting(string text);

        CommentListQueryResultDTO GetComments(CommentFilter filter, int requiredPage = 1);
    }
}
