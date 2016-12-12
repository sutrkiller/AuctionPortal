using System.Linq;
using AutoMapper.QueryableExtensions;
using BL.AppInfrastructure;
using BL.DTOs.Comments;
using BL.DTOs.Filters;
using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;

namespace BL.Queries
{
    public class CommentListQuery : AppQuery<CommentDTO>
    {

        public CommentFilter Filter { get; set; }

        public CommentListQuery(IUnitOfWorkProvider provider) : base(provider)
        {
        }

        protected override IQueryable<CommentDTO> GetQueryable()
        {
            IQueryable<Comment> query = Context.Comments;

            if (Filter.ID > 0)
            {
                query = query.Where(q => q.ID == Filter.ID);
            }
            if (Filter.AuctionId > 0)
            {
                query = query.Where(q => q.Auction.ID == Filter.AuctionId);
            }
            if (Filter.AuthorId > 0)
            {
                query = query.Where(q => q.Author.ID == Filter.AuthorId);
            }
            if ((Filter.ParentId ?? -1) > 0)
            {
                query = query.Where(q => q.ParentComment != null && q.ParentComment.ID == Filter.ParentId);
            }
            if (Filter.OnlyParent)
            {
                query = query.Where(x => x.ParentComment == null);
            }
            if (!string.IsNullOrEmpty(Filter.Text))
            {
                query = query.Where(q => q.Text.Contains(Filter.Text));
            }
            //if (Filter.Before != default(DateTime))
            //{
            //    query = query.Where(q => q.Time.Date < Filter.Before.Date);
            //}
            //if (Filter.After != default(DateTime))
            //{
            //    query = query.Where(q => q.Time.Date > Filter.After.Date);
            //}
            //if (Filter.Date != default(DateTime))
            //{
            //    query = query.Where(q => q.Time.Date == Filter.Date.Date);
            //}

            return query.ProjectTo<CommentDTO>();
        }
    }
}
