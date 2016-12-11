using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.DTOs.Auctions;
using BL.DTOs.Bids;
using BL.DTOs.Filters;

namespace BL.Services.Auctions
{
    public interface IAuctionService
    {
        int AuctionsPageSize { get; }
        int BidsPageSize { get; }
        double MinBidRisePercentage { get; }
        double OneClickBuyMultiplier { get; }
        long CreateAuction(AuctionCreateDTO auctionDTO);
        void DeleteAuction(long auctionId);
        AuctionDTO GetAuction(long auctionId, bool increaseViews);

        void CreateBid(BidDTO bidDTO);
        decimal GetMinimumBid(long auctionId);
        decimal GetOneClickBuyPrice(long auctionId);
        AuctionListQueryResultDTO GetAuctions(AuctionFilter filter, int requiredPage = 1);
        BidListQueryResultDTO GetBids(BidFilter filter, int requiredPage = 1);

    }
}
