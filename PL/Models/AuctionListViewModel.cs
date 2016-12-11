using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BL.DTOs.Auctions;
using BL.DTOs.Categories;
using BL.DTOs.Filters;
using BL.Utils.Enums;
using X.PagedList;

namespace PL.Models
{
    public class AuctionListViewModel
    {
        public IList<CategoryDTO> Categories { get; set; }
        public IPagedList<AuctionDTO> Auctions { get; set; }
        public AuctionFilter Filter { get; set; }
        public SelectList AllSortCriteria => new SelectList(Enum.GetNames(typeof(AuctionSortCriteria)).ToList());
    }
}