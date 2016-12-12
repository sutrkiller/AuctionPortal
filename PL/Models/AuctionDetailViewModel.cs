using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BL.DTOs.Auctions;
using BL.DTOs.Categories;
using BL.DTOs.Comments;
using BL.DTOs.Items;
using BL.DTOs.Users;
using X.PagedList;

namespace PL.Models
{
    public class AuctionDetailViewModel
    {
        public AuctionDTO Auction { get; set; }
        public UserDTO Author { get; set; }
        public CategoryDTO Category { get; set; }
        public IList<ItemViewModel> Items { get; set; }
        public IPagedList<CommentDTO> Comments { get; set; }
        public decimal Bid { get; set; }
    }
}