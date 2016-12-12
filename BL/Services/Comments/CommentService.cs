using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BL.DTOs.Comments;
using BL.DTOs.Filters;
using BL.Queries;
using BL.Repositories;
using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;

namespace BL.Services.Comments
{
    public class CommentService : BaseService, ICommentService
    {
        #region Dependencies

        private readonly CommentRepository _commentRepository;
        private readonly AuctionRepository _auctionRepository;
        private readonly UserRepository _userRepository;
        private readonly CommentListQuery _commentListQuery;

        public CommentService(CommentRepository commentRepository, CommentListQuery commentListQuery, AuctionRepository auctionRepository, UserRepository userRepository)
        {
            _commentRepository = commentRepository;
            _commentListQuery = commentListQuery;
            _auctionRepository = auctionRepository;
            _userRepository = userRepository;
        }

        #endregion

        public int CommentsPageSize => 5;

        public void CreateComment(CommentCreateDTO commentDTO)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var comment = Mapper.Map<Comment>(commentDTO);
                comment.Auction = GetCommentAuction(commentDTO.AuctionId);
                comment.Author = GetCommentAuthor(commentDTO.AuthorId);
                comment.ParentComment = _commentRepository.GetById(commentDTO.ParentId ?? 0);

                _commentRepository.Insert(comment);
                uow.Commit();
            }
        }

        public void EditComment(CommentEditDTO commentDTO)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var comment = _commentRepository.GetById(commentDTO.ID);
                if (comment == null)
                {
                    throw new NullReferenceException($"{this.GetType().Name} - {nameof(EditComment)} comment cannot be null.");
                }
                Mapper.Map(commentDTO, comment);
                _commentRepository.Update(comment);
                uow.Commit();
            }
        }

        public void DeleteComment(long commentId)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var comment = _commentRepository.GetById(commentId);
                if (comment == null) throw new NullReferenceException($"{this.GetType().Name} - {nameof(DeleteComment)} comment cannot be null.");
                for (int i = comment.ChildComments.Count - 1; i >= 0; i--)
                {
                    _commentRepository.Delete(comment.ChildComments.ElementAt(i).ID);
                }
                _commentRepository.Delete(comment);
                uow.Commit();
            }
            
        }

        public CommentDTO GetComment(long commentId)
        {
            using (UnitOfWorkProvider.Create())
            {
                var comment = _commentRepository.GetById(commentId);
                return comment != null ? Mapper.Map<CommentDTO>(comment) : null;
            }
        }

        //public IEnumerable<CommentDTO> GetAllComments()
        //{
        //    using (UnitOfWorkProvider.Create())
        //    {
        //        _commentListQuery.Filter = null;
        //        return _commentListQuery.Execute() ?? new List<CommentDTO>();
        //    }
        //}

        //public IEnumerable<CommentDTO> GetAllCommentsOfAuthor(long authorId)
        //{
        //    using (UnitOfWorkProvider.Create())
        //    {
        //        _commentListQuery.Filter = new CommentFilter() {AuthorId = authorId};
        //        return _commentListQuery.Execute() ?? new List<CommentDTO>();
        //    }
        //}

        //public IEnumerable<CommentDTO> GetAllCommentsOfAuction(long auctionId)
        //{
        //    using (UnitOfWorkProvider.Create())
        //    {
        //        _commentListQuery.Filter = new CommentFilter() { AuctionId = auctionId };
        //        return _commentListQuery.Execute() ?? new List<CommentDTO>();
        //    }
        //}

        //public IEnumerable<CommentDTO> GetAllChildComments(long parentId)
        //{
        //    using (UnitOfWorkProvider.Create())
        //    {
        //        _commentListQuery.Filter = new CommentFilter() { ParentId = parentId };
        //        return _commentListQuery.Execute() ?? new List<CommentDTO>();
        //    }
        //}

        //public IEnumerable<CommentDTO> GetAllCommentsContainting(string text)
        //{
        //    using (UnitOfWorkProvider.Create())
        //    {
        //        _commentListQuery.Filter = new CommentFilter() { Text = text };
        //        return _commentListQuery.Execute() ?? new List<CommentDTO>();
        //    }
        //}

        public CommentListQueryResultDTO GetComments(CommentFilter filter, int requiredPage = 1)
        {
            using (UnitOfWorkProvider.Create())
            {
                var query = _commentListQuery;
                query.ClearSortCriterias();
                query.Filter = filter;
                if (requiredPage > 0)
                {
                    query.Skip = Math.Max(0, requiredPage - 1)*CommentsPageSize;
                    query.Take = CommentsPageSize;
                }
                else
                {
                    query.Skip = null;
                    query.Take = null;
                }
                query.AddSortCriteria(nameof(Comment.Time),SortDirection.Descending);

                return new CommentListQueryResultDTO()
                {
                    RequestedPage = requiredPage,
                    Filter = filter,
                    TotalResultCount = query.GetTotalRowCount(),
                    ResultPage = query.Execute()
                };
            }
        }


        private Auction GetCommentAuction(long auctionId)
        {
            var auction = _auctionRepository.GetById(auctionId);
            if (auction == null)
            {
                throw new NullReferenceException($"{this.GetType().Name} - {nameof(GetCommentAuction)} auction cannot be null.");
            }
            return auction;
        }

        private User GetCommentAuthor(long authorId)
        {
            var author = _userRepository.GetById(authorId);
            if (author == null)
            {
                throw new NullReferenceException($"{this.GetType().Name} - {nameof(GetCommentAuthor)} author cannot be null.");
            }
            return author;
        }
    }
}
